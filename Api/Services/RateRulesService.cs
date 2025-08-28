using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public interface IRateRulesService
    {
        Task<List<RateRule>> GetUserRateRulesAsync(int userId);
        Task<RateRule?> GetByIdAsync(int rateRuleId);
        Task AddAsync(RateRule rule);
        Task UpdateAsync(RateRule rule);
        Task DeleteAsync(int rateRuleId);
    }

    public class RateRulesService : IRateRulesService
    {
        private readonly SmartForexDBContext _context;

        public RateRulesService(SmartForexDBContext context)
        {
            _context = context;
        }

        public async Task<List<RateRule>> GetUserRateRulesAsync(int userId)
        {
            return await _context.RateRules
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<RateRule?> GetByIdAsync(int rateRuleId)
        {
            return await _context.RateRules
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.RateRuleId == rateRuleId);
        }

        public async Task AddAsync(RateRule rule)
        {
            rule.CreatedAt = DateTime.UtcNow;
            _context.RateRules.Add(rule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RateRule rule)
        {
            _context.RateRules.Update(rule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int rateRuleId)
        {
            var rule = await _context.RateRules.FindAsync(rateRuleId);
            if (rule is not null)
            {
                _context.RateRules.Remove(rule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
