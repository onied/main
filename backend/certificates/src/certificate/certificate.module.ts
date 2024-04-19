import { Module } from "@nestjs/common";
import { CertificateController } from "./certificate.controller";
import { CertificateService } from "./certificate.service";
import { UserModule } from "../user/user.module";
import { CourseModule } from "../course/course.module";
import { OrderModule } from "../order/order.module";
import { HttpModule } from "@nestjs/axios";
import { ConfigModule } from "@nestjs/config";
import { UserCourseInfoModule } from "../user-course-info/user-course-info.module";

@Module({
  imports: [
    UserModule,
    CourseModule,
    OrderModule,
    HttpModule,
    ConfigModule,
    UserCourseInfoModule,
  ],
  controllers: [CertificateController],
  providers: [CertificateService],
})
export class CertificateModule {}
