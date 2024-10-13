using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Purchases.Data.Abstractions;
using Purchases.Data.Enums;
using Purchases.Data.Models;
using Purchases.Data.Models.PurchaseDetails;
using Purchases.Dtos;
using Purchases.Dtos.Responses;
using Purchases.Services;

namespace Tests.Purchases.UnitTests.ServiceTests;

public class SubscriptionManagementServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPurchaseRepository> _purchaseRepository = new();
    private readonly Mock<ISubscriptionRepository> _subscriptionRepository = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly SubscriptionManagementService _subscriptionManagementService;

    public SubscriptionManagementServiceTests()
    {
        _subscriptionManagementService = new SubscriptionManagementService(
            _userRepository.Object,
            _purchaseRepository.Object,
            _subscriptionRepository.Object,
            _mapper.Object
        );
    }

    [Fact]
    public async Task GetActiveSubscription_UserNotFound_ReturnsNotFound()
    {
        _userRepository
            .Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true))
            .ReturnsAsync((User)null);

        var result = await _subscriptionManagementService.GetActiveSubscription(Guid.NewGuid());

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetActiveSubscription_UserHasFreeSubscription_ReturnsNotFound()
    {
        var user = new User
        {
            Subscription = new Subscription { Id = (int)SubscriptionType.Free },
        };
        _userRepository.Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true)).ReturnsAsync(user);

        var result = await _subscriptionManagementService.GetActiveSubscription(Guid.NewGuid());

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetActiveSubscription_ValidUser_ReturnsOkWithSubscription()
    {
        var endDate = DateTime.Today.AddDays(10);
        var user = new User
        {
            SubscriptionId = (int)SubscriptionType.Pro,
            Subscription = new Subscription { Id = (int)SubscriptionType.Pro },
            Purchases = new List<Purchase>
            {
                new Purchase
                {
                    PurchaseDetails = new SubscriptionPurchaseDetails
                    {
                        PurchaseType = PurchaseType.Subscription,
                        SubscriptionId = (int)SubscriptionType.Pro,
                        EndDate = endDate,
                        AutoRenewalEnabled = true
                    }
                }
            }
        };
        var subscriptionDto = new SubscriptionUserDto();
        _userRepository.Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true)).ReturnsAsync(user);
        _mapper
            .Setup(m => m.Map<SubscriptionUserDto>(It.IsAny<Subscription>()))
            .Returns(subscriptionDto);

        var result = await _subscriptionManagementService.GetActiveSubscription(Guid.NewGuid());

        var okResult = Assert.IsType<Ok<SubscriptionUserDto>>(result);
        var returnedSubscription = Assert.IsType<SubscriptionUserDto>(okResult.Value);
        Assert.Equal(endDate, returnedSubscription.EndDate);
        Assert.True(returnedSubscription.AutoRenewalEnabled);
    }

    [Fact]
    public async Task GetAllSubscriptions_NoSubscriptionsFound_ReturnsNotFound()
    {
        _subscriptionRepository
            .Setup(r => r.GetAllSubscriptions())
            .ReturnsAsync(new List<Subscription>());

        var result = await _subscriptionManagementService.GetAllSubscriptions();

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetAllSubscriptions_SubscriptionsFound_ReturnsOkWithSubscriptions()
    {
        var subscriptions = new List<Subscription> { new Subscription(), new Subscription() };
        var subscriptionDtos = new List<SubscriptionDto>
        {
            new SubscriptionDto(),
            new SubscriptionDto()
        };
        _subscriptionRepository.Setup(r => r.GetAllSubscriptions()).ReturnsAsync(subscriptions);
        _mapper.Setup(m => m.Map<List<SubscriptionDto>>(subscriptions)).Returns(subscriptionDtos);

        var result = await _subscriptionManagementService.GetAllSubscriptions();

        var okResult = Assert.IsType<Ok<List<SubscriptionDto>>>(result);
        var returnedSubscriptions = Assert.IsType<List<SubscriptionDto>>(okResult.Value);
        Assert.Equal(subscriptionDtos.Count, returnedSubscriptions.Count);
    }

    [Fact]
    public async Task GetSubscriptionsByUser_UserNotFound_ReturnsNotFound()
    {
        _userRepository
            .Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true))
            .ReturnsAsync((User)null);

        var result = await _subscriptionManagementService.GetSubscriptionsByUser(Guid.NewGuid());

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task GetSubscriptionsByUser_NoActiveSubscription_ReturnsOk()
    {
        var user = new User { Purchases = new List<Purchase>() };
        _userRepository.Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true)).ReturnsAsync(user);

        var result = await _subscriptionManagementService.GetSubscriptionsByUser(Guid.NewGuid());

        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task GetSubscriptionsByUser_WithActiveSubscription_ReturnsOkWithDetails()
    {
        var user = new User
        {
            Purchases = new List<Purchase>
            {
                new Purchase
                {
                    Id = 1,
                    PurchaseDetails = new SubscriptionPurchaseDetails
                    {
                        EndDate = DateTime.Now.AddDays(1),
                        AutoRenewalEnabled = true
                    }
                }
            }
        };
        _userRepository.Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true)).ReturnsAsync(user);
        var subUserDto = new SubscriptionUserDto();
        _mapper
            .Setup(m => m.Map<SubscriptionUserDto>(It.IsAny<Subscription>()))
            .Returns(subUserDto);

        var result = await _subscriptionManagementService.GetSubscriptionsByUser(Guid.NewGuid());
        var okResult = Assert.IsType<Ok<SubscriptionUserDto>>(result);
        var data = Assert.IsType<SubscriptionUserDto>(okResult.Value);

        Assert.Equal(subUserDto, data);
    }

    [Fact]
    public async Task UpdateAutoRenewal_UserNotFound_ReturnsNotFound()
    {
        _userRepository
            .Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true))
            .ReturnsAsync((User)null);

        var result = await _subscriptionManagementService.UpdateAutoRenewal(
            Guid.NewGuid(),
            1,
            true
        );

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task UpdateAutoRenewal_Success_ReturnsOk()
    {
        var user = new User();
        _userRepository.Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true)).ReturnsAsync(user);
        _purchaseRepository
            .Setup(r => r.UpdateAutoRenewal(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(true);

        var result = await _subscriptionManagementService.UpdateAutoRenewal(
            Guid.NewGuid(),
            1,
            true
        );

        Assert.IsType<Ok>(result);
    }

    [Fact]
    public async Task UpdateAutoRenewal_Failure_ReturnsNotFound()
    {
        var user = new User();
        _userRepository.Setup(r => r.GetAsync(It.IsAny<Guid>(), true, true)).ReturnsAsync(user);
        _purchaseRepository
            .Setup(r => r.UpdateAutoRenewal(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(false);

        var result = await _subscriptionManagementService.UpdateAutoRenewal(
            Guid.NewGuid(),
            1,
            false
        );

        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public async Task UpdateSubscriptionWithAutoRenewal_CallsRepositoryMethod()
    {
        await _subscriptionManagementService.UpdateSubscriptionWithAutoRenewal();

        _purchaseRepository.Verify(r => r.UpdateSubscriptionWithAutoRenewal(), Times.Once);
    }
}
