using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGYmotor.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Amount must be between 1 and 10,000.")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }  // e.g. "Visa", "Cash", "Wallet"

        public DateTime PaymentDate { get; set; } = DateTime.Now;
        [ForeignKey(nameof(UserId))]
        public RegisterUser RegisterUser { get; set; }
    }
}
