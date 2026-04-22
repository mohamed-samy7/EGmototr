using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGYmotor.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required(ErrorMessage = "Place enter the message")]
        public string Message { get; set; }

        [ForeignKey("RegisterUser")]
        public int UserId { get; set; }

        public RegisterUser RegisterUser { get; set; }
    }
}
