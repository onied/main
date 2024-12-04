import { IQueryHandler, QueryHandler } from "@nestjs/cqrs";
import { CertificateService } from "../certificate.service";
import { GetCertificatesListQuery } from "../queries/get-certificates-list.query";

@QueryHandler(GetCertificatesListQuery)
export class GetCertificatesListQueryHandler
  implements IQueryHandler<GetCertificatesListQuery>
{
  constructor(private service: CertificateService) {}

  async execute(command: GetCertificatesListQuery) {
    return await this.service.getAvailableCertificates(command.userId);
  }
}
