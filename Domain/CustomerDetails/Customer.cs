using System.ComponentModel.DataAnnotations;

namespace Domain.CustomerDetails
{
    public class Customer
    {
        [Key]
        public Guid CustomerID { get; set; } 
        [Required]
        [MaxLength(20, ErrorMessage = "Maximum Accepts 20 letters only ")]
        [RegularExpression(@"^[a-zA-Z\s]+$",ErrorMessage = "CustomerName accepts charcters only .. ! ")]
        public string CustomerName { get; set; }
        public string? Phone { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Accept Number only while typing (Maximum 10)")]
        [MinLength(10)]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Accept the @ value Only ")]
        public string EmailAddress { get; set; }

        public string? Address { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2 to 50 characters")]
        public string? City { get; set; }
        public string? District { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Must be between 2 to 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string State { get; set; }

        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Only have 6 numbers")]
        public string Pincode { get; set; }
    }
}
