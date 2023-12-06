using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices.V1;
using Application.DTO.UserManagement;
using Domain._Shared.Events.Subscribed.UserAuthentication;
using Domain._Shared.Events.Topics.UserManagement;
using Domain.UserManagement.Entities;
using Domain.UserManagement.Enums;
using Domain.UserManagement.Repositories;
using Domain.UserManagement.Services._Interfaces;
using FluentAssertions;
using Infrastructure.Persistence._Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using Unit_Tests.UserManagement.Application.TestSetup;

namespace Unit_Tests.UserManagement.Application;

public class UserManagerServiceTests
{
    private readonly Mock<IUserValidationService> _mockUserValidationService;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ILogger<UserManagerService>> _mockLogger;
    private readonly Mock<IEventHandler> _mockEventHandler;
    private readonly Mock<ICacheManager> _mockCacheManager;
    private readonly UserManagerService _userManagerService;

    public UserManagerServiceTests()
    {
        _mockUserValidationService = new Mock<IUserValidationService>();
        _mockUserRepository = new Mock<IUserRepository>();
        _mockLogger = new Mock<ILogger<UserManagerService>>();
        _mockEventHandler = new Mock<IEventHandler>();
        _mockCacheManager = new Mock<ICacheManager>();

        _userManagerService = new UserManagerService(
            _mockUserValidationService.Object,
            _mockUserRepository.Object,
            _mockLogger.Object,
            _mockEventHandler.Object,
            _mockCacheManager.Object
        );
    }
    
    [Theory]
    [MemberData(nameof(UserManagerServiceTestCases.RegistrationTestCases), MemberType = typeof(UserManagerServiceTestCases))]
    public async Task RegisterAsync_VariousScenarios_ReturnsExpectedResult(UserRegisterDto userDto, bool expectedResult, int eventPublishTimes)
    {
        // Arrange
        var validationResult = new ValidationResult();
        _mockUserValidationService.Setup(s => s.ValidateNewUser(userDto.Email, userDto.Password))
            .Returns(validationResult);
        _mockUserRepository.Setup(r => r.CreateUserIfNotRegisteredAsync(It.Is<User>(u => u.Email == userDto.Email)))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _userManagerService.RegisterAsync(userDto);

        // Assert
        result.IsSuccess.Should().Be(expectedResult);
        _mockEventHandler.Verify(e => e.PublishProtobufEventAsync(It.IsAny<UserRegInitEvent>()), Times.Exactly(eventPublishTimes));
    }

    
    [Fact]
    public async Task DeleteUserByEmailAsync_UserExists_DeletesSuccessfully()
    {
        // Arrange
        var email = "test@example.com";
        _mockUserRepository.Setup(r => r.DeleteUserByEmailAsync(email)).ReturnsAsync(true);

        // Act
        var result = await _userManagerService.DeleteUserByEmailAsync(email);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _mockCacheManager.Verify(c => c.RemoveValueAsync(It.IsAny<string>()), Times.Once);
        _mockEventHandler.Verify(e => e.PublishProtobufEventAsync(It.IsAny<UserDeletionInitEvent>()), Times.Once);
        // Additional
        //
        // assertions...
    }
    
    [Fact]
    public async Task UpdateUserStatusByEmailAsync_ValidUser_UpdatesSuccessfully()
    {
        // Arrange
        var email = "test@example.com";
        var statusEvent = new UserAuthDetailsSetSuccessEvent { Email = email };
        _mockUserRepository.Setup(r => r.UpdateUserStatusByEmailAsync(email, It.IsAny<UserStatus>())).ReturnsAsync(true);

        // Act
        var result = await _userManagerService.UpdateUserStatusByEmailAsync(statusEvent, UserStatus.Active);

        // Assert
        result.IsSuccess.Should().BeTrue();
        // Additional assertions...
    }

    [Fact]
    public async Task RollBackUserAsync_ValidUser_RollsBackSuccessfully()
    {
        // Arrange
        var email = "test@example.com";
        var failedEvent = new UserAuthDetailsSetFailedEvent { Email = email };
        _mockUserRepository.Setup(r => r.DeleteUserByEmailAsync(email)).ReturnsAsync(true);

        // Act
        var result = await _userManagerService.RollBackUserAsync(failedEvent);

        // Assert
        result.IsSuccess.Should().BeTrue();
        // Additional assertions...
    }
}