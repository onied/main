export class PurchaseType {
  static Any = 0;
  static Course = 1;
  static Certificate = 2;
  static Subscription = 3;
}

export type PurchaseCreated = {
  userId: string;
  purchaseType: number;
  courseId: number | null;
  token: string;
};
