using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ResWeb.Data;
using ResWeb.Models;
using System.Data;

namespace ResWeb.Services
{
    public class Configuration
    {
        private readonly ILogger<Configuration> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Configuration(ILogger<Configuration> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public TimeSpan ReminderTime { get; set; }

        /// <summary>
        /// Reads from Configuration datatable and attempts to fill properties of the service.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LoadAsync(CancellationToken cancellationToken = default)
        {
            // dbContext scope cannot be used within a singleton service. Instead declare new scope for use during loads only.
            var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var results = await dbContext.Configuration.FromSqlRaw<Models.Configuration>("EXEC [uspGetAllConfiguration]").ToListAsync(cancellationToken);
            var properties = typeof(Configuration).GetProperties();

            // Loop through all properties. Attempt to map configuration values to properties by name.
            foreach (var property in properties)
            {
                var config = results.FirstOrDefault(c => c.Name.Equals(property.Name, StringComparison.OrdinalIgnoreCase));
                if (config != null)
                {
                    try
                    {
                        if (property.PropertyType == typeof(TimeSpan)) // Cannot convert TimeSpan with generic methods, use TimeSpan.Parse.
                        {
                            var value = TimeSpan.Parse(config.Value);
                            property.SetValue(this, value);
                        }
                        else // Generic casting -- all other types.
                        {
                            var value = Convert.ChangeType(config.Value, property.PropertyType);
                            property.SetValue(this, value);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle conversion errors or log them as needed.
                        _logger.LogWarning($"Error setting property {property.Name}: {ex.Message}");
                    }
                }
                else 
                {
                    // The property was not loaded, fail silently but log result.
                    _logger.LogWarning($"Error setting property {property.Name}: No configuration found for this property.");
                }
            }
        }
    }
}
