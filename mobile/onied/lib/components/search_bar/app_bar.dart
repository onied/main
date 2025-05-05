import 'package:flutter/material.dart';
import 'package:onied_mobile/app/app_theme.dart';
import 'package:onied_mobile/views/catalog/search_results_page.dart';

class CourseSearchBar extends StatelessWidget implements PreferredSizeWidget {
  const CourseSearchBar({super.key});

  @override
  Size get preferredSize => const Size.fromHeight(kToolbarHeight);

  @override
  Widget build(BuildContext context) {
    return AppBar(
      automaticallyImplyLeading: false,
      backgroundColor: AppTheme.backgroundColorHeader,
      elevation: 0,
      titleSpacing: 16,
      title: InkWell(
        onTap: () => Navigator.popUntil(context, (route) => route.isFirst),
        child: const Text(
          'OniEd',
          style: TextStyle(
            fontFamily: 'Inter',
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
                MaterialPageRoute(builder: (context) => SearchResultsPage()), // TODO: to route
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
