import 'package:flutter/material.dart';
import 'package:design_courses/widgets/catalog/search_filters.dart';

class SearchModeAppBar extends StatefulWidget implements PreferredSizeWidget {
  const SearchModeAppBar({super.key});

  @override
  State<SearchModeAppBar> createState() => _SearchModeAppBarState();

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}

class _SearchModeAppBarState extends State<SearchModeAppBar> {
  final TextEditingController _searchController = TextEditingController();

  void _openFilterPanel() {
    showModalBottomSheet(
      context: context,
      builder: (context) => const FilterBottomSheet(),
    );
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
        onChanged: (value) {},
      ),
      actions: [
        IconButton(
          icon: const Icon(Icons.filter_list),
          onPressed: _openFilterPanel,
        ),
        IconButton(
          icon: const Icon(Icons.close),
          onPressed: () => Navigator.pop(context),
        ),
      ],
    );
  }
}
