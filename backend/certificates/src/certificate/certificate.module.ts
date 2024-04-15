import { Module } from "@nestjs/common";
import { CertificateController } from "./certificate.controller";
import { CertificateService } from "./certificate.service";
import { UserModule } from "src/user/user.module";
import { CourseModule } from "src/course/course.module";

@Module({
  imports: [UserModule, CourseModule],
  controllers: [CertificateController],
  providers: [CertificateService],
})
export class CertificateModule {}
