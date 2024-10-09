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

                confirmEmail = component "ConfirmEmail" "" "" "Frontend"
                forgotPassword = component "ForgotPassword" "" "" "Frontend"
                login = component "Login" "" "" "Frontend"
                register = component "Register" "" "" "Frontend"
                resetPassword = component "ResetPassword" "" "" "Frontend"
                twoFactorAuth = component "TwoFactorAuth" "" "" "Frontend"

                catalog = component "Catalog" "" "" "Frontend"
                certificates = component "Certificates" "" "" "Frontend"
                checkTasks = component "CheckTasks" "" "" "Frontend"
                course = component "Course" "" "" "Frontend"
                createCourse = component "CreateCourse" "" "" "Frontend"
                editCourse = component "EditCourse" "" "" "Frontend"
                landing = component "Landing" "" "" "Frontend"
                manageModerators = component "ManageModerators" "" "" "Frontend"
                oauthRedirect = component "OauthRedirect" "" "" "Frontend"
                preview = component "Preview" "" "" "Frontend"
                profile = component "Profile" "" "" "Frontend"
                purchase = component "Purchase" "" "" "Frontend"
                subscriptions = component "Subscriptions" "" "" "Frontend"
                teaching = component "Teaching" "" "" "Frontend"

                notifications = component "Notifications" "" "" "Frontend"
            }
            group "Users Service" {
                users = container "Users" "Users authentication and authorization service /\n API Gateway" "C#, ASP.NET Core" {
                    tags "Service" "Users"

                    group "Controllers" {
                        usersController = component "UsersController" "" "" "Users"
                        profileController = component "ProfileController" "" "" "Users"
                    }

                    group "Producers" {
                        profileProducer = component "ProfileProducer" "" "" "Users"
                        userCreatedProducer = component "UserCreatedProducer" "" "" "Users"
                    }

                    group "Services" {
                        emailSender = component "EmailSender" "" "" "Users"
                        profileService = component "ProfileService" "" "" "Users"
                        usersService = component "UsersService" "" "" "Users"
                    }

                    group "API Gateway" {
                        ocelot = component "Ocelot" "" "" "Users"
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
                        accountsController = component "AccountsController" "" "" "Courses"
                        catalogController = component "CatalogController" "" "" "Courses"
                        categoriesController = component "CategoriesController" "" "" "Courses"
                        checkTasksController = component "CheckTasksController" "" "" "Courses"
                        coursesController = component "CoursesController" "" "" "Courses"
                        editCoursesController = component "EditCoursesController" "" "" "Courses"
                        landingController = component "LandingController" "" "" "Courses"
                        moderatorsCourseController = component "ModeratorsCourseController" "" "" "Courses"
                        teachingController = component "TeachingController" "" "" "Courses"
                    }

                    group "Services" {
                        accountsService = component "AccountsService" "" "" "Courses"
                        catalogService = component "CatalogService" "" "" "Courses"
                        checkTaskManagementService = component "CheckTaskManagementService" "" "" "Courses"
                        checkTasksService = component "CheckTasksService" "" "" "Courses"
                        courseManagementService = component "CourseManagementService" "" "" "Courses"
                        courseService = component "CourseService" "" "" "Courses"
                        landingPageContentService = component "LandingPageContentService" "" "" "Courses"
                        manualReviewService = component "ManualReviewService" "" "" "Courses"
                        notificationPreparerService = component "NotificationPreparerService" "" "" "Courses"
                        subscriptionManagementService = component "SubscriptionManagementService" "" "" "Courses"
                        teachingService = component "TeachingService" "" "" "Courses"
                        updateTasksBlockService = component "UpdateTasksBlockService" "" "" "Courses"
                    }

                    group "Repositories" {
                        userCourseInfoRepository = component "UserCourseInfoRepository" "" "" "Courses"
                        userRepository = component "UserRepository" "" "" "Courses"
                        userTaskPointsRepository = component "UserTaskPointsRepository" "" "" "Courses"
                        blockCompletedInfoRepository = component "BlockCompletedInfoRepository" "" "" "Courses"
                        blockRepository = component "BlockRepository" "" "" "Courses"
                        categoryRepository = component "CategoryRepository" "" "" "Courses"
                        courseRepository = component "CourseRepository" "" "" "Courses"
                        manualReviewTaskUserAnswerRepository = component "ManualReviewTaskUserAnswerRepository" "" "" "Courses"
                        moduleRepository = component "ModuleRepository" "" "" "Courses"
                    }

                    group "Consumers" {
                        profilePhotoUpdatedConsumer = component "ProfilePhotoUpdatedConsumer" "" "" "Courses"
                        profileUpdatedConsumer = component "ProfileUpdatedConsumer" "" "" "Courses"
                        purchaseCreatedConsumer = component "PurchaseCreatedConsumer" "" "" "Courses"
                        subscriptionChangedConsumer = component "SubscriptionChangedConsumer" "" "" "Courses"
                        userCreatedConsumer = component "UserCreatedConsumer" "" "" "Courses"
                    }

                    group "Producers" {
                        courseCompletedProducer = component "CourseCompletedProducer" "" "" "Courses"
                        courseCreatedProducer = component "CourseCreatedProducer" "" "" "Courses"
                        courseUpdatedProducer = component "CourseUpdatedProducer" "" "" "Courses"
                        notificationSentProducer = component "NotificationSentProducer" "" "" "Courses"
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

                    notificationSentProducer -> notificationPreparerService "Prepares notifications using"
                    notificationSentProducer -> userRepository "Uses to prepare notifications for all users"
                }
                courses_db = container "Courses Database" "Stores courses and their content" "PostgreSQL" {
                    tags "Database" "Courses"
                }
            }
            group "Purchases Service" {
                purchases = container "Purchases" "Implements business logic for purchases and subsciption handling" "C#, ASP.NET Core" {
                    tags "Service" "Purchases"

                    group "Controllers" {
                        purchasesController = component "PurchasesController" "" "" "Purchases"
                        purchasesMakingController = component "PurchasesMakingController" "" "" "Purchases"
                        subscriptionsController = component "SubscriptionsController" "" "" "Purchases"
                    }

                    group "Services" {
                        purchaseMakingService = component "PurchaseMakingService" "" "" "Purchases"
                        purchaseService = component "PurchaseService" "" "" "Purchases"
                        purchaseTokenService = component "PurchaseTokenService" "" "" "Purchases"
                        subscriptionManagementService = component "SubscriptionManagementService" "" "" "Purchases"
                        validatePurchaseService = component "ValidatePurchaseService" "" "" "Purchases"
                    }

                    group "Repositories" {
                        courseRepository = component "CourseRepository" "" "" "Purchases"
                        purchaseRepository = component "PurchaseRepository" "" "" "Purchases"
                        subscriptionRepository = component "SubscriptionRepository" "" "" "Purchases"
                        userCourseInfoRepository = component "UserCourseInfoRepository" "" "" "Purchases"
                        userRepository = component "UserRepository" "" "" "Purchases"
                    }

                    group "Consumers" {
                        courseCompletedConsumer = component "CourseCompletedConsumer" "" "" "Purchases"
                        courseCreatedConsumer = component "CourseCreatedConsumer" "" "" "Purchases"
                        courseUpdatedConsumer = component "CourseUpdatedConsumer" "" "" "Purchases"
                        userCreatedConsumer = component "UserCreatedConsumer" "" "" "Purchases"
                    }

                    group "Producers" {
                        purchaseCreatedProducer = component "PurchaseCreatedProducer" "" "" "Purchases"
                        subscriptionChangedProducer = component "SubscriptionChangedProducer" "" "" "Purchases"
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
                        certificateController = component "CertificateController" "" "" "Certificates"
                        certificateService = component "CertificateService" "" "" "Certificates"
                    }

                    group "OrderModule" {
                        orderController = component "OrderController" "" "" "Certificates"
                        orderService = component "OrderService" "" "" "Certificates"
                    }

                    group "CourseModule" {
                        courseService = component "CourseService" "" "" "Certificates"
                    }

                    group "UserModule" {
                        userService = component "UserService" "" "" "Certificates"
                    }

                    group "UserCourseInfoModule" {
                        userCourseInfoService = component "UserCourseInfoService" "" "" "Certificates"
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
                        notificationsController = component "NotificationsController" "" "" "Notifications"
                    }

                    group "Hubs" {
                        notificationsHub = component "NotificationsHub" "" "" "Notifications"
                    }

                    group "Services" {
                        notificationSenderService = component "NotificationSenderService" "" "" "Notifications"
                    }

                    group "Repositories" {
                        notificationRepository = component "NotificationRepository" "" "" "Notifications"
                    }

                    group "Consumers" {
                        notificationSentConsumer = component "NotificationSentConsumer" "" "" "Notifications"
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

        u -> vk "Has an account on"
        gmail -> u "Sends emails to"

        ss -> mb "Uses to display map and verify order address"
        ss -> gmail "Uses for sending emails to users"
        ss.users.emailSender -> gmail "Sends emails to user via"
        ss.users.usersService -> vk "Optionally authorizes user via"
        ss.certificates.certificateService -> mb "Verifies user address using"
        ss.frontend.certificates -> mb "Displays user address using"
        vkStrikesBack = vk -> ss.frontend.oauthRedirect "Redirects with authorization info to"
        ss.frontend.login -> vk "Links to"

        u -> ss.frontend "Uses"
        u -> ss.frontend.confirmEmail "Confirms email from link and sets up 2fa using"
        u -> ss.frontend.forgotPassword "Requests password reset using"
        u -> ss.frontend.login "Logs in on"
        u -> ss.frontend.register "Registers on"
        u -> ss.frontend.resetPassword "Enters new password on link sent to email"
        u -> ss.frontend.twoFactorAuth "Enters two-factor code for login on"
        u -> ss.frontend.catalog "Browses courses on"
        u -> ss.frontend.certificates "Orders certificate to their physical address on"
        u -> ss.frontend.checkTasks "Checks their student's tasks on"
        u -> ss.frontend.course "Learns and completes tasks on"
        u -> ss.frontend.createCourse "Requests a new course on"
        u -> ss.frontend.editCourse "Edits their course on"
        u -> ss.frontend.landing "Browses their started courses when logged in, and browses popular ones when logged out"
        u -> ss.frontend.manageModerators "Assigns moderators for their course on"
        u -> ss.frontend.oauthRedirect "Redirected by vk on"
        u -> ss.frontend.preview "Explores a specific course on"
        u -> ss.frontend.profile "Views or edits their profile info on, and check their started and completed courses on"
        u -> ss.frontend.purchase "Purchases courses, subscriptions or certificates on"
        u -> ss.frontend.subscriptions "Previews subscription plans on"
        u -> ss.frontend.teaching "Views or edits their created courses on"
        u -> ss.frontend.notifications "Receives notifications from"

        ss.frontend -> ss.users "Makes API calls to" "JSON/HTTP" "Http"
        ss.frontend.confirmEmail -> ss.users.usersController "Confirms email and sets up 2FA auth using"
        ss.frontend.forgotPassword -> ss.users.usersController "Changes password using"
        ss.frontend.login -> ss.users.usersController "Gets 2fa info from and logins in using"
        ss.frontend.register -> ss.users.usersController "Registers using"
        ss.frontend.resetPassword -> ss.users.usersController "Resets password using"
        ss.frontend.twoFactorAuth -> ss.users.usersController "Verifies 2fa and logs in via"
        ss.frontend.catalog -> ss.users.ocelot "Requests catalog from"
        ss.frontend.certificates -> ss.users.ocelot "Requests certificate info from"
        ss.frontend.checkTasks -> ss.users.ocelot "Gets points from and sends new points to"
        ss.frontend.course -> ss.users.ocelot "Gets course info from"
        ss.frontend.createCourse -> ss.users.ocelot "Sends a request to create course to"
        ss.frontend.editCourse -> ss.users.ocelot "Sends edited course to"
        ss.frontend.landing -> ss.users.ocelot "Requests info about courses from"
        ss.frontend.manageModerators -> ss.users.ocelot "Send requests to update moderators to"
        ss.frontend.oauthRedirect -> ss.users.usersController "Accepts info from VK and saves it using"
        ss.frontend.preview -> ss.users.ocelot "Gets course preview from"
        ss.frontend.profile -> ss.users.profileController "Gets profile info from"
        ss.frontend.purchase -> ss.users.ocelot "Sends purchase requests to"
        ss.frontend.subscriptions -> ss.users.ocelot "Gets subscriptions info from"
        ss.frontend.teaching -> ss.users.ocelot "Gets info about authored and moderated courses from"
        ss.frontend.notifications -> ss.users.ocelot "Gets notifications from and requests to mark them as read on"

        ss.users.ocelot -> ss.courses.accountsController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.catalogController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.categoriesController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.checkTasksController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.coursesController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.editCoursesController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.landingController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.moderatorsCourseController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.courses.teachingController "Authorizes and routes API requests to" "JSON/HTTP" "Http"

        ss.users.ocelot -> ss.purchases.purchasesController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.purchases.purchasesMakingController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.purchases.subscriptionsController "Authorizes and routes API requests to" "JSON/HTTP" "Http"

        ss.users.ocelot -> ss.certificates.certificateController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.certificates.orderController "Authorizes and routes API requests to" "JSON/HTTP" "Http"

        ss.users.ocelot -> ss.notifications.notificationsController "Authorizes and routes API requests to" "JSON/HTTP" "Http"
        ss.users.ocelot -> ss.notifications.notificationsHub "Authorizes and routes API requests to" "HTTP" "Http"

        ss.users.profileService -> ss.users_db "Reads from and writes to"
        ss.users.usersService -> ss.users_db "Reads from and writes to"
        ss.courses.userCourseInfoRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.userRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.userTaskPointsRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.blockCompletedInfoRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.blockRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.categoryRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.courseRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.manualReviewTaskUserAnswerRepository -> ss.courses_db "Reads from and writes to"
        ss.courses.moduleRepository -> ss.courses_db "Reads from and writes to"
        ss.purchases.courseRepository -> ss.purchases_db "Reads from and writes to"
        ss.purchases.purchaseRepository -> ss.purchases_db "Reads from and writes to"
        ss.purchases.subscriptionRepository -> ss.purchases_db "Reads from and writes to"
        ss.purchases.userCourseInfoRepository -> ss.purchases_db "Reads from and writes to"
        ss.purchases.userRepository -> ss.purchases_db "Reads from and writes to"
        ss.certificates.certificateService -> ss.certificates_db "Reads from and writes to"
        ss.certificates.orderService -> ss.certificates_db "Reads from and writes to"
        ss.certificates.courseService -> ss.certificates_db "Reads from and writes to"
        ss.certificates.userService -> ss.certificates_db "Reads from and writes to"
        ss.certificates.userCourseInfoService -> ss.certificates_db "Reads from and writes to"
        ss.notifications.notificationRepository -> ss.notifications_db "Reads from and writes to"

        ss.courses.courseCompletedProducer -> ss.courseCompleted "Publishes messages to" "" "Topic in"
        ss.courseCompleted -> ss.purchases.courseCompletedConsumer "Sends messages to" "" "Topic out"

        ss.courses.courseCreatedProducer -> ss.courseCreated "Publishes messages to" "" "Topic in"
        ss.courseCreated -> ss.purchases.courseCreatedConsumer "Sends messages to" "" "Topic out"
        ss.courseCreated -> ss.certificates.courseService "Sends messages to" "" "Topic out"

        ss.courses.courseUpdatedProducer -> ss.courseUpdated "Publishes messages to" "" "Topic in"
        ss.courseUpdated -> ss.purchases.courseUpdatedConsumer "Sends messages to" "" "Topic out"
        ss.courseUpdated -> ss.certificates.courseService "Sends messages to" "" "Topic out"

        ss.courses.notificationSentProducer -> ss.notificationSent "Publishes messages to" "" "Topic in"
        ss.notificationSent -> ss.notifications.notificationSentConsumer "Sends messages to" "" "Topic out"

        ss.users.profileProducer -> ss.profilePhotoUpdated "Publishes messages to" "" "Topic in"
        ss.profilePhotoUpdated -> ss.courses.profilePhotoUpdatedConsumer "Sends messages to" "" "Topic out"
        ss.profilePhotoUpdated -> ss.certificates.userService "Sends messages to" "" "Topic out"

        ss.users.profileProducer -> ss.profileUpdated "Publishes messages to" "" "Topic in"
        ss.profileUpdated -> ss.courses.profileUpdatedConsumer "Sends messages to" "" "Topic out"
        ss.profileUpdated -> ss.certificates.userService "Sends messages to" "" "Topic out"

        ss.purchases.purchaseCreatedProducer -> ss.purchaseCreated "Publishes messages to" "" "Topic in"
        ss.purchaseCreated -> ss.courses.purchaseCreatedConsumer "Sends messages to" "" "Topic out"
        ss.purchaseCreated -> ss.certificates.userCourseInfoService "Sends messages to" "" "Topic out"

        ss.purchases.subscriptionChangedProducer -> ss.subscriptionChanged "Publishes messages to" "" "Topic in"
        ss.subscriptionChanged -> ss.courses.subscriptionChangedConsumer "Sends messages to" "" "Topic out"

        ss.users.userCreatedProducer -> ss.userCreated "Publishes messages to" "" "Topic in"
        ss.userCreated -> ss.courses.userCreatedConsumer "Sends messages to" "" "Topic out"
        ss.userCreated -> ss.purchases.userCreatedConsumer "Sends messages to" "" "Topic out"
        ss.userCreated -> ss.certificates.userService "Sends messages to" "" "Topic out"
    }

    views {
        systemContext ss "Diagram1" {
            include *
            exclude vkStrikesBack
            autolayout tb
        }

        container ss "Diagram2" {
            include *
        }

        component ss.frontend "Diagram3-1_frontend" {
            include *
        }

        component ss.users "Diagram3-2_users" {
            include *
        }

        component ss.courses "Diagram3-3_courses" {
            include *
        }

        component ss.purchases "Diagram3-4_purchases" {
            include *
        }

        component ss.certificates "Diagram3-5_certificates" {
            include *
        }

        component ss.notifications "Diagram3-6_notifications" {
            include *
        }

        styles {
            element "Component" {
                shape roundedbox
            }
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
