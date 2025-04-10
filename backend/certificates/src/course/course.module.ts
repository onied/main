import { Module } from "@nestjs/common";
import { CourseService } from "./course.service";
import { TypeOrmModule } from "@nestjs/typeorm";
import { Course } from "./course.entity";
import { User } from "../user/user.entity";
import { RabbitModule } from "../common/brokers/rabbit.module";

@Module({
  imports: [TypeOrmModule.forFeature([Course, User]), RabbitModule],
  providers: [CourseService],
  exports: [CourseService],
})
export class CourseModule {}
