import { Module } from "@nestjs/common";
import { CourseService } from "./course.service";
import { TypeOrmModule } from "@nestjs/typeorm";
import { Course } from "./course.entity";

@Module({
  imports: [TypeOrmModule.forFeature([Course])],
  providers: [CourseService],
  exports: [CourseService],
})
export class CourseModule {}
