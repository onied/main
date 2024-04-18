import { Module } from "@nestjs/common";
import { UserCourseInfoService } from "./user-course-info.service";
import { TypeOrmModule } from "@nestjs/typeorm";
import { UserCourseInfo } from "./user-course-info.entity";
import { UserModule } from "../user/user.module";
import { CourseModule } from "../course/course.module";

@Module({
  imports: [
    TypeOrmModule.forFeature([UserCourseInfo]),
    UserModule,
    CourseModule,
  ],
  providers: [UserCourseInfoService],
})
export class UserCourseInfoModule {}
