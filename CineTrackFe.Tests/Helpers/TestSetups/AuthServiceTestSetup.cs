using CineTrackFE.AppServises;
using CineTrackFE.Models;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;

namespace CineTrackFE.Tests.Helpers.TestSetups;


public class AuthServiceTestHelper : IDisposable
{
    public Mock<HttpMessageHandler> MockHttpMessageHandler { get; }
    public HttpClient HttpClient { get; }
    public Mock<IUserStore> MockUserStore { get; }
    public AuthService AuthService { get; }

    public AuthServiceTestHelper()
    {
        MockHttpMessageHandler = new Mock<HttpMessageHandler>();
        HttpClient = new HttpClient(MockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.example.com/")
        };

        MockUserStore = new Mock<IUserStore>(); 
        MockUserStore.SetupAllProperties();      

        AuthService = new AuthService(HttpClient, MockUserStore.Object); 
    }

    public void Dispose()
    {
        HttpClient?.Dispose();
    }


    public void SetupHttpResponse(HttpStatusCode statusCode, object? responseData = null)
    {
        HttpContent? content = null;

        if (responseData != null)
        {
            if (responseData is string stringContent)
            {
                content = new StringContent(stringContent, Encoding.UTF8, "text/plain");
            }
            else
            {
                var json = JsonSerializer.Serialize(responseData);
                content = new StringContent(json, Encoding.UTF8, "application/json");
            }
        }

        MockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = content ?? new StringContent("")
            });
    }



    public void SetupHttpException(Exception exception)
    {
        MockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(exception);
    }


    public void VerifyHttpRequest(string expectedEndpoint, HttpMethod expectedMethod, Action<string>? contentValidator = null)
    {
        MockHttpMessageHandler
            .Protected()
            .Verify<Task<HttpResponseMessage>>(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.RequestUri!.ToString().EndsWith(expectedEndpoint) &&
                    req.Method == expectedMethod &&
                    (contentValidator == null || ValidateContent(req, contentValidator))),
                ItExpr.IsAny<CancellationToken>());
    }


    public void VerifyHttpCallCount(int expectedCallCount)
    {
        MockHttpMessageHandler
            .Protected()
            .Verify<Task<HttpResponseMessage>>(
                "SendAsync",
                Times.Exactly(expectedCallCount),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
    }


    public static class TestData
    {
        public static User CreateTestUser(string id = "test-id", string email = "test@example.com")
        {
            return new User { Id = id, Email = email };
        }

        public static LoginResponse CreateLoginResponse(string token = "test-token", User? user = null)
        {
            return new LoginResponse
            {
                Token = token,
                User = user ?? CreateTestUser()
            };
        }

        public static class Credentials
        {
            public const string ValidEmail = "test@example.com";
            public const string ValidPassword = "password123";
            public const string InvalidEmail = "invalid@example.com";
            public const string InvalidPassword = "wrongpassword";
        }

        public static class Tokens
        {
            public const string ValidToken = "test-jwt-token";
            public const string ExpiredToken = "expired-token";
        }
    }

    private static bool ValidateContent(HttpRequestMessage request, Action<string> validator)
    {
        if (request.Content == null) return false;

        var content = request.Content.ReadAsStringAsync().Result;
        validator(content);
        return true;
    }
}