using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGYmotor.Models
{
    public class Request
    {
        [Key]
        public int RequestId { get; set; }

        [Required(ErrorMessage = "place enter the problem")]
        public string Problem { get; set; }

        [Required(ErrorMessage = "enter your locatoin")]
        public string Location { get; set; }

        public string? ImagePath { get; set; }

        [ForeignKey("RegisterUser")]
        public int UserId { get; set; }

        public RegisterUser? RegisterUser { get; set; }
    }
}