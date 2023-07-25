using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SharedKernel;
using System.Text;

namespace InprocessFunctionApp.UnitTests;

public class VonageAnswerTests
{
    [Fact]
    public async Task Test1Async()
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
        var function = new VonageAnswer(mockConfiguration.Object, mockLoggerFactory.Object);
        //var result = await function.Run(request);
    }
}