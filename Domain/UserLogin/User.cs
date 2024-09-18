    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Metrics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using static System.Runtime.InteropServices.JavaScript.JSType;

    namespace Domain.UserLogin
    {
        public class User
        {
            [Key]
            public Guid UserID { get; set; } = Guid.NewGuid();
            [Required]
            [MaxLength(20, ErrorMessage = "Maximum Accepts 20 letters only ")]
            [RegularExpression(@"^[a-zA-Z\s]+$")]
            public string Username {get; set;}
        
            [EmailAddress]
            public string? Email { get; set; }
            [Required]
            [MinLength(8,ErrorMessage ="Password must be at least 8 characters.")]
            [MaxLength(20,ErrorMessage ="Password Cant be longer than 20 characters .")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character")]
            public string Password { get; set; }

        
        }
    }
