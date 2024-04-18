import { Module } from "@nestjs/common";
import { UserCourseInfoService } from "./user-course-info.service";
import { TypeOrmModule } from "@nestjs/typeorm";
import { UserCourseInfo } from "./user-course-info.entity";
import { UserModule } from "../user/user.module";
import { CourseModule } from "../course/course.module";
import { HttpModule } from "@nestjs/axios";
import { ConfigModule } from "@nestjs/config";

@Module({
  imports: [
    TypeOrmModule.forFeature([UserCourseInfo]),
    UserModule,
    HttpModule,
    ConfigModule,
    CourseModule,
  ],
  providers: [UserCourseInfoService],
  exports: [UserCourseInfoService],
})
export class UserCourseInfoModule {}
