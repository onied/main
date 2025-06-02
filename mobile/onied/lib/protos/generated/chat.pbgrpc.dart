//
//  Generated code. Do not modify.
//  source: chat.proto
//
// @dart = 3.3

// ignore_for_file: annotate_overrides, camel_case_types, comment_references
// ignore_for_file: constant_identifier_names
// ignore_for_file: curly_braces_in_flow_control_structures
// ignore_for_file: deprecated_member_use_from_same_package, library_prefixes
// ignore_for_file: non_constant_identifier_names

import 'dart:async' as $async;
import 'dart:core' as $core;

import 'package:grpc/service_api.dart' as $grpc;
import 'package:protobuf/protobuf.dart' as $pb;

import 'chat.pb.dart' as $0;

export 'chat.pb.dart';

@$pb.GrpcServiceName('supportChatGrpc.UserChatService')
class UserChatServiceClient extends $grpc.Client {
  /// The hostname for this service.
  static const $core.String defaultHost = '';

  /// OAuth scopes needed for the client.
  static const $core.List<$core.String> oauthScopes = [
    '',
  ];

  static final _$sendMessage =
      $grpc.ClientMethod<$0.SendMessageRequest, $0.Empty>(
          '/supportChatGrpc.UserChatService/SendMessage',
          ($0.SendMessageRequest value) => value.writeToBuffer(),
          ($core.List<$core.int> value) => $0.Empty.fromBuffer(value));
  static final _$markMessageAsRead =
      $grpc.ClientMethod<$0.MarkMessageAsReadRequest, $0.Empty>(
          '/supportChatGrpc.UserChatService/MarkMessageAsRead',
          ($0.MarkMessageAsReadRequest value) => value.writeToBuffer(),
          ($core.List<$core.int> value) => $0.Empty.fromBuffer(value));
  static final _$receiveMessage =
      $grpc.ClientMethod<$0.Empty, $0.MessageResponse>(
          '/supportChatGrpc.UserChatService/ReceiveMessage',
          ($0.Empty value) => value.writeToBuffer(),
          ($core.List<$core.int> value) =>
              $0.MessageResponse.fromBuffer(value));
  static final _$receiveReadAt =
      $grpc.ClientMethod<$0.Empty, $0.ReadAtResponse>(
          '/supportChatGrpc.UserChatService/ReceiveReadAt',
          ($0.Empty value) => value.writeToBuffer(),
          ($core.List<$core.int> value) => $0.ReadAtResponse.fromBuffer(value));

  UserChatServiceClient(super.channel, {super.options, super.interceptors});

  $grpc.ResponseFuture<$0.Empty> sendMessage($0.SendMessageRequest request,
      {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$sendMessage, request, options: options);
  }

  $grpc.ResponseFuture<$0.Empty> markMessageAsRead(
      $0.MarkMessageAsReadRequest request,
      {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$markMessageAsRead, request, options: options);
  }

  $grpc.ResponseStream<$0.MessageResponse> receiveMessage($0.Empty request,
      {$grpc.CallOptions? options}) {
    return $createStreamingCall(
        _$receiveMessage, $async.Stream.fromIterable([request]),
        options: options);
  }

  $grpc.ResponseStream<$0.ReadAtResponse> receiveReadAt($0.Empty request,
      {$grpc.CallOptions? options}) {
    return $createStreamingCall(
        _$receiveReadAt, $async.Stream.fromIterable([request]),
        options: options);
  }
}

@$pb.GrpcServiceName('supportChatGrpc.UserChatService')
abstract class UserChatServiceBase extends $grpc.Service {
  $core.String get $name => 'supportChatGrpc.UserChatService';

  UserChatServiceBase() {
    $addMethod($grpc.ServiceMethod<$0.SendMessageRequest, $0.Empty>(
        'SendMessage',
        sendMessage_Pre,
        false,
        false,
        ($core.List<$core.int> value) =>
            $0.SendMessageRequest.fromBuffer(value),
        ($0.Empty value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.MarkMessageAsReadRequest, $0.Empty>(
        'MarkMessageAsRead',
        markMessageAsRead_Pre,
        false,
        false,
        ($core.List<$core.int> value) =>
            $0.MarkMessageAsReadRequest.fromBuffer(value),
        ($0.Empty value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.Empty, $0.MessageResponse>(
        'ReceiveMessage',
        receiveMessage_Pre,
        false,
        true,
        ($core.List<$core.int> value) => $0.Empty.fromBuffer(value),
        ($0.MessageResponse value) => value.writeToBuffer()));
    $addMethod($grpc.ServiceMethod<$0.Empty, $0.ReadAtResponse>(
        'ReceiveReadAt',
        receiveReadAt_Pre,
        false,
        true,
        ($core.List<$core.int> value) => $0.Empty.fromBuffer(value),
        ($0.ReadAtResponse value) => value.writeToBuffer()));
  }

  $async.Future<$0.Empty> sendMessage_Pre($grpc.ServiceCall $call,
      $async.Future<$0.SendMessageRequest> $request) async {
    return sendMessage($call, await $request);
  }

  $async.Future<$0.Empty> markMessageAsRead_Pre($grpc.ServiceCall $call,
      $async.Future<$0.MarkMessageAsReadRequest> $request) async {
    return markMessageAsRead($call, await $request);
  }

  $async.Stream<$0.MessageResponse> receiveMessage_Pre(
      $grpc.ServiceCall $call, $async.Future<$0.Empty> $request) async* {
    yield* receiveMessage($call, await $request);
  }

  $async.Stream<$0.ReadAtResponse> receiveReadAt_Pre(
      $grpc.ServiceCall $call, $async.Future<$0.Empty> $request) async* {
    yield* receiveReadAt($call, await $request);
  }

  $async.Future<$0.Empty> sendMessage(
      $grpc.ServiceCall call, $0.SendMessageRequest request);
  $async.Future<$0.Empty> markMessageAsRead(
      $grpc.ServiceCall call, $0.MarkMessageAsReadRequest request);
  $async.Stream<$0.MessageResponse> receiveMessage(
      $grpc.ServiceCall call, $0.Empty request);
  $async.Stream<$0.ReadAtResponse> receiveReadAt(
      $grpc.ServiceCall call, $0.Empty request);
}
