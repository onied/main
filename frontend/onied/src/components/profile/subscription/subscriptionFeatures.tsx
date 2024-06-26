import classes from "./subscription.module.css";

function SubscriptionFeatures(props: {
  coursesHighlightingEnabled: boolean;
  showingMainPageEnabled: boolean;
  adsEnabled: boolean;
  certificatesEnabled: boolean;
  courseCreatingEnabled: boolean;
}) {
  const subscriptionFeatures = [
    props.courseCreatingEnabled ? "Создание курсов" : undefined,
    props.adsEnabled ? "Реклама в рассылке" : undefined,
    props.certificatesEnabled ? "Выдача сертификатов" : undefined,
    props.showingMainPageEnabled ? "Показ на главной странице" : undefined,
    props.coursesHighlightingEnabled
      ? "Визуальное выделение курсов"
      : undefined,
  ];

  return (
    <div className={classes.subscriptionFeaturesList}>
      <ul>
        {subscriptionFeatures.map((feature, index) =>
          feature != undefined ? (
            <li key={index}>
              <span>{feature}</span>
            </li>
          ) : (
            ""
          )
        )}
      </ul>
    </div>
  );
}

export default SubscriptionFeatures;
