import 'package:graphql/client.dart';
import 'package:onied_mobile/services/graphql_service.dart';

class CourseProvider {
  final GraphQlService service;
  CourseProvider(this.service);

  Future<QueryResult> getAllCategories() async {
    String query = '''
    query Categories {
        categories {
            name
        }
    }
    ''';

    return await service.performQuery(query, variables: const {});
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

    return await service.performQuery(query, variables: {"amount": amount});
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

    return await service.performQuery(query, variables: {"amount": amount});
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

    return await service.performQuery(query, variables: {"amount": amount});
  }
}
