import { Module } from "@nestjs/common";
import { TypeOrmModule } from "@nestjs/typeorm";
import { ConfigModule } from "@nestjs/config";
import { UserModule } from "./user/user.module";
import { OrderModule } from "./order/order.module";
import { CourseModule } from "./course/course.module";
import { CertificateModule } from "./certificate/certificate.module";
import { UserCourseInfoModule } from "./user-course-info/user-course-info.module";
import { RabbitModule } from "./common/brokers/rabbit.module";

@Module({
  imports: [
    ConfigModule.forRoot(),
    TypeOrmModule.forRoot({
      type: "postgres",
      host: process.env.DATABASE_HOST,
      port: Number(process.env.DATABASE_PORT),
      username: process.env.DATABASE_USER,
      password: process.env.DATABASE_PASS,
      database: process.env.DATABASE_NAME,
      autoLoadEntities: true,
      synchronize: true,
    }),
    UserModule,
    OrderModule,
    CourseModule,
    CertificateModule,
    RabbitModule,
    UserCourseInfoModule,
  ],
})
export class AppModule {}
