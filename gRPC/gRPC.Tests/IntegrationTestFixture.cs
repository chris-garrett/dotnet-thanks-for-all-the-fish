using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace gRPC.Tests;

public interface IGrpcMessageHandlerFactory
{
    HttpMessageHandler CreateMessageHandler();
}

[TestFixture]
public class IntegrationTestFixture<TClient> : IGrpcMessageHandlerFactory
    where TClient : ClientBase<TClient>
{
    private readonly WebApplicationFactory<Program> _factory;

    protected IServiceProvider Services { get; }

    public HttpMessageHandler CreateMessageHandler() =>
        _factory.Server.CreateHandler();

    public TClient Client => Services.GetRequiredService<TClient>();

    protected IntegrationTestFixture()
    {
        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(b => b
                .ConfigureServices((c, s) =>
                {
                    s.AddSingleton<IGrpcMessageHandlerFactory>(_ => this);
                    s.AddGrpcClient<TClient>(
                        options =>
                        {
                            options.Address = new Uri("http://localhost:5000");
                        })
                    .ConfigurePrimaryHttpMessageHandler(x => x.GetRequiredService<IGrpcMessageHandlerFactory>().CreateMessageHandler());

                    ConfigureServices(s, c.Configuration);
                })
            );
        Services = _factory.Services;
    }

    protected virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _factory.Dispose();
    }
}