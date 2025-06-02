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

import 'dart:core' as $core;

import 'package:protobuf/protobuf.dart' as $pb;

import 'timestamp.pb.dart' as $1;

export 'package:protobuf/protobuf.dart' show GeneratedMessageGenericExtensions;

class SendMessageRequest extends $pb.GeneratedMessage {
  factory SendMessageRequest({
    $core.String? messageContent,
  }) {
    final result = create();
    if (messageContent != null) result.messageContent = messageContent;
    return result;
  }

  SendMessageRequest._();

  factory SendMessageRequest.fromBuffer($core.List<$core.int> data,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromBuffer(data, registry);
  factory SendMessageRequest.fromJson($core.String json,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromJson(json, registry);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(
      _omitMessageNames ? '' : 'SendMessageRequest',
      package:
          const $pb.PackageName(_omitMessageNames ? '' : 'supportChatGrpc'),
      createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'messageContent',
        protoName: 'messageContent')
    ..hasRequiredFields = false;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SendMessageRequest clone() => SendMessageRequest()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  SendMessageRequest copyWith(void Function(SendMessageRequest) updates) =>
      super.copyWith((message) => updates(message as SendMessageRequest))
          as SendMessageRequest;

  @$core.override
  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static SendMessageRequest create() => SendMessageRequest._();
  @$core.override
  SendMessageRequest createEmptyInstance() => create();
  static $pb.PbList<SendMessageRequest> createRepeated() =>
      $pb.PbList<SendMessageRequest>();
  @$core.pragma('dart2js:noInline')
  static SendMessageRequest getDefault() => _defaultInstance ??=
      $pb.GeneratedMessage.$_defaultFor<SendMessageRequest>(create);
  static SendMessageRequest? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get messageContent => $_getSZ(0);
  @$pb.TagNumber(1)
  set messageContent($core.String value) => $_setString(0, value);
  @$pb.TagNumber(1)
  $core.bool hasMessageContent() => $_has(0);
  @$pb.TagNumber(1)
  void clearMessageContent() => $_clearField(1);
}

class MarkMessageAsReadRequest extends $pb.GeneratedMessage {
  factory MarkMessageAsReadRequest({
    $core.String? messageId,
  }) {
    final result = create();
    if (messageId != null) result.messageId = messageId;
    return result;
  }

  MarkMessageAsReadRequest._();

  factory MarkMessageAsReadRequest.fromBuffer($core.List<$core.int> data,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromBuffer(data, registry);
  factory MarkMessageAsReadRequest.fromJson($core.String json,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromJson(json, registry);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(
      _omitMessageNames ? '' : 'MarkMessageAsReadRequest',
      package:
          const $pb.PackageName(_omitMessageNames ? '' : 'supportChatGrpc'),
      createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'messageId', protoName: 'messageId')
    ..hasRequiredFields = false;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  MarkMessageAsReadRequest clone() =>
      MarkMessageAsReadRequest()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  MarkMessageAsReadRequest copyWith(
          void Function(MarkMessageAsReadRequest) updates) =>
      super.copyWith((message) => updates(message as MarkMessageAsReadRequest))
          as MarkMessageAsReadRequest;

  @$core.override
  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static MarkMessageAsReadRequest create() => MarkMessageAsReadRequest._();
  @$core.override
  MarkMessageAsReadRequest createEmptyInstance() => create();
  static $pb.PbList<MarkMessageAsReadRequest> createRepeated() =>
      $pb.PbList<MarkMessageAsReadRequest>();
  @$core.pragma('dart2js:noInline')
  static MarkMessageAsReadRequest getDefault() => _defaultInstance ??=
      $pb.GeneratedMessage.$_defaultFor<MarkMessageAsReadRequest>(create);
  static MarkMessageAsReadRequest? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get messageId => $_getSZ(0);
  @$pb.TagNumber(1)
  set messageId($core.String value) => $_setString(0, value);
  @$pb.TagNumber(1)
  $core.bool hasMessageId() => $_has(0);
  @$pb.TagNumber(1)
  void clearMessageId() => $_clearField(1);
}

class MessageResponse extends $pb.GeneratedMessage {
  factory MessageResponse({
    $core.String? messageId,
    $core.int? supportNumber,
    $1.Timestamp? createdAt,
    $core.String? message,
    $core.bool? isSystem,
  }) {
    final result = create();
    if (messageId != null) result.messageId = messageId;
    if (supportNumber != null) result.supportNumber = supportNumber;
    if (createdAt != null) result.createdAt = createdAt;
    if (message != null) result.message = message;
    if (isSystem != null) result.isSystem = isSystem;
    return result;
  }

  MessageResponse._();

  factory MessageResponse.fromBuffer($core.List<$core.int> data,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromBuffer(data, registry);
  factory MessageResponse.fromJson($core.String json,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromJson(json, registry);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(
      _omitMessageNames ? '' : 'MessageResponse',
      package:
          const $pb.PackageName(_omitMessageNames ? '' : 'supportChatGrpc'),
      createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'messageId', protoName: 'messageId')
    ..a<$core.int>(
        2, _omitFieldNames ? '' : 'supportNumber', $pb.PbFieldType.O3,
        protoName: 'supportNumber')
    ..aOM<$1.Timestamp>(3, _omitFieldNames ? '' : 'createdAt',
        protoName: 'createdAt', subBuilder: $1.Timestamp.create)
    ..aOS(4, _omitFieldNames ? '' : 'message')
    ..aOB(5, _omitFieldNames ? '' : 'isSystem', protoName: 'isSystem')
    ..hasRequiredFields = false;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  MessageResponse clone() => MessageResponse()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  MessageResponse copyWith(void Function(MessageResponse) updates) =>
      super.copyWith((message) => updates(message as MessageResponse))
          as MessageResponse;

  @$core.override
  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static MessageResponse create() => MessageResponse._();
  @$core.override
  MessageResponse createEmptyInstance() => create();
  static $pb.PbList<MessageResponse> createRepeated() =>
      $pb.PbList<MessageResponse>();
  @$core.pragma('dart2js:noInline')
  static MessageResponse getDefault() => _defaultInstance ??=
      $pb.GeneratedMessage.$_defaultFor<MessageResponse>(create);
  static MessageResponse? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get messageId => $_getSZ(0);
  @$pb.TagNumber(1)
  set messageId($core.String value) => $_setString(0, value);
  @$pb.TagNumber(1)
  $core.bool hasMessageId() => $_has(0);
  @$pb.TagNumber(1)
  void clearMessageId() => $_clearField(1);

  @$pb.TagNumber(2)
  $core.int get supportNumber => $_getIZ(1);
  @$pb.TagNumber(2)
  set supportNumber($core.int value) => $_setSignedInt32(1, value);
  @$pb.TagNumber(2)
  $core.bool hasSupportNumber() => $_has(1);
  @$pb.TagNumber(2)
  void clearSupportNumber() => $_clearField(2);

  @$pb.TagNumber(3)
  $1.Timestamp get createdAt => $_getN(2);
  @$pb.TagNumber(3)
  set createdAt($1.Timestamp value) => $_setField(3, value);
  @$pb.TagNumber(3)
  $core.bool hasCreatedAt() => $_has(2);
  @$pb.TagNumber(3)
  void clearCreatedAt() => $_clearField(3);
  @$pb.TagNumber(3)
  $1.Timestamp ensureCreatedAt() => $_ensure(2);

  @$pb.TagNumber(4)
  $core.String get message => $_getSZ(3);
  @$pb.TagNumber(4)
  set message($core.String value) => $_setString(3, value);
  @$pb.TagNumber(4)
  $core.bool hasMessage() => $_has(3);
  @$pb.TagNumber(4)
  void clearMessage() => $_clearField(4);

  @$pb.TagNumber(5)
  $core.bool get isSystem => $_getBF(4);
  @$pb.TagNumber(5)
  set isSystem($core.bool value) => $_setBool(4, value);
  @$pb.TagNumber(5)
  $core.bool hasIsSystem() => $_has(4);
  @$pb.TagNumber(5)
  void clearIsSystem() => $_clearField(5);
}

class ReadAtResponse extends $pb.GeneratedMessage {
  factory ReadAtResponse({
    $core.String? messageId,
    $1.Timestamp? readAt,
  }) {
    final result = create();
    if (messageId != null) result.messageId = messageId;
    if (readAt != null) result.readAt = readAt;
    return result;
  }

  ReadAtResponse._();

  factory ReadAtResponse.fromBuffer($core.List<$core.int> data,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromBuffer(data, registry);
  factory ReadAtResponse.fromJson($core.String json,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromJson(json, registry);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(
      _omitMessageNames ? '' : 'ReadAtResponse',
      package:
          const $pb.PackageName(_omitMessageNames ? '' : 'supportChatGrpc'),
      createEmptyInstance: create)
    ..aOS(1, _omitFieldNames ? '' : 'messageId', protoName: 'messageId')
    ..aOM<$1.Timestamp>(2, _omitFieldNames ? '' : 'readAt',
        protoName: 'readAt', subBuilder: $1.Timestamp.create)
    ..hasRequiredFields = false;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ReadAtResponse clone() => ReadAtResponse()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  ReadAtResponse copyWith(void Function(ReadAtResponse) updates) =>
      super.copyWith((message) => updates(message as ReadAtResponse))
          as ReadAtResponse;

  @$core.override
  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static ReadAtResponse create() => ReadAtResponse._();
  @$core.override
  ReadAtResponse createEmptyInstance() => create();
  static $pb.PbList<ReadAtResponse> createRepeated() =>
      $pb.PbList<ReadAtResponse>();
  @$core.pragma('dart2js:noInline')
  static ReadAtResponse getDefault() => _defaultInstance ??=
      $pb.GeneratedMessage.$_defaultFor<ReadAtResponse>(create);
  static ReadAtResponse? _defaultInstance;

  @$pb.TagNumber(1)
  $core.String get messageId => $_getSZ(0);
  @$pb.TagNumber(1)
  set messageId($core.String value) => $_setString(0, value);
  @$pb.TagNumber(1)
  $core.bool hasMessageId() => $_has(0);
  @$pb.TagNumber(1)
  void clearMessageId() => $_clearField(1);

  @$pb.TagNumber(2)
  $1.Timestamp get readAt => $_getN(1);
  @$pb.TagNumber(2)
  set readAt($1.Timestamp value) => $_setField(2, value);
  @$pb.TagNumber(2)
  $core.bool hasReadAt() => $_has(1);
  @$pb.TagNumber(2)
  void clearReadAt() => $_clearField(2);
  @$pb.TagNumber(2)
  $1.Timestamp ensureReadAt() => $_ensure(1);
}

class Empty extends $pb.GeneratedMessage {
  factory Empty() => create();

  Empty._();

  factory Empty.fromBuffer($core.List<$core.int> data,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromBuffer(data, registry);
  factory Empty.fromJson($core.String json,
          [$pb.ExtensionRegistry registry = $pb.ExtensionRegistry.EMPTY]) =>
      create()..mergeFromJson(json, registry);

  static final $pb.BuilderInfo _i = $pb.BuilderInfo(
      _omitMessageNames ? '' : 'Empty',
      package:
          const $pb.PackageName(_omitMessageNames ? '' : 'supportChatGrpc'),
      createEmptyInstance: create)
    ..hasRequiredFields = false;

  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  Empty clone() => Empty()..mergeFromMessage(this);
  @$core.Deprecated('See https://github.com/google/protobuf.dart/issues/998.')
  Empty copyWith(void Function(Empty) updates) =>
      super.copyWith((message) => updates(message as Empty)) as Empty;

  @$core.override
  $pb.BuilderInfo get info_ => _i;

  @$core.pragma('dart2js:noInline')
  static Empty create() => Empty._();
  @$core.override
  Empty createEmptyInstance() => create();
  static $pb.PbList<Empty> createRepeated() => $pb.PbList<Empty>();
  @$core.pragma('dart2js:noInline')
  static Empty getDefault() =>
      _defaultInstance ??= $pb.GeneratedMessage.$_defaultFor<Empty>(create);
  static Empty? _defaultInstance;
}

const $core.bool _omitFieldNames =
    $core.bool.fromEnvironment('protobuf.omit_field_names');
const $core.bool _omitMessageNames =
    $core.bool.fromEnvironment('protobuf.omit_message_names');
