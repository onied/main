import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:onied_mobile/app/injection.dart';
import 'package:onied_mobile/blocs/catalog/catalog_bloc.dart';
import 'package:onied_mobile/blocs/catalog/catalog_bloc_event.dart';
import 'package:onied_mobile/blocs/catalog/catalog_bloc_state.dart';
import 'package:onied_mobile/components/course_card/course_card.dart';
import 'package:onied_mobile/providers/courses_provider.dart';
import 'package:onied_mobile/repositories/course_repository.dart';
import 'components/search_mode_app_bar.dart';

class CatalogPage extends StatelessWidget {
  const CatalogPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) => CatalogBloc(
            courseRepository: CourseRepository(getIt<CourseProvider>()),
          )..add(InitializeCourses()),
      child: BlocBuilder<CatalogBloc, CatalogBlocState>(
        builder: (context, state) {
          return switch (state) {
            LoadingState() => const Center(child: CircularProgressIndicator()),
            LoadedState(:final searchResults) => Scaffold(
              appBar: SearchModeAppBar(
                searchQuery: state.query,
                categories: state.categories,
                currentSearchFilters: state.searchFilters,
                onSearchChanged:
                    (query) => context.read<CatalogBloc>().add(
                      UpdateSearchQuery(query),
                    ),
                onSearchFiltersChanged:
                    (newSearchFilters) => context.read<CatalogBloc>().add(
                      UpdateSearchFilters(newSearchFilters),
                    ),
              ),
              body: ListView.builder(
                padding: EdgeInsets.only(top: 12),
                itemCount: searchResults.length,
                itemBuilder: (context, index) {
                  final course = state.searchResults[index];
                  return CourseCard(courseCard: course);
                },
              ),
            ),
            ErrorState(:final errorMessage) => Center(
              child: Text(errorMessage),
            ),
            _ => const Center(child: Text("Something went wrong.")),
          };
        },
      ),
    );
  }
}
