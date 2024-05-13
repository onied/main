import {
  SubscriptionFeatureInfo,
  SubscriptionInfo,
} from "./subscriptionsPreview";

export function mapSubscriptionInfo(subscriptions: Array<SubscriptionInfoDto>) {
  return subscriptions.map((sub: SubscriptionInfoDto): SubscriptionInfo => {
    return {
      subscriptionId: sub.id,
      title: sub.title,
      price: sub.price,
      durationPolicy: "на одного пользователя в месяц",
      isHighlighted: sub.coursesHighlightingEnabled,
      features: [
        sub.activeCoursesNumber > 0
          ? `${sub.activeCoursesNumber} активных платных курса`
          : sub.activeCoursesNumber == 0
            ? ""
            : "Неограниченные платные курсы",
        sub.adsEnabled ? "Реклама в рассылке" : "",
        sub.adsEnabled ? "Показ на главной странице" : "",
        sub.certificatesEnabled ? "Выдача сертифкатов" : "",
        sub.coursesHighlightingEnabled ? "Визуальное выделение курсов" : "",
      ].filter((value) => value.length > 0),
    };
  });
}

export function mapSubscriptionFeaturesInfo(
  subscriptions: Array<SubscriptionInfoDto>
) {
  let featuresInfo: Array<SubscriptionFeatureInfo> = [];
  featuresInfo.push({
    featureDescription: "Количество активных платных курсов",
    free: subscriptions[0].activeCoursesNumber.toString(),
    default: subscriptions[1].activeCoursesNumber.toString(),
    full: subscriptions[2].activeCoursesNumber.toString(),
  });

  featuresInfo.push({
    featureDescription: "Автоматическая проверка тестовых заданий",
    free: subscriptions[0].autoTestsReview,
    default: subscriptions[1].autoTestsReview,
    full: subscriptions[2].autoTestsReview,
  });

  featuresInfo.push({
    featureDescription: "Число учащихся на каждом курсе",
    free:
      subscriptions[0].studentsOnCourseLimit > -1
        ? `до ${subscriptions[0].studentsOnCourseLimit}`
        : "неограничено",
    default:
      subscriptions[1].studentsOnCourseLimit > -1
        ? `до ${subscriptions[1].studentsOnCourseLimit}`
        : "неограничено",
    full:
      subscriptions[2].studentsOnCourseLimit > -1
        ? `до ${subscriptions[2].studentsOnCourseLimit}`
        : "неограничено",
  });

  featuresInfo.push({
    featureDescription: "Реклама в рассылке пользователей",
    free: subscriptions[0].adsEnabled,
    default: subscriptions[1].adsEnabled,
    full: subscriptions[2].adsEnabled,
  });

  featuresInfo.push({
    featureDescription: "Выдача сертификатов пользователю по окончанию курса",
    free: subscriptions[0].certificatesEnabled,
    default: subscriptions[1].certificatesEnabled,
    full: subscriptions[2].certificatesEnabled,
  });

  featuresInfo.push({
    featureDescription: "Визуальное выделение курсов",
    free: subscriptions[0].coursesHighlightingEnabled,
    default: subscriptions[1].coursesHighlightingEnabled,
    full: subscriptions[2].coursesHighlightingEnabled,
  });

  featuresInfo.push({
    featureDescription: "Показ на главной странице ",
    free: subscriptions[0].adsEnabled,
    default: subscriptions[1].adsEnabled,
    full: subscriptions[2].adsEnabled,
  });

  return featuresInfo;
}

export type SubscriptionInfoDto = {
  id: number;
  price: number;
  title: string;
  activeCoursesNumber: number;
  adsEnabled: boolean;
  certificatesEnabled: boolean;
  coursesHighlightingEnabled: boolean;
  autoTestsReview: boolean;
  studentsOnCourseLimit: number;
};
