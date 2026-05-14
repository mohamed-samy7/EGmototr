using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGYmotor.Models
{
    public class RegisterUser
    {
        public int RegisterUserId { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "User name must be between 3 and 50 characters.")]
        [RegularExpression(@"^[\p{L} ]+$", ErrorMessage = "Invalid Name !")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [DataType(DataType.PhoneNumber)]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must contain only numbers.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Confirm password is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Registration Date")]
        public DateTime RegisterDate { get; set; } = DateTime.Now;

        public Request? Request { get; set; }
        public Feedback? Feedback { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}