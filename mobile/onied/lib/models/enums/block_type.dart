enum BlockType { anyBlock, summaryBlock, videoBlock, tasksBlock }

BlockType blockTypeFromString(String value) {
  switch (value.toUpperCase()) {
    case 'SUMMARY_BLOCK':
      return BlockType.summaryBlock;
    case 'VIDEO_BLOCK':
      return BlockType.videoBlock;
    case 'TASKS_BLOCK':
      return BlockType.tasksBlock;
    default:
      return BlockType.anyBlock;
  }
}

String graphqlDataFieldFromBlockType(BlockType blockType) {
  switch (blockType) {
    case BlockType.summaryBlock:
      return "summaryBlockById";
    case BlockType.videoBlock:
      return "videoBlockById";
    case BlockType.tasksBlock:
      return "tasksBlockById";
    case BlockType.anyBlock:
      throw UnimplementedError("Can't get block without specified type");
  }
}
