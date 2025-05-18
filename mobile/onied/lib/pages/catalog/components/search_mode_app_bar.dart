import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/models/search_filters_model.dart';
import 'package:onied_mobile/repositories/course_repository.dart';
import 'search_filters.dart';

class SearchModeAppBar extends StatefulWidget implements PreferredSizeWidget {
  final ValueChanged<String> onSearchChanged;
  final void Function(SearchFiltersModel) onSearchFiltersChanged;
  final Iterable<CategoryModel> categories;
  final SearchFiltersModel currentSearchFilters;

  const SearchModeAppBar({
    super.key,
    required this.onSearchChanged,
    required this.onSearchFiltersChanged,
    required this.categories,
    required this.currentSearchFilters,
  });

  @override
  State<SearchModeAppBar> createState() => _SearchModeAppBarState();

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}

class _SearchModeAppBarState extends State<SearchModeAppBar> {
  final TextEditingController _searchController = TextEditingController();

  void _openFilterPanel() async {
    final SearchFiltersModel? searchFilters =
        await showModalBottomSheet<SearchFiltersModel>(
          context: context,
          builder:
              (context) => FilterBottomSheet(
                categories: widget.categories,
                currentSearchFilters: widget.currentSearchFilters,
              ),
        );
    if (searchFilters != null) {
      widget.onSearchFiltersChanged(searchFilters);
    }
  }

  @override
  Widget build(BuildContext context) {
    return AppBar(
      automaticallyImplyLeading: false,
      elevation: 0,
      backgroundColor: Colors.white,
      titleSpacing: 16,
      title: TextField(
        controller: _searchController,
        autofocus: true,
        decoration: const InputDecoration(
          hintText: 'Введите запрос...',
          border: InputBorder.none,
        ),
        onChanged: widget.onSearchChanged,
      ),
      actions: [
        IconButton(
          icon: const Icon(Icons.filter_list),
          onPressed: _openFilterPanel,
        ),
        IconButton(
          icon: const Icon(Icons.close),
          onPressed: () => context.pop(),
        ),
      ],
    );
  }
}
