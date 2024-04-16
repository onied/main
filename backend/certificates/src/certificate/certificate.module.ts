import { Module } from "@nestjs/common";
import { CertificateController } from "./certificate.controller";
import { CertificateService } from "./certificate.service";
import { UserModule } from "../user/user.module";
import { CourseModule } from "../course/course.module";
import { OrderModule } from "src/order/order.module";

@Module({
  imports: [UserModule, CourseModule, OrderModule],
  controllers: [CertificateController],
  providers: [CertificateService],
})
export class CertificateModule {}
