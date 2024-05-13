using Courses.Attributes;

namespace Courses.Enums;

public enum ServerApiConfig
{
    [StringValue("PurchasesServer")]
    PurchasesServer,
    [StringValue("SubscriptionsServer")]
    SubscriptionsServer
}
