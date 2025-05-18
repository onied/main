import 'package:flutter/material.dart';
import 'package:onied_mobile/models/course_preview_model.dart';

class SearchFiltersModel {
  CategoryModel selectedCategory;
  RangeValues selectedPriceRange;
  RangeValues selectedCompletionTimeRange;
  bool selectedHasCertificates;
  bool selectedIsArchived;

  SearchFiltersModel({
    required this.selectedCategory,
    required this.selectedPriceRange,
    required this.selectedCompletionTimeRange,
    required this.selectedHasCertificates,
    required this.selectedIsArchived,
  });
}
