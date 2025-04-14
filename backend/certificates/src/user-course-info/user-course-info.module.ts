import { Module } from "@nestjs/common";
import { UserCourseInfoService } from "./user-course-info.service";
import { TypeOrmModule } from "@nestjs/typeorm";
import { UserCourseInfo } from "./user-course-info.entity";
import { UserModule } from "../user/user.module";
import { CourseModule } from "../course/course.module";
import { ConfigModule, ConfigService } from "@nestjs/config";
import { PurchasesServiceClient } from "../grpc-generated/purchases.client";
import { GrpcTransport } from "@protobuf-ts/grpc-transport";
import { ChannelCredentials } from "@grpc/grpc-js";
import { RabbitModule } from "../common/brokers/rabbit.module";

@Module({
  imports: [
    TypeOrmModule.forFeature([UserCourseInfo]),
    UserModule,
    CourseModule,
    RabbitModule,
    ConfigModule,
  ],
  providers: [
    UserCourseInfoService,
    {
      provide: PurchasesServiceClient,
      useFactory: (configService: ConfigService) => {
        const transport = new GrpcTransport({
          host: configService.get<string>("PURCHASES_API_URL"),
          channelCredentials: ChannelCredentials.createSsl(null, null, null, {
            checkServerIdentity: null,
          }),
        });

        return new PurchasesServiceClient(transport);
      },
      inject: [ConfigService],
    },
  ],
  exports: [UserCourseInfoService],
})
export class UserCourseInfoModule {}
