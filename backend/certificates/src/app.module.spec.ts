import { Test, TestingModule } from "@nestjs/testing";
import { AppModule } from "./app.module";

describe("CertificateModule", () => {
  let mod: AppModule;

  beforeEach(async () => {
    const module: TestingModule = await Test.createTestingModule({
      providers: [AppModule],
    }).compile();

    mod = module.get<AppModule>(AppModule);
  });

  it("should be defined", () => {
    expect(mod).toBeDefined();
  });
});
