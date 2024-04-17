import { Module } from "@nestjs/common";
import { CourseService } from "./course.service";
import { TypeOrmModule } from "@nestjs/typeorm";
import { Course } from "./course.entity";
import { User } from "../user/user.entity";

@Module({
  imports: [TypeOrmModule.forFeature([Course, User])],
  providers: [CourseService],
  exports: [CourseService],
})
export class CourseModule {}
