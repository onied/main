import {
  Controller,
  Get,
  NotFoundException,
  Param,
  Query,
} from "@nestjs/common";
import { CertificatePreview } from "../common/types/certificatePreview";
import { CertificateService } from "./certificate.service";

@Controller("api/v1/certificate/:courseId")
export class CertificateController {
  constructor(private certificateService: CertificateService) {}
  @Get()
  async getCertificatePreview(
    @Query() userId: string,
    @Param() courseId: string
  ): Promise<CertificatePreview> {
    const nCourseId = Number(courseId);
    if (isNaN(nCourseId)) throw new NotFoundException();
    const result = await this.certificateService.getCertificatePreview(
      userId,
      nCourseId
    );
    const cr = result as CertificatePreview;
    if (cr === undefined) throw result;
    return cr;
  }
}
