import { Module } from "@nestjs/common";
import { CertificateController } from "./certificate.controller";
import { CertificateService } from "./certificate.service";
import { UserModule } from "../user/user.module";
import { CourseModule } from "../course/course.module";
import { OrderModule } from "../order/order.module";
import { HttpModule } from "@nestjs/axios";
import { ConfigModule } from "@nestjs/config";
import { UserCourseInfoModule } from "../user-course-info/user-course-info.module";
import { CreateOrderCommandHandler } from "./handlers/create-order.command.handler";
import { GetCertificatePreviewQueryHandler } from "./handlers/get-certificate-preview.query.handler";
import { GetCertificatesListQueryHandler } from "./handlers/get-certificates-list.query.handler";
import { CqrsModule } from "@nestjs/cqrs";

export const CommandHandlers = [CreateOrderCommandHandler];
export const QueryHandlers = [
  GetCertificatePreviewQueryHandler,
  GetCertificatesListQueryHandler,
];

@Module({
  imports: [
    CqrsModule,
    UserModule,
    CourseModule,
    OrderModule,
    HttpModule,
    ConfigModule,
    UserCourseInfoModule,
  ],
  controllers: [CertificateController],
  providers: [CertificateService, ...CommandHandlers, ...QueryHandlers],
})
export class CertificateModule {}
