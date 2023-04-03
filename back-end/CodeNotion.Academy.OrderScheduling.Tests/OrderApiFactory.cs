using System.Data.Common;
using CodeNotion.Academy.OrderScheduling.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Testcontainers.MsSql;

namespace CodeNotion.Academy.OrderScheduling.Tests;

public class OrderApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithCleanUp(true)
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    public HttpClient HttpClient { get; private set; } = default!;

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _dbConnection = new SqlConnection(_container.GetConnectionString());
        HttpClient = CreateClient();
        Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
        await InitializeRespawner();
    }

    public async Task ResetDatabaseAsync() => await _respawner.ResetAsync(_dbConnection);

    public new async Task DisposeAsync()
    {
        await _dbConnection.DisposeAsync();
        await _container.DisposeAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("IntegrationalTests");
        builder.ConfigureTestServices(services =>
        {
            // replace DatabaseContext
            services.Remove(services.Single(x => x.ServiceType == typeof(DbContextOptions<DatabaseContext>)));
            services.AddDbContext<DatabaseContext>(builder => builder.UseSqlServer(_container.GetConnectionString()));
        });
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer
        });
    }    
}