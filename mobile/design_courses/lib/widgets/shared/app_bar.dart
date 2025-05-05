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
      backgroundColor: Color(0xFF383737),
      elevation: 0,
      titleSpacing: 16,
      title: InkWell(
        onTap: () => Navigator.popUntil(context, (route) => route.isFirst),
        child: const Text(
          'OniEd',
          style: TextStyle(
            fontFamily: 'Comic Sans MS',
            fontSize: 32,
            color: Colors.white,
          ),
        ),
      ),
      actions: [
        IconButton(
          onPressed:
              () => Navigator.push(
                context,
                MaterialPageRoute(builder: (context) => SearchResultsPage()),
              ),
          icon: const Icon(Icons.search, color: Colors.white, size: 30),
        ),
        IconButton(
          onPressed: () {},
          icon: const Icon(
            Icons.account_circle_outlined,
            color: Colors.white,
            size: 45,
          ),
        ),
        Padding(padding: EdgeInsets.only(right: 8)),
      ],
    );
  }
}
