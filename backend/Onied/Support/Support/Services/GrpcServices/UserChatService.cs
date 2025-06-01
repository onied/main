using System.Collections.Concurrent;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using MassTransit;
using Support.Dtos.Hub.Response;
using Support.Events.Messages;
using SupportChatGrpc;
using Empty = SupportChatGrpc.Empty;

namespace Support.Services.GrpcServices;

public class UserChatService(
    UsersGrpc.AuthorizationService.AuthorizationServiceClient authorizationService,
    IPublishEndpoint publishEndpoint,
    IMapper mapper) : SupportChatGrpc.UserChatService.UserChatServiceBase
{
    private static ConcurrentDictionary<Guid, ConcurrentQueue<MessageResponse>> UserToReceiveMessageQueue { get; } =
        new();

    private static ConcurrentDictionary<Guid, ConcurrentQueue<ReadAtResponse>> UserToReceiveMarkAsReadQueue { get; } =
        new();

    public void ReceiveMessageSender(Guid userId, HubMessageDto messageDto)
    {
        if (UserToReceiveMessageQueue.TryGetValue(userId, out var queue))
            queue.Enqueue(mapper.Map<MessageResponse>(messageDto));
    }

    public void ReceiveReadAtSender(Guid userId, Guid messageId, DateTime readAt)
    {
        if (UserToReceiveMarkAsReadQueue.TryGetValue(userId, out var queue))
            queue.Enqueue(new ReadAtResponse() { MessageId = messageId.ToString(), ReadAt = readAt.ToTimestamp() });
    }

    private async Task<Guid> GetUserId(Metadata requestHeaders)
    {
        try
        {
            var headers = new Metadata
            {
                {
                    "Authorization",
                    requestHeaders.FirstOrDefault(value =>
                        value.Key.Equals("authorization", StringComparison.CurrentCultureIgnoreCase))?.Value ?? ""
                }
            };
            var userIdResponse = await authorizationService.GetCurrentUserIdAsync(new UsersGrpc.Empty(), headers);
            var success = Guid.TryParse(userIdResponse.UserId, out var userId);
            if (!success)
                throw new Exception();

            return userId;
        }
        catch (Exception)
        {
            throw new RpcException(new Status(StatusCode.Unauthenticated, "Could not get user id"));
        }
    }

    public override async Task<Empty> SendMessage(SendMessageRequest request, ServerCallContext context)
    {
        var userId = await GetUserId(context.RequestHeaders);
        await publishEndpoint.Publish(new SendMessage(userId, request.MessageContent, []));
        // For now, sending files via gRPC is not supported
        return new Empty();
    }

    public override async Task<Empty> MarkMessageAsRead(MarkMessageAsReadRequest request, ServerCallContext context)
    {
        var userId = await GetUserId(context.RequestHeaders);
        var success = Guid.TryParse(request.MessageId, out var messageId);
        if (!success)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Could not parse messageId"));
        await publishEndpoint.Publish(new MarkMessageAsRead(userId, messageId));
        return new Empty();
    }

    public override async Task ReceiveMessage(Empty request, IServerStreamWriter<MessageResponse> responseStream,
        ServerCallContext context)
    {
        var userId = await GetUserId(context.RequestHeaders);
        if (userId != Guid.Empty)
            UserToReceiveMessageQueue[userId] = new ConcurrentQueue<MessageResponse>();
        while (!context.CancellationToken.IsCancellationRequested)
            while (UserToReceiveMessageQueue[userId].TryDequeue(out var messageResponse))
                await responseStream.WriteAsync(messageResponse, context.CancellationToken);
        UserToReceiveMessageQueue.TryRemove(userId, out _);
    }

    public override async Task ReceiveReadAt(Empty request, IServerStreamWriter<ReadAtResponse> responseStream,
        ServerCallContext context)
    {
        var userId = await GetUserId(context.RequestHeaders);
        if (userId != Guid.Empty)
            UserToReceiveMarkAsReadQueue[userId] = new ConcurrentQueue<ReadAtResponse>();
        while (!context.CancellationToken.IsCancellationRequested)
            while (UserToReceiveMarkAsReadQueue[userId].TryDequeue(out var readAtResponse))
                await responseStream.WriteAsync(readAtResponse, context.CancellationToken);
        UserToReceiveMarkAsReadQueue.TryRemove(userId, out _);
    }
}
