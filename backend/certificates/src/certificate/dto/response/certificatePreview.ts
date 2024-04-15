export type CertificateCourseAuthor = {
  firstName: string;
  lastName: string;
};

export type CertificateCourse = {
  title: string;
  author: CertificateCourseAuthor;
};

export type CertificatePreview = {
  price: number;
  course: CertificateCourse;
};
