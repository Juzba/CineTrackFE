using CineTrackFE.AppServises;
using CineTrackFE.Models;
using FluentAssertions;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;


//namespace CineTrackFE.Tests.Unit.AppServices;


//public class ApiServiceTests : IDisposable
//{
//    private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
//    private readonly HttpClient _httpClient;
//    private readonly ApiService _apiService;

//    public ApiServiceTests()
//    {
//        _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
//        _httpClient = new HttpClient(_mockHttpMessageHandler.Object)
//        {
//            BaseAddress = new Uri("https://api.example.com/")
//        };
//        _apiService = new ApiService(_httpClient);
//    }

//    public void Dispose()
//    {
//        _httpClient?.Dispose();
//    }

//    [Fact]
//    public async Task GetAsync__SuccessResponse_ReturnsDeserializedObject()
//    {
//        // Arrange
//        var expectedFilm = new Film { Id = 1, Name = "Test Film", Director = "Test Director" };
//        var json = JsonSerializer.Serialize(expectedFilm);

//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent(json, Encoding.UTF8, "application/json")
//            });

//        // Act
//        var result = await _apiService.GetAsync<Film>("films/1");

//        // Assert
//        result.Should().NotBeNull();
//        result!.Id.Should().Be(1);
//        result.Name.Should().Be("Test Film");
//        result.Director.Should().Be("Test Director");
//    }

//    [Fact]
//    public async Task GetAsync__ErrorResponse_ThrowsHttpRequestException()
//    {
//        // Arrange
//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.NotFound
//            });

//        // Act
//        var act = async () => await _apiService.GetAsync<Film>("films/999");

//        // Assert
//        await act.Should().ThrowAsync<HttpRequestException>()
//            .WithMessage("*NotFound*");
//    }

//    [Fact]
//    public async Task GetAsync__NetworkException_ThrowsException()
//    {
//        // Arrange
//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ThrowsAsync(new HttpRequestException("Network error"));

//        // Act
//        var act = async () => await _apiService.GetAsync<Film>("films/1");

//        // Assert
//        await act.Should().ThrowAsync<Exception>()
//            .WithMessage("An error occurred while making the API request: Network error");
//    }

//    [Fact]
//    public async Task GetAsync__InvalidJson_ThrowsException()
//    {
//        // Arrange
//        var invalidJson = "{ invalid json }";

//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent(invalidJson, Encoding.UTF8, "application/json")
//            });

//        // Act
//        var act = async () => await _apiService.GetAsync<Film>("films/1");

//        // Assert
//        await act.Should().ThrowAsync<Exception>()
//            .WithMessage("An error occurred while making the API request:*");
//    }

//    [Fact]
//    public async Task GetAsync__EmptyResponse_ReturnsDefault()
//    {
//        // Arrange
//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent("null", Encoding.UTF8, "application/json")
//            });

//        // Act
//        var result = await _apiService.GetAsync<Film>("films/1");

//        // Assert
//        result.Should().BeNull();
//    }

//    [Fact]
//    public async Task GetAsync__CancellationRequested_ThrowsOperationCanceledException()
//    {
//        // Arrange
//        var cts = new CancellationTokenSource();
//        cts.Cancel();

//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ThrowsAsync(new OperationCanceledException());

//        // Act
//        var act = async () => await _apiService.GetAsync<Film>("films/1", cts.Token);

//        // Assert
//        var exception = await act.Should().ThrowAsync<Exception>()
//               .WithMessage("An error occurred while making the API request:*");

//        exception.And.InnerException.Should().BeOfType<HttpRequestException>()
//        .Which.Message.Should().Be("Network error");

//    }

//    [Fact]
//    public async Task GetAsync__CorrectEndpointCalled()
//    {
//        // Arrange
//        var expectedEndpoint = "films/123";
//        HttpRequestMessage? capturedRequest = null;

//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .Callback<HttpRequestMessage, CancellationToken>((request, token) =>
//            {
//                capturedRequest = request;
//            })
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent("{}", Encoding.UTF8, "application/json")
//            });

//        // Act
//        await _apiService.GetAsync<Film>(expectedEndpoint);

//        // Assert
//        capturedRequest.Should().NotBeNull();
//        capturedRequest!.RequestUri!.ToString().Should().EndWith(expectedEndpoint);
//        capturedRequest.Method.Should().Be(HttpMethod.Get);
//    }

//    [Theory]
//    [InlineData(HttpStatusCode.BadRequest)]
//    [InlineData(HttpStatusCode.Unauthorized)]
//    [InlineData(HttpStatusCode.Forbidden)]
//    [InlineData(HttpStatusCode.InternalServerError)]
//    public async Task GetAsync_VariousErrorStatusCodes_ThrowsHttpRequestException(HttpStatusCode statusCode)
//    {
//        // Arrange
//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = statusCode
//            });

//        // Act
//        var act = async () => await _apiService.GetAsync<Film>("films/1");

//        // Assert
//        await act.Should().ThrowAsync<HttpRequestException>()
//            .WithMessage($"*{statusCode}*");
//    }

//    [Theory]
//    [InlineData("films/1", typeof(Film))]
//    [InlineData("genres", typeof(List<Genre>))]
//    [InlineData("users/profile", typeof(User))]
//    public async Task GetAsync_DifferentEndpointsAndTypes_WorksCorrectly(string endpoint, Type expectedType)
//    {
//        // Arrange
//        var json = "{}"; // Prázdný objekt pro jednoduchý test

//        _mockHttpMessageHandler
//            .Protected()
//            .Setup<Task<HttpResponseMessage>>(
//                "SendAsync",
//                ItExpr.IsAny<HttpRequestMessage>(),
//                ItExpr.IsAny<CancellationToken>())
//            .ReturnsAsync(new HttpResponseMessage
//            {
//                StatusCode = HttpStatusCode.OK,
//                Content = new StringContent(json, Encoding.UTF8, "application/json")
//            });

//        // Act & Assert - použití reflection pro generické volání
//        var method = typeof(ApiService).GetMethod("GetAsync");
//        var genericMethod = method!.MakeGenericMethod(expectedType);
//        var task = (Task)genericMethod.Invoke(_apiService, new object[] { endpoint, CancellationToken.None })!;
//        await task;

//        // Ověření, že volání proběhlo bez chyby
//        task.IsCompletedSuccessfully.Should().BeTrue();
//    }
//}
