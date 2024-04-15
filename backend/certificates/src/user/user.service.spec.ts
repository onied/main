import { Test, TestingModule } from "@nestjs/testing";
import { UserService } from "./user.service";
import { Repository } from "typeorm";
import { User } from "./user.entity";
import { getRepositoryToken } from "@nestjs/typeorm";

describe("UserService", () => {
  let service: UserService;
  let repo: Repository<User>;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [
        UserService,
        {
          provide: getRepositoryToken(User),
          useClass: Repository,
        },
      ],
    }).compile();

    service = module.get<UserService>(UserService);
    repo = module.get<Repository<User>>(getRepositoryToken(User));
  });

  it("should be defined", () => {
    expect(service).toBeDefined();
  });

  it("should return for findOne", async () => {
    // Arrange

    const id = "066c23b2-cabf-4a7f-a6cd-e2c8b9e92425";

    const testUser: User = {
      id: id,
      firstName: "Василий",
      lastName: "Теркин",
      avatar:
        "https://upload.wikimedia.org/wikipedia/commons/thumb/d/d8/Lynx_lynx-4.JPG/640px-Lynx_lynx-4.JPG",
    };
    jest.spyOn(repo, "findOneBy").mockResolvedValueOnce(testUser);

    // Act

    const result = await service.findOne(id);

    // Assert

    expect(result).toEqual(testUser);
  });
});
