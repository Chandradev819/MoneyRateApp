using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IDashboardService
    {
        /// <summary>
        /// Returns headline metrics for the dashboard: counts, recent trends, etc.
        /// </summary>
        Task<DashboardSummaryDto> GetSummaryAsync();

        /// <summary>
        /// Returns chart‑ready data for recent activity (e.g., trades, alerts, conversions).
        /// </summary>
        Task<IEnumerable<ActivityChartPoint>> GetRecentActivityAsync(int days);

        /// <summary>
        /// Returns personalized recommendations or alerts for the logged‑in user.
        /// </summary>
        Task<IEnumerable<DashboardAlertDto>> GetActiveAlertsAsync();
    }

    public class DashboardService : IDashboardService
    {
        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            // Placeholder: later hook into SmartForexDBContext
            await Task.Delay(20);
            return new DashboardSummaryDto
            {
                TotalUsers = 1250,
                ActiveToday = 342,
                SignalsTriggered = 58,
                LastUpdatedUtc = DateTime.UtcNow
            };
        }

        public async Task<IEnumerable<ActivityChartPoint>> GetRecentActivityAsync(int days)
        {
            await Task.Delay(20);
            return Enumerable.Range(0, days)
                .Select(i => new ActivityChartPoint
                {
                    Date = DateTime.UtcNow.Date.AddDays(-i),
                    Value = Random.Shared.Next(10, 100)
                });
        }

        public async Task<IEnumerable<DashboardAlertDto>> GetActiveAlertsAsync()
        {
            await Task.Delay(20);
            return new[]
            {
                new DashboardAlertDto { Message = "EUR/USD volatility spike detected", Severity = "High" },
                new DashboardAlertDto { Message = "New optimization rule ready for review", Severity = "Medium" }
            };
        }
    }

    // Simple DTOs for shaping dashboard payloads
    public record DashboardSummaryDto
    {
        public int TotalUsers { get; init; }
        public int ActiveToday { get; init; }
        public int SignalsTriggered { get; init; }
        public DateTime LastUpdatedUtc { get; init; }
    }

    public record ActivityChartPoint
    {
        public DateTime Date { get; init; }
        public int Value { get; init; }
    }

    public record DashboardAlertDto
    {
        public string Message { get; init; }
        public string Severity { get; init; }
    }
}

