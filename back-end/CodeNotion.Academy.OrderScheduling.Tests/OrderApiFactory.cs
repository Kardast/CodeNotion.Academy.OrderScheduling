using System.Data.Common;
using System.Net;
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
using Respawn;

namespace CodeNotion.Academy.OrderScheduling.Tests;

public class OrderApiFactory : WebApplicationFactory<DatabaseContext>, IAsyncLifetime
{
    private readonly TestcontainerDatabase _container =
        new TestcontainersBuilder<MsSqlTestcontainer>()
            .WithDatabase(new MsSqlTestcontainerConfiguration
            {
                Password = "localdevpassword#123",
            })
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithCleanUp(true)
            .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;
    public HttpClient HttpClient { get; private set; } = default!;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DatabaseContext));
            services.AddDbContext<DatabaseContext>(options => { options.UseSqlServer(_container.ConnectionString); });
            services.AddTransient<Order>();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        _dbConnection = new SqlConnection(_container.ConnectionString);
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