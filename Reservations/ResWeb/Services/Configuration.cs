using Microsoft.EntityFrameworkCore;
using ResWeb.Data;

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

        private Dictionary<string, Models.Configuration> _configItems = new Dictionary<string, Models.Configuration>();

        public TimeSpan ReminderTime
        {
            get
            {
                var configItem = _configItems.FirstOrDefault(c => c.Key == "ReminderTime").Value;
                if (TimeSpan.TryParse(configItem.Value ?? "", out var timeSpan))
                {
                    return timeSpan;
                }

                return new TimeSpan(0, 15, 0); // Default return, if configItem is misconfigured.
            }
            set
            {
                var configItem = _configItems.FirstOrDefault(c => c.Key == "ReminderTime");
                if (configItem.Value != null)
                {
                    configItem.Value.Value = value.ToString();
                }
            }
        }

        /// <summary>
        /// Reads from Configuration datatable and attempts to fill properties of the service.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task LoadAsync(CancellationToken cancellationToken = default)
        {
            // Clear the dictionary before using it.
            _configItems = new Dictionary<string, Models.Configuration>();

            // dbContext scope cannot be used within a singleton service. Instead declare new scope for use during loads only.
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var results = await dbContext.Configuration.FromSqlRaw<Models.Configuration>("EXEC [uspGetAllConfiguration]").ToListAsync(cancellationToken);
                foreach (var configItem in results)
                {
                    _configItems.Add(configItem.Name, configItem);
                }
            }
        }

        /// <summary>
        /// Writes current configuration changes to the database.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            // Declare dbContext scope.
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                // All properties write back to the _configItems dictionary. Use this to update the database.
                // NOTE: Using properties directly will not work as they are not tracked by EF Core.
                var configItems = _configItems.Values.ToList();
                dbContext.Configuration.UpdateRange(configItems);

                await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error saving configuration:{ex.Message}", ex);
            }
        }
    }
}
