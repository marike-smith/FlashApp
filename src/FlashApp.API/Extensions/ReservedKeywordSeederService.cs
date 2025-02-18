using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;

namespace FlashApp.API.Extensions;

public class ReservedKeywordSeederService(IConfiguration configuration, ILogger<ReservedKeywordSeederService> logger) : IHostedService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<ReservedKeywordSeederService> _logger = logger;
    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reserved_keywords.txt");

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting keyword seeding service...");
        try
        {
            await SeedReservedKeywordsAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Seeding process was canceled.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during the Reserved Keyword seeding process.");
        }
        _logger.LogInformation("Keyword seeding service started.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Reserved Keyword Seeder Service stopping...");
        return Task.CompletedTask;
    }

    private async Task SeedReservedKeywordsAsync(CancellationToken cancellationToken)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            _logger.LogError("Connection string 'DefaultConnection' was not found in configuration.");
            throw new InvalidOperationException("Missing connection string.");
        }

        if (!File.Exists(_filePath))
        {
            _logger.LogError("Seed file not found at path: {FilePath}", _filePath);
            return;
        }

        var keywords = (await File.ReadAllLinesAsync(_filePath, cancellationToken))
            .Select(line => line.Replace(",", "").Trim())
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (!keywords.Any())
        {
            _logger.LogWarning("No valid keywords found in the file: {FilePath}", _filePath);
            return;
        }

        using var connection = new SqlConnection(connectionString);

        try
        {
            await connection.OpenAsync(cancellationToken);

            var dt = new DataTable();
            dt.Columns.Add("Keyword", typeof(string));
            foreach (var keyword in keywords)
            {
                dt.Rows.Add(keyword);
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Keywords", dt.AsTableValuedParameter("KeywordTableType"));

            await connection.ExecuteAsync("InsertReservedKeywordsBulk", parameters, commandType: CommandType.StoredProcedure);

            _logger.LogInformation("Keywords seeded successfully.");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Failed to insert reserved keywords.");
        }
    }
}