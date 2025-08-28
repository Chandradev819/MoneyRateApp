using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.ViewModel
{
    public class RateRuleVM
    {
        public int RateRuleId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "Currency pair is required")]
        [RegularExpression(@"^[A-Z]{3}/[A-Z]{3}$", ErrorMessage = "Format must be like EUR/USD")]
        public string CurrencyPair { get; set; } = string.Empty;

        [Required(ErrorMessage = "Target rate is required")]
        [Range(0.0001, 10000, ErrorMessage = "Target rate must be positive")]
        public decimal TargetRate { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        public RateCondition Condition { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public string Status { get; set; } = "Active";
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RateCondition
    {
        Above,
        Below,
        Equal
    }
}
