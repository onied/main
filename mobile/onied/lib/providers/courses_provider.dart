import 'dart:convert';
import 'dart:io';

import 'package:graphql/client.dart';
import 'package:http/http.dart' as http;
import 'package:onied_mobile/app/config.dart';
import 'package:onied_mobile/models/search_filters_model.dart';
import 'package:onied_mobile/providers/authorization_provider.dart';
import 'package:onied_mobile/services/graphql_service.dart';

class CourseProvider {
  final AuthorizationProvider authorizationProvider;
  final GraphQlService graphqlService;
  CourseProvider(this.graphqlService, this.authorizationProvider);

  Future<QueryResult> getAllCategories() async {
    String query = '''
    query Categories {
        categories {
            id
            name
        }
    }
    ''';

    return await graphqlService.performQuery(query, variables: const {});
  }

  Future<QueryResult> getSearchResult(
    String searchQuery,
    SearchFiltersModel searchFilters,
  ) async {
    String query = '''
    query Courses(
        \$query: String
        \$minPrice: Int
        \$maxPrice: Int
        ${searchFilters.selectedCategory.id != -1 ? "\$id: Int" : ''}
        ${searchFilters.selectedMustHaveCertificates ? "\$hasCertificates: Boolean" : ''}
        ${!searchFilters.selectedIsActiveOnly ? '' : "\$isArchived: Boolean"}
    ) {
        courses(
            where: {
                title: { contains: \$query }
                priceRubles: { gte: \$minPrice, lte: \$maxPrice }
                ${searchFilters.selectedCategory.id != -1 ? "categoryId: { eq: \$id }" : ''}
                ${searchFilters.selectedMustHaveCertificates ? "hasCertificates: { eq: \$hasCertificates }" : ''}
                ${!searchFilters.selectedIsActiveOnly ? '' : "isArchived: { eq: \$isArchived }"}
            }
        ) {
            nodes {
                id
                title
                pictureHref
                description
                priceRubles
                isArchived
                hasCertificates
                category {
                    id
                    name
                }
            }
        }
    }
    ''';

    var variables = {
      "query": searchQuery,
      "id": searchFilters.selectedCategory.id,
      "minPrice": searchFilters.selectedPriceRange.start.toInt(),
      "maxPrice": searchFilters.selectedPriceRange.end.toInt(),
      "hasCertificates": searchFilters.selectedMustHaveCertificates,
      "isArchived": !searchFilters.selectedIsActiveOnly,
    };

    return await graphqlService.performQuery(query, variables: variables);
  }

  Future<QueryResult> getCoursePreviewById(int id) async {
    String query = r'''
    query PublicCourseById($id: Int!) {
        publicCourseById(id: $id) {
            id
            title
            pictureHref
            description
            hoursCount
            priceRubles
            category {
                id
                name
            }
            author {
                firstName
                lastName
                avatarHref
            }
            isArchived
            hasCertificates
            isOwned
            modules {
                title
            }
        }
    }
    ''';

    return await graphqlService.performQuery(query, variables: {"id": id});
  }

  Future<QueryResult> getCourseHierarchyById(int id) async {
    String query = r'''
    query CourseById($id: Int!) {
        courseById(id: $id) {
            id
            title
            modules {
                id
                index
                title
                blocks {
                    id
                    index
                    title
                    blockType
                }
            }
        }
    }
    ''';

    return await graphqlService.performQuery(query, variables: {"id": id});
  }

  Future<QueryResult> getSummaryBlockById(int id) async {
    String query = r'''
        query SummaryBlockById($id: Int!) {
        summaryBlockById(id: $id) {
            id
            title
            markdownText
            fileName
            fileHref
        }
    }
    ''';

    return await graphqlService.performQuery(query, variables: {"id": id});
  }

  Future<QueryResult> getVideoBlockById(int id) async {
    String query = r'''
    query VideoBlockById($id: Int!) {
        videoBlockById(id: $id) {
            id
            title
            href
        }
    }
    ''';

    return await graphqlService.performQuery(query, variables: {"id": id});
  }

  Future<QueryResult> getTasksBlockById(int id) async {
    String query = r'''
    query TasksBlockById($id: Int!) {
        tasksBlockById(id: $id) {
            id
            title
            tasks {
                id
                title
                points
                maxPoints
                variants {
                    id
                    description
                }
                taskType
            }
        }
    }
    ''';

    return await graphqlService.performQuery(query, variables: {"id": id});
  }

  Future<QueryResult> getOwnedCourses(int amount) async {
    String query = r'''
    query OwnedCourses ($amount: Int) {
        ownedCourses(first: $amount) {
            nodes {
                id
                title
                pictureHref
            }
        }
    }
    ''';

    return await graphqlService.performQuery(
      query,
      variables: {"amount": amount},
    );
  }

  Future<QueryResult> getPopularCourses(int amount) async {
    String query = r'''
    query PopularCourses($amount: Int) {
        popularCourses(first: $amount) {
            nodes {
                id
                title
                pictureHref
            }
        }
    }
    ''';

    return await graphqlService.performQuery(
      query,
      variables: {"amount": amount},
    );
  }

  Future<QueryResult> getRecommendedCourses(int amount) async {
    String query = r'''
    query Courses($amount: Int) {
        courses(first: $amount, where: { isGlowing: { eq: true } }) {
            nodes {
                id
                title
                pictureHref
                }
            }
        }
    ''';

    return await graphqlService.performQuery(
      query,
      variables: {"amount": amount},
    );
  }

  Future<bool> likeCourse(int courseId, bool like) async {
    final auth = await authorizationProvider.getCredentials();
    final response = await http.post(
      Uri.parse(
        "${Config.backendUrl}/courses/$courseId/${like ? "like" : "unlike"}",
      ),
      headers: {
        'Content-Type': 'application/json',
        "Authorization": "Bearer ${auth?.accessToken}",
      },
    );

    if (response.statusCode != HttpStatus.ok) {
      return false;
    }
    return true;
  }
}
