import 'package:flutter/material.dart';

import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/models/course_hierarchy_model.dart';

class CourseSidebar extends StatelessWidget {
  final CourseHierarchyModel hierarchy;
  final int? selectedBlockId;
  final Function(int) onBlockSelected;

  const CourseSidebar({
    super.key,
    required this.hierarchy,
    required this.selectedBlockId,
    required this.onBlockSelected,
  });

  @override
  Widget build(BuildContext context) {
    return Container(
      width: AppTheme.sidebarWidth,
      decoration: BoxDecoration(color: AppTheme.backgroundColorHeader),
      child: ListView.builder(
        itemCount: hierarchy.modules.length,
        itemBuilder: (context, moduleIndex) {
          final module = hierarchy.modules[moduleIndex];
          return Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Container(
                decoration: BoxDecoration(color: AppTheme.accent),
                child: ListTile(
                  title: Text(
                    "${moduleIndex + 1}. ${module.title}",
                    style: Theme.of(
                      context,
                    ).textTheme.titleLarge?.copyWith(color: Colors.white),
                  ),
                ),
              ),
              ...List.generate(module.blocks.length, (blockIndex) {
                final block = module.blocks[blockIndex];
                final isSelected = selectedBlockId == block.id;
                return Container(
                  decoration: BoxDecoration(
                    color: isSelected ? Color(0xFF383838) : Colors.transparent,
                  ),
                  child: ListTile(
                    title: Text(
                      "${moduleIndex + 1}.${blockIndex + 1} ${block.title}",
                      style: Theme.of(
                        context,
                      ).textTheme.titleLarge?.copyWith(color: Colors.white),
                    ),
                    onTap: () => onBlockSelected(block.id),
                  ),
                );
              }),
            ],
          );
        },
      ),
    );
  }
}
