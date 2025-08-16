using Microsoft.EntityFrameworkCore;
using ResWeb.Data;

namespace ResWeb.Services
{
    public class Schedules
    {
        private readonly ILogger<Schedules> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Schedules(ILogger<Schedules> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        }

        public async Task<List<Models.Schedule>> GetAllSchedulesAsync(CancellationToken cancellationToken = default)
        {
            var schedules = new List<Models.Schedule>();
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                schedules = await dbContext.Schedules
                    .Include(s => s.DateTimes)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error retrieving schedules: {ex.Message}", ex);
            }

            return schedules;
        }

        public async Task UpdateSchedule(Models.Schedule schedule, CancellationToken cancellationToken = default)
        {
            if (schedule == null) throw new ArgumentNullException(nameof(schedule));
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Schedules.Update(schedule);
                await dbContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error updating schedule: {ex.Message}", ex);
            }
        }

        public async Task DeleteSchedule(Guid scheduleId, CancellationToken cancellationToken = default)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var schedule = await dbContext.Schedules.Include(s=>s.DateTimes).FirstOrDefaultAsync().ConfigureAwait(false);
            if (schedule != null)
            {
                await DeleteSchedule(schedule, dbContext, cancellationToken).ConfigureAwait(false);
            }
        }

        public async Task DeleteSchedule(Models.Schedule schedule, ApplicationDbContext? dbContext = null, CancellationToken cancellationToken = default)
        {
            if (dbContext == null)
            {
                using var scope = _scopeFactory.CreateScope();
                dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            }

            dbContext.Schedules.Remove(schedule);
            await dbContext.SaveChangesAsync();
        }
    }
}
