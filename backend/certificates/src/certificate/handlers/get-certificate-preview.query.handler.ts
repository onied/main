import { IQueryHandler, QueryHandler } from "@nestjs/cqrs";
import { CertificateService } from "../certificate.service";
import { GetCertificatePreviewQuery } from "../queries/get-certificate-preview.query";

@QueryHandler(GetCertificatePreviewQuery)
export class GetCertificatePreviewQueryHandler
  implements IQueryHandler<GetCertificatePreviewQuery>
{
  constructor(private service: CertificateService) {}

  async execute(command: GetCertificatePreviewQuery) {
    return await this.service.getCertificatePreview(
      command.userId,
      command.courseId
    );
  }
}
