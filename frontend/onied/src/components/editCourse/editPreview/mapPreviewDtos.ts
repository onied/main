export function mapPreviewToEditPreview(
  previewDto: PreviewDto
): EditPreviewDto {
  return {
    title: previewDto.title,
    description: previewDto.description,
    categoryId: previewDto.category.id,
    price: previewDto.price,
    hoursCount: previewDto.hoursCount,
    pictureHref: previewDto.pictureHref,
    isProgramVisible: previewDto.isProgramVisible,
    hasCertificates: previewDto.hasCertificates,
    isArchived: previewDto.isArchived,
  };
}

export type PreviewDto = {
  title: string;
  pictureHref: string;
  description: string;
  hoursCount: number;
  price: number;
  category: {
    id: number;
    name: string;
  };
  courseAuthor: {
    name: string;
    avatarHref: string;
  };
  isArchived: boolean;
  hasCertificates: boolean;
  isProgramVisible: boolean;
  courseProgram: Array<string> | undefined;
};

type EditPreviewDto = {
  title: string;
  description: string;
  categoryId: number;
  price: number;
  hoursCount: number;
  pictureHref: string;
  isProgramVisible: boolean;
  hasCertificates: boolean;
  isArchived: boolean;
};
