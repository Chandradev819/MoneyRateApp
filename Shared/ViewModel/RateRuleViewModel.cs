namespace Shared.ViewModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class RateRuleViewModel
    {
        public int RateRuleId { get; set; }

        [Required(ErrorMessage = "User is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Currency Pair is required")]
        [StringLength(10, ErrorMessage = "Currency Pair cannot be longer than 10 characters")]
        public string CurrencyPair { get; set; } = string.Empty;

        [Required(ErrorMessage = "Target Rate is required")]
        [Range(0.0001, double.MaxValue, ErrorMessage = "Target Rate must be greater than zero")]
        public decimal TargetRate { get; set; }

        [Required(ErrorMessage = "Condition is required")]
        [RegularExpression("Above|Below", ErrorMessage = "Condition must be either 'Above' or 'Below'")]
        public string Condition { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

}
