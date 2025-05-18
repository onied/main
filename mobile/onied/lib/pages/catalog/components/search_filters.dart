import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:onied_mobile/components/button/button.dart';
import 'package:onied_mobile/models/course_card_model.dart';
import 'package:onied_mobile/models/course_preview_model.dart';
import 'package:onied_mobile/models/search_filters_model.dart';

class FilterBottomSheet extends StatefulWidget {
  final Iterable<CategoryModel> categories;
  final SearchFiltersModel currentSearchFilters;
  const FilterBottomSheet({
    super.key,
    required this.categories,
    required this.currentSearchFilters,
  });

  @override
  State<StatefulWidget> createState() => _FilterBottomSheetState();
}

class _FilterBottomSheetState extends State<FilterBottomSheet> {
  late SearchFiltersModel currentSearchFilters;

  @override
  void initState() {
    super.initState();
    currentSearchFilters = widget.currentSearchFilters;
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
            DropdownButtonFormField<CategoryModel>(
              decoration: const InputDecoration(
                labelText: 'Категория',
                border: OutlineInputBorder(),
              ),
              value: currentSearchFilters.selectedCategory,
              items:
                  widget.categories.map((category) {
                    return DropdownMenuItem<CategoryModel>(
                      value: category,
                      child: Text(category.name),
                    );
                  }).toList(),
              onChanged: (CategoryModel? newValue) {
                if (newValue != null) {
                  setState(() {
                    currentSearchFilters.selectedCategory = newValue;
                  });
                }
              },
            ),

            const SizedBox(height: 16),

            const Text('Ценовой диапазон'),
            RangeSlider(
              values: currentSearchFilters.selectedPriceRange,
              min: 0,
              max: 50_000,
              divisions: 20,
              labels: RangeLabels(
                '${currentSearchFilters.selectedPriceRange.start.toInt()} ₽',
                '${currentSearchFilters.selectedPriceRange.end.toInt()} ₽',
              ),
              onChanged: (RangeValues values) {
                setState(() {
                  currentSearchFilters.selectedPriceRange = values;
                });
              },
            ),

            const SizedBox(height: 16),

            CheckboxListTile(
              title: const Text('С сертификатом'),
              value: currentSearchFilters.selectedHasCertificates,
              onChanged: (value) {
                setState(() {
                  currentSearchFilters.selectedHasCertificates = value!;
                });
              },
            ),
            CheckboxListTile(
              title: const Text('В архиве'),
              value: currentSearchFilters.selectedIsArchived,
              onChanged: (value) {
                setState(() {
                  currentSearchFilters.selectedIsArchived = value!;
                });
              },
            ),

            const SizedBox(height: 16),

            Button(
              onPressed: () {
                context.pop(currentSearchFilters);
              },
              text: 'Применить',
            ),
          ],
        ),
      ),
    );
  }
}
