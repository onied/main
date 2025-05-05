import 'package:design_courses/pages/search_results_page.dart';
import 'package:flutter/material.dart';

class CustomAppBar extends StatelessWidget implements PreferredSizeWidget {
  const CustomAppBar({super.key});

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);

  @override
  Widget build(BuildContext context) {
    return AppBar(
      automaticallyImplyLeading: false,
      elevation: 0,
      backgroundColor: Colors.white,
      titleSpacing: 16,
      title: const Text(
        'OniEd',
        style: TextStyle(
          fontFamily: 'Comic Sans MS',
          fontSize: 32,
          color: Colors.black,
        ),
      ),
      actions: [
        IconButton(
          onPressed:
              () => Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => SearchResultsPage()),
              ),
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
