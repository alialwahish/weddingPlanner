using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace LoginReg
{

    public class User
    {


        [Key]
        [Required]
        public int UserId { get; set; }


        [Required(ErrorMessage = "Field Required")]
        [RegularExpression("^[a-zA-Z ]*$"
    , ErrorMessage = "Letters Only!")]
        [MinLength(2)]

        public string First_Name { get; set; }

        [Required(ErrorMessage = "Field Required")]
        [RegularExpression("^[a-zA-Z ]*$"
    , ErrorMessage = "Letters Only!")]
        [MinLength(2)]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Field Required")]
        [EmailAddress]

        public string Email { get; set; }

        [Required(ErrorMessage = "Field Required")]
        [MinLength(8)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password Don't match")]
        public string Confirm_Password { get; set; }


        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }

        public int? WeddingsId { get; set; }

        
        public int? VistorsId {get;set;}
        
        
        public List<Wedding> UserWeddings { get; set; }

        public User (){
            UserWeddings = new List<Wedding>();
        }

    }

}