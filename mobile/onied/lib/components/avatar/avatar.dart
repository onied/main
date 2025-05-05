import 'package:flutter/material.dart';

class Avatar extends StatelessWidget {
  final bool circular;
  final String? name;
  final String? url;
  final double width;
  const Avatar({
    super.key,
    required this.width,
    this.name,
    this.url,
    this.circular = false,
  });

  @override
  Widget build(BuildContext context) {
    final initials =
        name?.split(' ').take(2).map((str) {
          return str[0];
        }).join();

    final child = Center(
      child: Text(
        initials ?? "",
        style: TextStyle(fontSize: 32, color: Colors.white),
      ),
    );

    return switch (circular) {
      true => CircleAvatar(
        radius: width / 2,
        backgroundColor: Colors.grey,
        backgroundImage: switch (url) {
          null => null,
          _ => Image.network(url!, width: width, fit: BoxFit.cover).image,
        },
        child: switch (url) {
          null => child,
          _ => null,
        },
      ),
      false => Container(
        width: width,
        height: width,
        clipBehavior: Clip.antiAlias,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(12),
          color: Colors.grey,
        ),
        child: switch (url) {
          null => child,
          _ => Image.network(url!, width: width, fit: BoxFit.cover),
        },
      ),
    };
  }
}
