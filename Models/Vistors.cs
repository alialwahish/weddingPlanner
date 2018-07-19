using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;


namespace LoginReg
{
    public class Vistors
    {

        [Key]
        public int VistsId { get; set; }

        public int UserId { get; set; }

        public User Users { get; set; }

        public int WeddingId {get;set;}

        public Wedding Wedding {get;set;}
       
    }


}