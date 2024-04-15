export enum PurchaseType {
  Course,
  Certificate,
  Subscription,
}

export enum SubscriptionType {
  Basic,
  Full,
}

export type Purchase = {
  purchaseType: PurchaseType;
  price: number;
  cardInfo: CardInfo;
};

export type CoursePurchase =
  | Purchase
  | {
      courseId: number;
    };

export type CertificatePurchase =
  | Purchase
  | {
      courseId: number;
    };

export type SubscriptionPurchase =
  | Purchase
  | {
      subscriptionType: SubscriptionType;
    };

export type CardInfo = {
  number: string;
  holder: string;
  month: number;
  year: number;
  securityCode: string;
};

export type PurchaseInfoData = {
  id: number;
  title: string;
  price: number;
};
