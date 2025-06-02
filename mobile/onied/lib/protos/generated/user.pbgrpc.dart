//
//  Generated code. Do not modify.
//  source: user.proto
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

import 'user.pb.dart' as $0;

export 'user.pb.dart';

@$pb.GrpcServiceName('usersGrpc.AuthorizationService')
class AuthorizationServiceClient extends $grpc.Client {
  /// The hostname for this service.
  static const $core.String defaultHost = '';

  /// OAuth scopes needed for the client.
  static const $core.List<$core.String> oauthScopes = [
    '',
  ];

  static final _$getCurrentUserId =
      $grpc.ClientMethod<$0.Empty, $0.UserIdResponse>(
          '/usersGrpc.AuthorizationService/GetCurrentUserId',
          ($0.Empty value) => value.writeToBuffer(),
          ($core.List<$core.int> value) => $0.UserIdResponse.fromBuffer(value));

  AuthorizationServiceClient(super.channel,
      {super.options, super.interceptors});

  $grpc.ResponseFuture<$0.UserIdResponse> getCurrentUserId($0.Empty request,
      {$grpc.CallOptions? options}) {
    return $createUnaryCall(_$getCurrentUserId, request, options: options);
  }
}

@$pb.GrpcServiceName('usersGrpc.AuthorizationService')
abstract class AuthorizationServiceBase extends $grpc.Service {
  $core.String get $name => 'usersGrpc.AuthorizationService';

  AuthorizationServiceBase() {
    $addMethod($grpc.ServiceMethod<$0.Empty, $0.UserIdResponse>(
        'GetCurrentUserId',
        getCurrentUserId_Pre,
        false,
        false,
        ($core.List<$core.int> value) => $0.Empty.fromBuffer(value),
        ($0.UserIdResponse value) => value.writeToBuffer()));
  }

  $async.Future<$0.UserIdResponse> getCurrentUserId_Pre(
      $grpc.ServiceCall $call, $async.Future<$0.Empty> $request) async {
    return getCurrentUserId($call, await $request);
  }

  $async.Future<$0.UserIdResponse> getCurrentUserId(
      $grpc.ServiceCall call, $0.Empty request);
}
