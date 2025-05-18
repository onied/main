import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/components/button/button.dart';
import 'package:onied_mobile/models/course_card_model.dart';

class FilterBottomSheet extends StatefulWidget {
  final Iterable<String> categories;
  const FilterBottomSheet({super.key, required this.categories});

  @override
  State<StatefulWidget> createState() => _FilterBottomSheetState();
}

class _FilterBottomSheetState extends State<FilterBottomSheet> {
  static const double maxPrice = 100_000;

  List<String> categories = ['All'];
  String selectedCategory = 'All';

  RangeValues priceRange = const RangeValues(0, maxPrice);

  bool isCertificateIncluded = false;
  bool isOnlyActive = false;

  @override
  void initState() {
    super.initState();
    categories.addAll(widget.categories);
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(16),
      height: 500,
      width: double.infinity,
      child: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            const Text(
              'Фильтры',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 16),

            DropdownButtonFormField<String>(
              value: selectedCategory,
              decoration: const InputDecoration(labelText: 'Категория'),
              items:
                  categories
                      .map(
                        (category) => DropdownMenuItem(
                          value: category,
                          child: Text(category),
                        ),
                      )
                      .toList(),
              onChanged: (value) {
                setState(() {
                  selectedCategory = value!;
                });
              },
            ),

            const SizedBox(height: 16),

            const Text('Ценовой диапазон'),
            RangeSlider(
              values: priceRange,
              min: 0,
              max: maxPrice,
              divisions: 20,
              labels: RangeLabels(
                '${priceRange.start.toInt()} ₽',
                '${priceRange.end.toInt()} ₽',
              ),
              onChanged: (RangeValues values) {
                setState(() {
                  priceRange = values;
                });
              },
            ),

            const SizedBox(height: 16),

            CheckboxListTile(
              title: const Text('Только с сертификатом'),
              value: isCertificateIncluded,
              onChanged: (value) {
                setState(() {
                  isCertificateIncluded = value!;
                });
              },
            ),
            CheckboxListTile(
              title: const Text('Только активные'),
              value: isOnlyActive,
              onChanged: (value) {
                setState(() {
                  isOnlyActive = value!;
                });
              },
            ),

            const SizedBox(height: 16),

            Button(
              onPressed: () {
                filteringPredicate(CourseCardModel courseCardDto) {
                  final matchesCategory =
                      selectedCategory == 'All' ||
                      courseCardDto.category == selectedCategory;
                  final matchesPrice =
                      courseCardDto.price >= priceRange.start &&
                      courseCardDto.price <= priceRange.end;
                  final matchesCertificate =
                      !isCertificateIncluded || courseCardDto.hasCertificates;
                  final matchesIsActive =
                      !isOnlyActive || !courseCardDto.isArchived;

                  return matchesCategory &&
                      matchesPrice &&
                      matchesCertificate &&
                      matchesIsActive;
                }

                context.pop(filteringPredicate);
              },
              text: 'Применить',
            ),
          ],
        ),
      ),
    );
  }
}
