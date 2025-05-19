import 'package:flutter/material.dart';
import 'package:onied_mobile/models/course_preview_model.dart';

class SearchFiltersModel {
  CategoryModel selectedCategory;
  RangeValues selectedPriceRange;
  RangeValues selectedCompletionTimeRange;
  bool selectedMustHaveCertificates;
  bool selectedIsActiveOnly;

  SearchFiltersModel({
    required this.selectedCategory,
    required this.selectedPriceRange,
    required this.selectedCompletionTimeRange,
    required this.selectedMustHaveCertificates,
    required this.selectedIsActiveOnly,
  });
}
