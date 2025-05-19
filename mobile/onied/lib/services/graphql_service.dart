import 'package:onied_mobile/app/config.dart';
import 'package:graphql/client.dart';
import 'package:onied_mobile/providers/authorization_provider.dart';

class GraphQlService {
  final AuthorizationProvider authorizationProvider;
  late final GraphQLClient client;

  GraphQlService({required this.authorizationProvider}) {
    final authLink = AuthLink(
      getToken: () async {
        final credentials = await authorizationProvider.getCredentials();
        return 'Bearer ${credentials!.accessToken}';
      },
    );

    final httpLink = HttpLink(Config.graphQlEndpoint);
    final link = authLink.concat(httpLink);

    client = GraphQLClient(link: link, cache: GraphQLCache());
  }

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
