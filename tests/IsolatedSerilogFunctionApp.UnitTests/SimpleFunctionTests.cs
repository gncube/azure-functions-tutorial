using FluentAssertions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel;
using System.Text;

namespace IsolatedSerilogFunctionApp.UnitTests;

public class SimpleFunctionTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var mockConfiguration = new Mock<IConfiguration>();
        var body = new MemoryStream(Encoding.UTF8.GetBytes("{ \"test\": true }"));
        var context = new Mock<FunctionContext>();
        var request = new MockHttpRequestData(
                        context.Object,
                        new Uri("http://localhost"),
                        body);

        // Act
        var function = new SimpleFunction(mockLoggerFactory.Object);
        var result = function.Run(request);
        result.Body.Position = 0;

        // Assert
        var reader = new StreamReader(result.Body);
        var responseBody = reader.ReadToEnd();
        result.Should().NotBe(null);
        responseBody.Should().Be("Hello test");
    }
}