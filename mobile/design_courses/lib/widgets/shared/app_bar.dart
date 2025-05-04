import 'package:flutter/material.dart';
import 'package:design_courses/widgets/catalog/search_filters.dart';

class CustomAppBar extends StatefulWidget implements PreferredSizeWidget {
  const CustomAppBar({super.key});

  @override
  State<CustomAppBar> createState() => _CustomAppBarState();

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);
}

class _CustomAppBarState extends State<CustomAppBar> {
  bool _isSearchMode = false;
  final TextEditingController _searchController = TextEditingController();

  void _enterSearchMode() {
    setState(() => _isSearchMode = true);
  }

  void _exitSearchMode() {
    setState(() => _isSearchMode = false);
    _searchController.clear();
  }

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
      title:
          _isSearchMode
              ? TextField(
                controller: _searchController,
                autofocus: true,
                decoration: const InputDecoration(
                  hintText: 'Введите запрос...',
                  border: InputBorder.none,
                ),
                onChanged: (value) {},
              )
              : const Text(
                'OniEd',
                style: TextStyle(
                  fontFamily: 'Comic Sans MS',
                  fontSize: 32,
                  color: Colors.black,
                ),
              ),
      actions:
          _isSearchMode
              ? [
                IconButton(
                  icon: const Icon(Icons.filter_list),
                  onPressed: _openFilterPanel,
                ),
                IconButton(
                  icon: const Icon(Icons.close),
                  onPressed: _exitSearchMode,
                ),
              ]
              : [
                IconButton(
                  onPressed: _enterSearchMode,
                  icon: const Icon(Icons.search, color: Colors.black, size: 30),
                ),
                IconButton(
                  onPressed: () {},
                  icon: const Icon(
                    Icons.account_circle_outlined,
                    color: Colors.black,
                    size: 45,
                  ),
                ),
                Padding(padding: EdgeInsets.only(right: 8)),
              ],
    );
  }
}
