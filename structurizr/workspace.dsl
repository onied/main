workspace "Onied" {

    !identifiers hierarchical

    model {
        u = person "User" "Typically a self-learner who wants to receive an online education on some of the topics"
        ss = softwareSystem "Onied" "Learning Management System" {
            frontend = container "Single-Page Application" "Implements UI for all the user-facing features in the LMS" "JS/TS, React" {
                tags "Frontend"

                component "ConfirmEmail"
                component "ForgotPassword"
                component "Login"
                component "Register"
                component "ResetPassword"
                component "TwoFactorAuth"

                component "Catalog"
                component "Certificates"
                component "CheckTasks"
                component "Course"
                component "CreateCourse"
                component "EditCourse"
                component "Landing"
                component "ManageModerators"
                component "OauthRedirect"
                component "Preview"
                component "Profile"
                component "Purchase"
                component "Subscriptions"
                component "Teaching"
            }
            group "Users Service" {
                users = container "Users" "Users authentication and authorization service /\n API Gateway" "C#, ASP.NET Core" {
                    tags "Service" "Users"

                    group "Controllers" {
                        component "UsersController"
                        component "ProfileController"
                    }

                    group "Producers" {
                        component "ProfileProducer"
                        component "UserCreatedProducer"
                    }

                    group "Services" {
                        component "EmailSender"
                        component "ProfileService"
                        component "UsersService"
                    }
                }
                users_db = container "Users Database" "Stores data related to users" "PostgreSQL"  {
                    tags "Database" "Users"
                }
            }
            group "Courses Service" {
                courses = container "Courses" "Implements business logic for courses and their content (including learning materials)" "C#, ASP.NET Core" {
                    tags "Service" "Courses"

                    group "Controllers" {
                        component "AccountsController"
                        component "CatalogController"
                        component "CategoriesController"
                        component "CheckTasksController"
                        component "CoursesController"
                        component "EditCoursesController"
                        component "LandingController"
                        component "ModeratorsCourseController"
                        component "TeachingController"
                    }

                    group "Services" {
                        component "AccountsService"
                        component "CatalogService"
                        component "CheckTaskManagementService"
                        component "CheckTasksService"
                        component "CourseManagementService"
                        component "CourseService"
                        component "LandingPageContentService"
                        component "ManualReviewService"
                        component "NotificationPreparerService"
                        component "SubscriptionManagementService"
                        component "TeachingService"
                        component "UpdateTasksBlockService"
                    }

                    group "Repositories" {
                        component "UserCourseInfoRepository"
                        component "UserRepository"
                        component "UserTaskPointsRepository"
                        component "BlockCompletedInfoRepository"
                        component "BlockRepository"
                        component "CategoryRepository"
                        component "CourseRepository"
                        component "ManualReviewTaskUserAnswerRepository"
                        component "ModuleRepository"
                    }

                    group "Consumers" {
                        component "ProfilePhotoUpdatedConsumer"
                        component "ProfileUpdatedConsumer"
                        component "PurchaseCreatedConsumer"
                        component "SubscriptionChangedConsumer"
                        component "UserCreatedConsumer"
                    }

                    group "Producers" {
                        component "CourseCompletedProducer"
                        component "CourseCreatedProducer"
                        component "CourseUpdatedProducer"
                        component "NotificationSentProducer"
                    }

                }
                courses_db = container "Courses Database" "Stores courses and their content" "PostgreSQL" {
                    tags "Database" "Courses"
                }
            }
            group "Purchases Service" {
                purchases = container "Purchases" "Implements business logic for purchases and subsciption handling" "C#, ASP.NET Core" {
                    tags "Service" "Purchases"

                    group "Controllers" {
                        component "PurchasesController"
                        component "PurchasesMakingController"
                        component "SubscriptionsController"
                    }

                    group "Services" {
                        component "JwtTokenService"
                        component "PurchaseMakingService"
                        component "PurchaseService"
                        component "PurchaseTokenService"
                        component "SubscriptionManagementService"
                        component "ValidatePurchaseService"
                    }

                    group "Consumers" {
                        component "CourseCompletedConsumer"
                        component "CourseCreatedConsumer"
                        component "CourseUpdatedConsumer"
                        component "UserCreatedConsumer"
                    }

                    group "Producers" {
                        component "PurchaseCreatedProducer"
                        component "SubscriptionChangedProducer"
                    }

                }
                purchases_db = container "Purchases Database" "Stores purchases" "PostgreSQL" {
                    tags "Database" "Purchases"
                }
            }
            group "Certificates Service" {
                certificates = container "Certificates" "Implements business logic for ordering certificates upon course completion" "JS, Node.js" {
                    tags "Service" "Certificates"

                    group "Modules" {
                        component "AppModule"
                        component "CertificateModule"
                        component "CourseModule"
                        component "OrderModule"
                        component "UserModule"
                    }

                    group "Controllers" {
                        component "CertificateController"
                        component "OrderController"
                    }

                    group "Services" {
                        component "CertificateService"
                        component "CourseService"
                        component "OrderService"
                        component "UserService"
                    }
                }
                certificates_db = container "Certificates Database" "Stores orders and data required to generate certificates" "PostgreSQL" {
                    tags "Database" "Certificates"
                }
            }
            group "Notifications Service" {
                notifications = container "Notifications" "Implements business logic for sending notifications to users" "C#, ASP.NET Core"  {
                    tags "Service" "Notifications"

                    group "Controllers" {
                        component "NotificationsController"
                    }

                    group "Hubs" {
                        component "NotificationsHub"
                    }

                    group "Services" {
                        component "UserIdProvider"
                        component "NotificationSenderService"
                    }

                    group "Consumers" {
                        component "NotificationSentConsumer"
                    }
                }
                notifications_db = container "Notifications Database" "Stores sent notifications" "PostgreSQL" {
                    tags "Database" "Notifications"
                }
            }

            group "Topics" {
                courseCompleted = container "CourseCompleted Topic" "Signals that a user has completed a course" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                courseCreated = container "CourseCreated Topic" "Signals that a course was created" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                courseUpdated = container "CourseUpdated Topic" "Signals that a course was updated" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                notificationSent = container "NotificationSent Topic" "Signals that a notification needs to be sent" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                profilePhotoUpdated = container "ProfilePhotoUpdated Topic" "Signals that a user has updated their profile photo" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                profileUpdated = container "ProfileUpdated Topic" "Signals that a user has updated their profile info" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                purchaseCreated = container "PurchaseCreated Topic" "Signals that a user has purchased something" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                subscriptionChanged = container "SubscriptionChanged Topic" "Signals that user's subscription has changed" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
                userCreated = container "UserCreated Topic" "Signals that a new user was created" "MassTransit, RabbitMQ" {
                    tags "Topic"
                }
            }
        }

        u -> ss.frontend "Uses" 
        ss.frontend -> ss.users "Makes API calls to" "JSON/HTTP" "Http"
        ss.users -> ss.courses "Routes API requests to" "JSON/HTTP" "Http"
        ss.users -> ss.purchases "Routes API requests to" "JSON/HTTP" "Http"
        ss.users -> ss.certificates "Routes API requests to" "JSON/HTTP" "Http"
        ss.users -> ss.notifications "Routes API requests to" "JSON/HTTP" "Http"
        ss.users -> ss.users_db "Reads from and writes to"
        ss.courses -> ss.courses_db "Reads from and writes to"
        ss.purchases -> ss.purchases_db "Reads from and writes to"
        ss.certificates -> ss.certificates_db "Reads from and writes to"
        ss.notifications -> ss.notifications_db "Reads from and writes to"

        ss.courses -> ss.courseCompleted "Publishes messages to" "" "Topic in"
        ss.courseCompleted -> ss.purchases "Sends messages to" "" "Topic out"

        ss.courses -> ss.courseCreated "Publishes messages to" "" "Topic in"
        ss.courseCreated -> ss.purchases "Sends messages to" "" "Topic out"
        ss.courseCreated -> ss.certificates "Sends messages to" "" "Topic out"

        ss.courses -> ss.courseUpdated "Publishes messages to" "" "Topic in"
        ss.courseUpdated -> ss.purchases "Sends messages to" "" "Topic out"
        ss.courseUpdated -> ss.certificates "Sends messages to" "" "Topic out"

        ss.courses -> ss.notificationSent "Publishes messages to" "" "Topic in"
        ss.notificationSent -> ss.notifications "Sends messages to" "" "Topic out"

        ss.users -> ss.profilePhotoUpdated "Publishes messages to" "" "Topic in"
        ss.profilePhotoUpdated -> ss.courses "Sends messages to" "" "Topic out"
        ss.profilePhotoUpdated -> ss.certificates "Sends messages to" "" "Topic out"

        ss.users -> ss.profileUpdated "Publishes messages to" "" "Topic in"
        ss.profileUpdated -> ss.courses "Sends messages to" "" "Topic out"
        ss.profileUpdated -> ss.certificates "Sends messages to" "" "Topic out"

        ss.purchases -> ss.purchaseCreated "Publishes messages to" "" "Topic in"
        ss.purchaseCreated -> ss.courses "Sends messages to" "" "Topic out"
        ss.purchaseCreated -> ss.certificates "Sends messages to" "" "Topic out"

        ss.purchases -> ss.subscriptionChanged "Publishes messages to" "" "Topic in"
        ss.subscriptionChanged -> ss.courses "Sends messages to" "" "Topic out"

        ss.users -> ss.userCreated "Publishes messages to" "" "Topic in"
        ss.userCreated -> ss.courses "Sends messages to" "" "Topic out"
        ss.userCreated -> ss.purchases "Sends messages to" "" "Topic out"
        ss.userCreated -> ss.certificates "Sends messages to" "" "Topic out"
    }

    views {
        systemContext ss "Diagram1" {
            include *
            autolayout lr
        }

        container ss "Diagram2" {
            include *
        }

        component ss.frontend "Diagram3_frontend" {
            include *
            autolayout lr
        }

        component ss.users "Diagram3_users" {
            include *
            autolayout lr
        }

        component ss.courses "Diagram3_courses" {
            include *
            autolayout lr
        }

        component ss.purchases "Diagram3_purchases" {
            include *
            autolayout lr
        }

        component ss.certificates "Diagram3_certificates" {
            include *
            autolayout lr
        }

        component ss.notifications "Diagram3_notifications" {
            include *
            autolayout lr
        }

        styles {
            element "Person" {
                shape person
                background #64BFBA
                color white
            }
            element "Frontend" {
                shape webbrowser
                background #9715D3
                color white
            }
            element "Database" {
                shape cylinder
            }
            element "Service" {
                shape hexagon
            }
            element "Topic" {
                shape pipe
                color #ff6600
                background #F8F8F8
            }

            element "Software System" {
                shape roundedbox
                background #282828
                color white
            }

            element "Users" {
                background #A0041E
                color white
            }
            element "Courses" {
                background #DD2E44
                color white
            }
            element "Purchases" {
                background #F4900C
                color white
            }
            element "Certificates" {
                background #55ACEE
                color white
            }
            element "Notifications" {
                background #555555
                color white
            }

            relationship "Http" {
                color black
            }

            relationship "Topic in" {
                style dotted
                color #339933
            }

            relationship "Topic out" {
                style dotted
                color #ff6600
            }
        }
    }
}
