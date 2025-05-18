import 'package:onied_mobile/app/config.dart';
import 'package:graphql/client.dart';

class GraphQlService {
  final HttpLink httpLink = HttpLink(
    Config.graphQlEndpoint,
    defaultHeaders: {'Authorization': 'Bearer'},
  );

  late final GraphQLClient client = GraphQLClient(
    link: httpLink,
    cache: GraphQLCache(),
  );

  Future<QueryResult> performQuery(
    String query, {
    required Map<String, dynamic> variables,
  }) async {
    QueryOptions options = QueryOptions(
      document: gql(query),
      variables: variables,
    );

    final result = await client.query(options);

    return result;
  }
}
