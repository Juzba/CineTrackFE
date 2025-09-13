using CineTrackFE.Tests.Helpers.TestSetups;
using FluentAssertions;
using System.Net;

namespace CineTrackFE.Tests.Unit.AppServices;


public class AuthServiceTests : IDisposable
{
    private readonly AuthServiceTestHelper _setup;

    public AuthServiceTests()
    {
        _setup = new AuthServiceTestHelper();
    }

    public void Dispose()
    {
        _setup?.Dispose();
    }


    [Fact]
    public async Task LoginAsync_ValidCredentials_ReturnsTrue()
    {
        // Arrange
        var email = AuthServiceTestHelper.TestData.Credentials.ValidEmail;
        var password = AuthServiceTestHelper.TestData.Credentials.ValidPassword;
        var expectedUser = AuthServiceTestHelper.TestData.CreateTestUser();
        var loginResponse = AuthServiceTestHelper.TestData.CreateLoginResponse(
            AuthServiceTestHelper.TestData.Tokens.ValidToken, expectedUser);

        _setup.SetupHttpResponse(HttpStatusCode.OK, loginResponse);

        // Act
        var result = await _setup.AuthService.LoginAsync(email, password);

        // Assert
        result.Should().BeTrue();
        _setup.AuthService.IsAuthenticated.Should().BeTrue();
        _setup.UserStore.User.Should().Be(expectedUser);
        _setup.HttpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();
        _setup.HttpClient.DefaultRequestHeaders.Authorization!.Scheme.Should().Be("Bearer");
        _setup.HttpClient.DefaultRequestHeaders.Authorization.Parameter.Should().Be(AuthServiceTestHelper.TestData.Tokens.ValidToken);
    }

    [Fact]
    public async Task LoginAsync_ValidCredentialsWithRememberMe_ReturnsTrue()
    {
        // Arrange
        var loginResponse = AuthServiceTestHelper.TestData.CreateLoginResponse();
        _setup.SetupHttpResponse(HttpStatusCode.OK, loginResponse);

        // Act
        var result = await _setup.AuthService.LoginAsync(
            AuthServiceTestHelper.TestData.Credentials.ValidEmail,
            AuthServiceTestHelper.TestData.Credentials.ValidPassword,
            true);

        // Assert
        result.Should().BeTrue();
        _setup.VerifyHttpRequest("/api/AuthApi/login", HttpMethod.Post,
            content => content.Should().Contain("\"RememberMe\":true"));
    }

    [Fact]
    public async Task LoginAsync_InvalidCredentials_ReturnsFalse()
    {
        // Arrange
        _setup.SetupHttpResponse(HttpStatusCode.Unauthorized);

        // Act
        var result = await _setup.AuthService.LoginAsync(
            AuthServiceTestHelper.TestData.Credentials.ValidEmail,
            AuthServiceTestHelper.TestData.Credentials.InvalidPassword);

        // Assert
        result.Should().BeFalse();
        _setup.AuthService.IsAuthenticated.Should().BeFalse();
        _setup.UserStore.User.Should().BeNull();
        _setup.HttpClient.DefaultRequestHeaders.Authorization.Should().BeNull();
    }

    [Theory]
    [InlineData("", "password")]
    [InlineData("   ", "password")]
    [InlineData(null, "password")]
    [InlineData("email", "")]
    [InlineData("email", "   ")]
    [InlineData("email", null)]
    public async Task LoginAsync_InvalidParameters_ThrowsArgumentException(string? email, string? password)
    {
        // Act
        var act = async () => await _setup.AuthService.LoginAsync(email!, password!);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task LoginAsync_HttpException_ThrowsExceptionWithMessage()
    {
        // Arrange
        _setup.SetupHttpException(new HttpRequestException("Network error"));

        // Act
        var act = async () => await _setup.AuthService.LoginAsync(
            AuthServiceTestHelper.TestData.Credentials.ValidEmail,
            AuthServiceTestHelper.TestData.Credentials.ValidPassword);

        // Assert
        var exception = await act.Should().ThrowAsync<Exception>()
            .WithMessage("An error occurred while making the API request: Network error");

        exception.And.InnerException.Should().BeOfType<HttpRequestException>()
            .Which.Message.Should().Be("Network error");
    }

    [Fact]
    public async Task LoginAsync_NullResponse_ReturnsFalse()
    {
        // Arrange
        _setup.SetupHttpResponse(HttpStatusCode.OK, null);

        // Act
        var result = await _setup.AuthService.LoginAsync(
            AuthServiceTestHelper.TestData.Credentials.ValidEmail,
            AuthServiceTestHelper.TestData.Credentials.ValidPassword);

        // Assert
        result.Should().BeTrue();
        _setup.AuthService.IsAuthenticated.Should().BeFalse();
    }

    [Fact]
    public async Task RegisterAsync_ValidCredentials_ReturnsTrue()
    {
        // Arrange
        var email = "newuser@example.com";
        var password = AuthServiceTestHelper.TestData.Credentials.ValidPassword;
        _setup.SetupHttpResponse(HttpStatusCode.OK);

        // Act
        var result = await _setup.AuthService.RegisterAsync(email, password);

        // Assert
        result.Should().BeTrue();
        _setup.VerifyHttpRequest("/api/AuthApi/register", HttpMethod.Post,
            content => content.Should().Contain($"\"Email\":\"{email}\""));
    }

    [Fact]
    public async Task RegisterAsync_ConflictResponse_ThrowsException()
    {
        // Arrange
        var errorMessage = "User already exists";
        _setup.SetupHttpResponse(HttpStatusCode.Conflict, errorMessage);

        // Act
        var act = async () => await _setup.AuthService.RegisterAsync("existing@example.com",
            AuthServiceTestHelper.TestData.Credentials.ValidPassword);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage($"An error occurred while making the API request: Registration failed: {errorMessage}");
    }


    [Fact]
    public async Task Logout_WhenAuthenticated_ClearsTokenAndAuthHeader()
    {
        // Arrange
        var loginResponse = AuthServiceTestHelper.TestData.CreateLoginResponse();
        _setup.SetupHttpResponse(HttpStatusCode.OK, loginResponse);
        await _setup.AuthService.LoginAsync(
            AuthServiceTestHelper.TestData.Credentials.ValidEmail,
            AuthServiceTestHelper.TestData.Credentials.ValidPassword);

        _setup.AuthService.IsAuthenticated.Should().BeTrue();
        _setup.HttpClient.DefaultRequestHeaders.Authorization.Should().NotBeNull();

        // Act
        _setup.AuthService.Logout();

        // Assert
        _setup.AuthService.IsAuthenticated.Should().BeFalse();
        _setup.HttpClient.DefaultRequestHeaders.Authorization.Should().BeNull();
    }

    [Fact]
    public void Logout_WhenNotAuthenticated_DoesNotThrow()
    {
        // Arrange
        _setup.AuthService.IsAuthenticated.Should().BeFalse();

        // Act
        var act = () => _setup.AuthService.Logout();

        // Assert
        act.Should().NotThrow();
        _setup.AuthService.IsAuthenticated.Should().BeFalse();
    }

}

