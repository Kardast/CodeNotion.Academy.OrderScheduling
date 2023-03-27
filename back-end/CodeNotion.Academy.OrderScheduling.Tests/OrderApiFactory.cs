using System.Data.Common;
using System.Net;
using System.Reflection;
using CodeNotion.Academy.OrderScheduling.Database;
using CodeNotion.Academy.OrderScheduling.Models;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Respawn;
using Testcontainers.MsSql;

namespace CodeNotion.Academy.OrderScheduling.Tests;

public class FakeLogger : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
    }
}

public class OrderApiFactory : WebApplicationFactory<DatabaseContext>, IAsyncLifetime
{
    // private readonly MsSqlContainer _container =
    //     new(new MsSqlConfiguration(Assembly.GetExecutingAssembly().GetName().Name, "sa", "yourStrong(!)Password"), new FakeLogger());
    //     // new TestcontainersBuilder<MsSqlContainer>()
    //     //     // .WithDatabase(new MsSqlConfiguration("sa", "Password123!", "OrderScheduling"))
    //     //     .WithImage("mcr.microsoft.com/mssql/server:2019-latest")
    //     //     .WithCleanUp(true)
    //     //     .Build();
    private readonly MsSqlContainer _container = new MsSqlBuilder()
        .WithPassword("yourStrong(!)Password")
        .WithCleanUp(true)
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    public HttpClient HttpClient { get; private set; } = default!;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // services.RemoveAll(typeof(DatabaseContext));
            // services.AddDbContext<DatabaseContext>(options => { options.UseSqlServer(_container.GetConnectionString()); });
            services.AddTransient<DatabaseContext>(sp => new DatabaseContext(new DbContextOptionsBuilder<DatabaseContext>()
                .UseSqlServer(_container.GetConnectionString())
                .Options));
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _dbConnection = new SqlConnection(_container.GetConnectionString());
        HttpClient = CreateClient();
        await InitializeRespawner();
    }

    private async Task InitializeRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = new[] { "public" }
        });
    }

    public new async Task DisposeAsync()
    {
        await _container.DisposeAsync();
    }
}