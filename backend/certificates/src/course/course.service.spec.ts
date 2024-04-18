import { Test, TestingModule } from "@nestjs/testing";
import { CourseService } from "./course.service";
import { Course } from "./course.entity";
import { Repository } from "typeorm";
import { getRepositoryToken } from "@nestjs/typeorm";
import { User } from "../user/user.entity";

describe("CourseService", () => {
  let service: CourseService;
  let repo: Repository<Course>;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        CourseService,
        {
          provide: getRepositoryToken(Course),
          useClass: Repository,
        },
        {
          provide: getRepositoryToken(User),
          useClass: Repository,
        },
      ],
    }).compile();

    service = module.get<CourseService>(CourseService);
    repo = module.get<Repository<Course>>(getRepositoryToken(Course));
  });

  it("should be defined", () => {
    expect(service).toBeDefined();
  });

  it("should return for findOne", async () => {
    // Arrange

    const author: User = {
      id: "066c23b2-cabf-4a7f-a6cd-e2c8b9e92425",
      firstName: "Василий",
      lastName: "Теркин",
      avatar:
        "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Lynx_lynx-4.JPG/640px-Lynx_lynx-4.JPG",
      gender: 0,
    };

    const course: Course = {
      id: 1,
      title: "asdfasdf",
      author: author,
      hasCertificates: true,
    };
    jest.spyOn(repo, "findOne").mockResolvedValueOnce(course);

    // Act

    const result = await service.findOne(course.id);

    // Assert

    expect(result).toEqual(course);
  });

  it("should return for findInList", async () => {
    // Arrange

    const author: User = {
      id: "066c23b2-cabf-4a7f-a6cd-e2c8b9e92425",
      firstName: "Василий",
      lastName: "Теркин",
      avatar:
        "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Lynx_lynx-4.JPG/640px-Lynx_lynx-4.JPG",
      gender: 0,
    };

    const coursesIds = [1, 2, 3];

    const courses = [
      {
        id: 1,
        title: "asdfasdf",
        author: author,
        hasCertificates: true,
      },
      {
        id: 2,
        title: "asdfasdf",
        author: author,
        hasCertificates: true,
      },
      {
        id: 3,
        title: "asdfasdf",
        author: author,
        hasCertificates: true,
      },
    ];
    jest.spyOn(repo, "find").mockResolvedValueOnce(courses);

    // Act

    const result = await service.findInList(coursesIds);

    // Assert

    expect(result).toEqual(courses);
  });
});
