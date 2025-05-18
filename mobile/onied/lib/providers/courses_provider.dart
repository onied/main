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

    return await service.performQuery(query, variables: {"id": id});
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

    return await service.performQuery(query, variables: {"id": id});
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

    return await service.performQuery(query, variables: {"id": id});
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

    return await service.performQuery(query, variables: {"id": id});
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

    return await service.performQuery(query, variables: {"id": id});
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
