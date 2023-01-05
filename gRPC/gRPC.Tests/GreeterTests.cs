using FluentAssertions;
using gRPC.Server;

namespace gRPC.Tests;

public class GreeterTests : IntegrationTestFixture<Greeter.GreeterClient>
{
    [TestCaseSource(nameof(HelloTestInputs))]
    public async Task SayHello_ShouldSucceed(HelloTestInput input)
    {
        var result = await Client.SayHelloAsync(input.Request);

        result.Should().Be(input.Reply);
    }

    public readonly record struct HelloTestInput(HelloRequest Request, HelloReply Reply);

    public static readonly IEnumerable<HelloTestInput> HelloTestInputs = new HelloTestInput[]
    {
        new (
            new HelloRequest()
            {
                First = "Chris"
            },
            new HelloReply()
            {
                Message = "Hello Chris"
            }
        ),
        new (
            new HelloRequest()
            {
                First = "Chris",
                Last = "Garrett"
            },
            new HelloReply()
            {
                Message = "Hello Chris Garrett"
            }
        ),        
        new (
            new HelloRequest()
            {
                First = "Chris",
                Last = "Garrett",
                Age = 42
            },
            new HelloReply()
            {
                Message = "Hello Chris Garrett and you're 42 years old"
            }
        ),          
    };

}