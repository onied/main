import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';

import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/blocs/course/course_bloc.dart';
import 'package:onied_mobile/blocs/course/course_bloc_event.dart';
import 'package:onied_mobile/blocs/course/course_bloc_state.dart';
import 'package:onied_mobile/repositories/course_repository.dart';
import 'components/block_view.dart';
import 'components/course_sidebar.dart';

class CoursePage extends StatelessWidget {
  final int id;

  const CoursePage({super.key, required this.id});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) =>
              CourseBloc(courseRepository: CourseRepository())
                ..add(LoadHierarchy(courseId: id)),
      child: Theme(
        data: AppTheme.dataCourseBlocks,
        child: BlocBuilder<CourseBloc, CourseBlocState>(
          builder: (context, state) {
            return switch (state) {
              LoadingState() => const Center(
                child: CircularProgressIndicator(),
              ),
              ErrorState(:final errorMessage) => Center(
                child: Text(errorMessage),
              ),
              LoadedState(
                :final hierarchy,
                :final block,
                :final isBlockLoading,
              ) =>
                Scaffold(
                  appBar: AppBar(
                    title: Text(
                      hierarchy.title,
                      style: Theme.of(
                        context,
                      ).textTheme.titleLarge?.copyWith(color: Colors.white),
                    ),
                    backgroundColor: AppTheme.backgroundColorHeader,
                  ),
                  drawer: Drawer(
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.zero,
                    ),
                    child: CourseSidebar(
                      hierarchy: hierarchy,
                      selectedBlockId: block?.id,
                      onBlockSelected: (blockId) {
                        context.read<CourseBloc>().add(
                          LoadBlock(blockId: blockId),
                        );
                        Navigator.pop(context);
                      },
                    ),
                  ),
                  body: Row(
                    children: [
                      Expanded(
                        child:
                            isBlockLoading
                                ? const Center(
                                  child: CircularProgressIndicator(),
                                )
                                : block != null
                                ? BlockView(block: block)
                                : Center(
                                  child: Text(
                                    "Выберите блок",
                                    style:
                                        Theme.of(context).textTheme.titleLarge,
                                  ),
                                ),
                      ),
                    ],
                  ),
                ),
              _ => const Center(child: Text("Something went wrong.")),
            };
          },
        ),
      ),
    );
  }
}
