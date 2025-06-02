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

import 'dart:convert' as $convert;
import 'dart:core' as $core;
import 'dart:typed_data' as $typed_data;

@$core.Deprecated('Use sendMessageRequestDescriptor instead')
const SendMessageRequest$json = {
  '1': 'SendMessageRequest',
  '2': [
    {'1': 'messageContent', '3': 1, '4': 1, '5': 9, '10': 'messageContent'},
  ],
};

/// Descriptor for `SendMessageRequest`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List sendMessageRequestDescriptor = $convert.base64Decode(
    'ChJTZW5kTWVzc2FnZVJlcXVlc3QSJgoObWVzc2FnZUNvbnRlbnQYASABKAlSDm1lc3NhZ2VDb2'
    '50ZW50');

@$core.Deprecated('Use markMessageAsReadRequestDescriptor instead')
const MarkMessageAsReadRequest$json = {
  '1': 'MarkMessageAsReadRequest',
  '2': [
    {'1': 'messageId', '3': 1, '4': 1, '5': 9, '10': 'messageId'},
  ],
};

/// Descriptor for `MarkMessageAsReadRequest`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List markMessageAsReadRequestDescriptor =
    $convert.base64Decode(
        'ChhNYXJrTWVzc2FnZUFzUmVhZFJlcXVlc3QSHAoJbWVzc2FnZUlkGAEgASgJUgltZXNzYWdlSW'
        'Q=');

@$core.Deprecated('Use messageResponseDescriptor instead')
const MessageResponse$json = {
  '1': 'MessageResponse',
  '2': [
    {'1': 'messageId', '3': 1, '4': 1, '5': 9, '10': 'messageId'},
    {
      '1': 'supportNumber',
      '3': 2,
      '4': 1,
      '5': 5,
      '9': 0,
      '10': 'supportNumber',
      '17': true
    },
    {
      '1': 'createdAt',
      '3': 3,
      '4': 1,
      '5': 11,
      '6': '.google.protobuf.Timestamp',
      '10': 'createdAt'
    },
    {'1': 'message', '3': 4, '4': 1, '5': 9, '10': 'message'},
    {'1': 'isSystem', '3': 5, '4': 1, '5': 8, '10': 'isSystem'},
  ],
  '8': [
    {'1': '_supportNumber'},
  ],
};

/// Descriptor for `MessageResponse`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List messageResponseDescriptor = $convert.base64Decode(
    'Cg9NZXNzYWdlUmVzcG9uc2USHAoJbWVzc2FnZUlkGAEgASgJUgltZXNzYWdlSWQSKQoNc3VwcG'
    '9ydE51bWJlchgCIAEoBUgAUg1zdXBwb3J0TnVtYmVyiAEBEjgKCWNyZWF0ZWRBdBgDIAEoCzIa'
    'Lmdvb2dsZS5wcm90b2J1Zi5UaW1lc3RhbXBSCWNyZWF0ZWRBdBIYCgdtZXNzYWdlGAQgASgJUg'
    'dtZXNzYWdlEhoKCGlzU3lzdGVtGAUgASgIUghpc1N5c3RlbUIQCg5fc3VwcG9ydE51bWJlcg==');

@$core.Deprecated('Use readAtResponseDescriptor instead')
const ReadAtResponse$json = {
  '1': 'ReadAtResponse',
  '2': [
    {'1': 'messageId', '3': 1, '4': 1, '5': 9, '10': 'messageId'},
    {
      '1': 'readAt',
      '3': 2,
      '4': 1,
      '5': 11,
      '6': '.google.protobuf.Timestamp',
      '10': 'readAt'
    },
  ],
};

/// Descriptor for `ReadAtResponse`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List readAtResponseDescriptor = $convert.base64Decode(
    'Cg5SZWFkQXRSZXNwb25zZRIcCgltZXNzYWdlSWQYASABKAlSCW1lc3NhZ2VJZBIyCgZyZWFkQX'
    'QYAiABKAsyGi5nb29nbGUucHJvdG9idWYuVGltZXN0YW1wUgZyZWFkQXQ=');

@$core.Deprecated('Use emptyDescriptor instead')
const Empty$json = {
  '1': 'Empty',
};

/// Descriptor for `Empty`. Decode as a `google.protobuf.DescriptorProto`.
final $typed_data.Uint8List emptyDescriptor =
    $convert.base64Decode('CgVFbXB0eQ==');
