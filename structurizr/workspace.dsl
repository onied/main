workspace "Onied" {

    !identifiers hierarchical

    model {
        u = person "User" "Typically a self-learner who wants to receive an online education on some of the topics"
        vk = softwareSystem "VK" "vk.com" {
             tags "External"
        }
        gmail = softwareSystem "Gmail" {
             tags "External"
        }
        mb = softwareSystem "Mapbox" {
             tags "External"
        }
        ss = softwareSystem "Onied" "Learning Management System" {
            frontend = container "Single-Page Application" "Implements UI for all the user-facing features in the LMS" "JS/TS, React" {
                tags "Frontend"

                confirmEmail = component "ConfirmEmail"
                forgotPassword = component "ForgotPassword"
                login = component "Login"
                register = component "Register"
                resetPassword = component "ResetPassword"
                twoFactorAuth = component "TwoFactorAuth"

                catalog = component "Catalog"
                certificates = component "Certificates"
                checkTasks = component "CheckTasks"
                course = component "Course"
                createCourse = component "CreateCourse"
                editCourse = component "EditCourse"
                landing = component "Landing"
                manageModerators = component "ManageModerators"
                oauthRedirect = component "OauthRedirect"
                preview = component "Preview"
                profile = component "Profile"
                purchase = component "Purchase"
                subscriptions = component "Subscriptions"
                teaching = component "Teaching"
            }
            group "Users Service" {
                users = container "Users" "Users authentication and authorization service /\n API Gateway" "C#, ASP.NET Core" {
                    tags "Service" "Users"

                    group "Controllers" {
                        usersController = component "UsersController"
                        profileController = component "ProfileController"
                    }

                    group "Producers" {
                        profileProducer = component "ProfileProducer"
                        userCreatedProducer = component "UserCreatedProducer"
                    }

                    group "Services" {
                        emailSender = component "EmailSender"
                        profileService = component "ProfileService"
                        usersService = component "UsersService"
                    }

                    group "API Gateway" {
                        ocelot = component "Ocelot"
                    }

                    usersController -> usersService "Passes requests to"
                    profileController -> profileService "Passes requests to"

                    usersService -> emailSender "Uses to send confirmation emails"
                    usersService -> userCreatedProducer "Uses to notify other services of a new user"

                    profileService -> profileProducer "Uses to notify other services when user's profile changes"
                }
                users_db = container "Users Database" "Stores data related to users" "PostgreSQL"  {
                    tags "Database" "Users"
                }
            }
            group "Courses Service" {
                courses = container "Courses" "Implements business logic for courses and their content (including learning materials)" "C#, ASP.NET Core" {
                    tags "Service" "Courses"

                    group "Controllers" {
                        accountsController = component "AccountsController"
                        catalogController = component "CatalogController"
                        categoriesController = component "CategoriesController"
                        checkTasksController = component "CheckTasksController"
                        coursesController = component "CoursesController"
                        editCoursesController = component "EditCoursesController"
                        landingController = component "LandingController"
                        moderatorsCourseController = component "ModeratorsCourseController"
                        teachingController = component "TeachingController"
                    }

                    group "Services" {
                        accountsService = component "AccountsService"
                        catalogService = component "CatalogService"
                        checkTaskManagementService = component "CheckTaskManagementService"
                        checkTasksService = component "CheckTasksService"
                        courseManagementService = component "CourseManagementService"
                        courseService = component "CourseService"
                        landingPageContentService = component "LandingPageContentService"
                        manualReviewService = component "ManualReviewService"
                        notificationPreparerService = component "NotificationPreparerService"
                        subscriptionManagementService = component "SubscriptionManagementService"
                        teachingService = component "TeachingService"
                        updateTasksBlockService = component "UpdateTasksBlockService"
                    }

                    group "Repositories" {
                        userCourseInfoRepository = component "UserCourseInfoRepository"
                        userRepository = component "UserRepository"
                        userTaskPointsRepository = component "UserTaskPointsRepository"
                        blockCompletedInfoRepository = component "BlockCompletedInfoRepository"
                        blockRepository = component "BlockRepository"
                        categoryRepository = component "CategoryRepository"
                        courseRepository = component "CourseRepository"
                        manualReviewTaskUserAnswerRepository = component "ManualReviewTaskUserAnswerRepository"
                        moduleRepository = component "ModuleRepository"
                    }

                    group "Consumers" {
                        profilePhotoUpdatedConsumer = component "ProfilePhotoUpdatedConsumer"
                        profileUpdatedConsumer = component "ProfileUpdatedConsumer"
                        purchaseCreatedConsumer = component "PurchaseCreatedConsumer"
                        subscriptionChangedConsumer = component "SubscriptionChangedConsumer"
                        userCreatedConsumer = component "UserCreatedConsumer"
                    }

                    group "Producers" {
                        courseCompletedProducer = component "CourseCompletedProducer"
                        courseCreatedProducer = component "CourseCreatedProducer"
                        courseUpdatedProducer = component "CourseUpdatedProducer"
                        notificationSentProducer = component "NotificationSentProducer"
                    }

                    accountsController -> accountsService "Passes requests to"
                    catalogController -> catalogService "Passes requests to"
                    categoriesController -> categoryRepository "Gets all categories from"
                    checkTasksController -> checkTaskManagementService "Passes requests to"
                    coursesController -> courseService "Passes requests to"
                    editCoursesController -> courseManagementService "Passes requests to"
                    landingController -> landingPageContentService "Gets content from"
                    moderatorsCourseController -> courseManagementService "Passes requests to"
                    teachingController -> manualReviewService "Passes requests to"
                    teachingController -> teachingService "Gets data from"

                    accountsService -> userRepository "Uses to find user with courses"
                    catalogService -> courseRepository "Uses to find courses"
                    catalogService -> userRepository "Uses to find user with courses"
                    checkTaskManagementService -> courseRepository "Uses to find course with blocks"
                    checkTaskManagementService -> blockRepository "Uses to find task block"
                    checkTaskManagementService -> blockCompletedInfoRepository "Uses to manage completed blocks"
                    checkTaskManagementService -> userCourseInfoRepository "Uses to find if user has a course"
                    checkTaskManagementService -> checkTasksService "Uses to check tasks"
                    checkTaskManagementService -> courseCompletedProducer "Uses to notify other services that user has completed a course"
                    checkTaskManagementService -> courseManagementService "Uses to determine if user is allowed on the course"
                    checkTaskManagementService -> userTaskPointsRepository "Uses to manage task points"
                    checkTaskManagementService -> notificationSentProducer "Uses to notify a user of a finished course"
                    courseManagementService -> courseRepository "Uses to manage courses and moderators"
                    courseManagementService -> userCourseInfoRepository "Uses to find if user has a course"
                    courseManagementService -> blockRepository "Uses to manage blocks"
                    courseManagementService -> updateTasksBlockService "Uses to update tasks block"
                    courseManagementService -> moduleRepository "Uses to manage modules"
                    courseManagementService -> categoryRepository "Uses to find categories"
                    courseManagementService -> courseUpdatedProducer "Uses to notify other services of updated course"
                    courseManagementService -> subscriptionManagementService "Uses to verify user's subsciption privileges"
                    courseService -> courseRepository "Uses to find and create courses"
                    courseService -> userCourseInfoRepository "Uses to check and add courses to users"
                    courseService -> blockCompletedInfoRepository "Uses to check and mark blocks as completed"
                    courseService -> blockRepository "Uses to find blocks"
                    courseService -> userRepository "Uses to find users"
                    courseService -> courseCreatedProducer "Uses to notify other services of a new course"
                    courseService -> categoryRepository "Uses to get all categories"
                    courseService -> subscriptionManagementService "Uses to verify user's subscription privileges"
                    landingPageContentService -> courseRepository "Gets popular and recommended courses from"
                    manualReviewService -> userRepository "Uses to find users"
                    manualReviewService -> manualReviewTaskUserAnswerRepository "Uses to manage answer reviews"
                    manualReviewService -> checkTaskManagementService "Uses to check answer tasks as completed"
                    manualReviewService -> userTaskPointsRepository "Uses to get task points"
                    manualReviewService -> notificationSentProducer "Uses to notify a user of a reviewed task"
                    subscriptionManagementService -> userRepository "Uses to find users"
                    subscriptionManagementService -> courseRepository "Uses to update courses after getting new privileges"
                    subscriptionManagementService -> courseUpdatedProducer "Uses to notify other services of updated course"
                    teachingService -> userRepository "Uses to find users"
                    updateTasksBlockService -> blockRepository "Uses to get updated tasks block"

                    profilePhotoUpdatedConsumer -> userRepository "Updates profile photo using"
                    profileUpdatedConsumer -> userRepository "Updates profile using"
                    purchaseCreatedConsumer -> userCourseInfoRepository "Adds course to user after purchase using"
                    subscriptionChangedConsumer -> subscriptionManagementService "Updates courses after subscription change using"
                    userCreatedConsumer -> userRepository "Adds user to known users using"
                }
                courses_db = container "Courses Database" "Stores courses and their content" "PostgreSQL" {
                    tags "Database" "Courses"
                }
            }
            group "Purchases Service" {
                purchases = container "Purchases" "Implements business logic for purchases and subsciption handling" "C#, ASP.NET Core" {
                    tags "Service" "Purchases"

                    group "Controllers" {
                        purchasesController = component "PurchasesController"
                        purchasesMakingController = component "PurchasesMakingController"
                        subscriptionsController = component "SubscriptionsController"
                    }

                    group "Services" {
                        purchaseMakingService = component "PurchaseMakingService"
                        purchaseService = component "PurchaseService"
                        purchaseTokenService = component "PurchaseTokenService"
                        subscriptionManagementService = component "SubscriptionManagementService"
                        validatePurchaseService = component "ValidatePurchaseService"
                    }

                    group "Repositories" {
                        courseRepository = component "CourseRepository"
                        purchaseRepository = component "PurchaseRepository"
                        subscriptionRepository = component "SubscriptionRepository"
                        userCourseInfoRepository = component "UserCourseInfoRepository"
                        userRepository = component "UserRepository"
                    }

                    group "Consumers" {
                        courseCompletedConsumer = component "CourseCompletedConsumer"
                        courseCreatedConsumer = component "CourseCreatedConsumer"
                        courseUpdatedConsumer = component "CourseUpdatedConsumer"
                        userCreatedConsumer = component "UserCreatedConsumer"
                    }

                    group "Producers" {
                        purchaseCreatedProducer = component "PurchaseCreatedProducer"
                        subscriptionChangedProducer = component "SubscriptionChangedProducer"
                    }

                    purchasesController -> purchaseService "Passes requests to"
                    purchasesMakingController -> purchaseMakingService "Passes requests to"
                    subscriptionsController -> subscriptionManagementService "Passes requests to"

                    purchaseMakingService -> validatePurchaseService "Validates purchases using"
                    purchaseMakingService -> userRepository "Finds users and updates their subscription using"
                    purchaseMakingService -> courseRepository "Uses to find courses"
                    purchaseMakingService -> subscriptionRepository "Uses to find subscriptions"
                    purchaseMakingService -> purchaseRepository "Uses to store purchases"
                    purchaseMakingService -> purchaseTokenService "Uses to generate purchase token"
                    purchaseMakingService -> purchaseCreatedProducer "Uses to notify other services of a new purchase"
                    purchaseMakingService -> subscriptionChangedProducer "Uses to notify other services of a changed subscription of a user"
                    purchaseService -> userRepository "Uses to find users"
                    purchaseService -> purchaseRepository "Uses to find purchases"
                    purchaseService -> purchaseTokenService "Uses to decompose claims from tokens"
                    subscriptionManagementService -> userRepository "Uses to find users"
                    subscriptionManagementService -> purchaseRepository "Updates auto-renewal using"
                    subscriptionManagementService -> subscriptionRepository "Gets all subscriptions using"
                    validatePurchaseService -> userRepository "Uses to find users"
                    validatePurchaseService -> courseRepository "Uses to find courses"
                    validatePurchaseService -> subscriptionRepository "Uses to find subscriptions"
                    validatePurchaseService -> userCourseInfoRepository "Uses to find if user has course"

                    courseCompletedConsumer -> userRepository "Uses to find users"
                    courseCompletedConsumer -> courseRepository "Uses to find courses"
                    courseCompletedConsumer -> userCourseInfoRepository "Saves information about a user with a course using"
                    courseCreatedConsumer -> courseRepository "Saves a course using"
                    courseCreatedConsumer -> purchaseRepository "Adds a bogus purchase for course author using"
                    courseCreatedConsumer -> purchaseTokenService "Generates new token using"
                    courseCreatedConsumer -> purchaseCreatedProducer "Uses to notify other services of a new (bogus) purchase"
                    courseUpdatedConsumer -> courseRepository "Updates a course using"
                    userCreatedConsumer -> userRepository "Saves a new user ID using"
                }
                purchases_db = container "Purchases Database" "Stores purchases" "PostgreSQL" {
                    tags "Database" "Purchases"
                }
            }
            group "Certificates Service" {
                certificates = container "Certificates" "Implements business logic for ordering certificates upon course completion" "JS, Node.js" {
                    tags "Service" "Certificates"

                    group "CertificateModule" {
                        certificateController = component "CertificateController"
                        certificateService = component "CertificateService"
                    }

                    group "OrderModule" {
                        orderController = component "OrderController"
                        orderService = component "OrderService"
                    }

                    group "CourseModule" {
                        courseService = component "CourseService"
                    }

                    group "UserModule" {
                        userService = component "UserService"
                    }

                    group "UserCourseInfoModule" {
                        userCourseInfoService = component "UserCourseInfoService"
                    }

                    certificateController -> certificateService "Passes request to"
                    certificateService -> userService "Uses to find users"
                    certificateService -> courseService "Uses to find courses"
                    certificateService -> userCourseInfoService "Uses to check if user can buy certificate for course"
                    certificateService -> orderService "Uses to create orders and check existing orders"

                    orderController -> orderService "Uses to find orders"
                    orderController -> userService "Uses to find users"

                    userCourseInfoService -> orderService "Uses to find available certificates"
                }
                certificates_db = container "Certificates Database" "Stores orders and data required to generate certificates" "PostgreSQL" {
                    tags "Database" "Certificates"
                }
            }
            group "Notifications Service" {
                notifications = container "Notifications" "Implements business logic for sending notifications to users" "C#, ASP.NET Core"  {
                    tags "Service" "Notifications"

                    group "Controllers" {
                        notificationsController = component "NotificationsController"
                    }

                    group "Hubs" {
                        notificationsHub = component "NotificationsHub"
                    }

                    group "Services" {
                        notificationSenderService = component "NotificationSenderService"
                    }

                    group "Repositories" {
                        notificationRepository = component "NotificationRepository"
                    }

                    group "Consumers" {
                        notificationSentConsumer = component "NotificationSentConsumer"
                    }

                    notificationsController -> notificationRepository "Gets notification history from"
                    notificationsHub -> notificationRepository "Uses to mark notifications as read"
                    notificationSentConsumer -> notificationSenderService "Sends notifications using"
                    notificationSenderService -> notificationRepository "Adds notification to history using"
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
        ss.users.ocelot -> ss.courses "Routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.purchases "Routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.certificates "Routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.notifications "Routes API requests to" "JSON/HTTP" "Http"
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

        ss.users.emailSender -> gmail "Sends emails to user via"
        ss.users.usersService -> vk "Optionally authorizes user via"
        ss.certificates.certificateService -> mb "Verifies user address using"
        ss.frontend -> mb "Displays user address using"
        ss -> mb "Uses to display map and verify order address"
        ss -> gmail "Uses for sending emails to users"
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

            element "External" {
                shape roundedbox
                background #DEDEDE
                color black
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
