using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace LoginReg
{
    public class Vistors
    {

        [Key]
        public int VistorsId { get; set; }

        public int? UsersId { get; set; }

        public User User { get; set; }

        public int WeddingId {get;set;}
        public Vistors()
        {
            User = new User();
        }
    }


}