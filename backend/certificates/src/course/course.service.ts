import { Injectable } from "@nestjs/common";
import { InjectRepository } from "@nestjs/typeorm";
import { Repository } from "typeorm";
import { Course } from "./course.entity";

@Injectable()
export class CourseService {
  constructor(
    @InjectRepository(Course)
    private courseRepository: Repository<Course>
  ) {}

  findOne(id: number): Promise<Course | null> {
    return this.courseRepository.findOneBy({ id });
  }
}
