export class GetCertificatePreviewQuery {
  constructor(
    public readonly userId: string,
    public readonly courseId: number
  ) {}
}
