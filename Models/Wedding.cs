using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;



namespace LoginReg
{

    public class Wedding 
    {
        [Key]
        [Required]
        public int WeddingId {get;set;}


        [Required]
        public string Wedding_Names {get;set;}


        [Required]
        
        public DateTime Date {get;set;}


        [Required]
        public int Guest {get;set;}

        [Required]
        public int Stat {get;set;}
        
        [Required]
        public string Wedding_Address {get;set;}

        public int UserId {get;set;}
        
        public int? VistorsId {get;set;}

        public List<Vistors> Vistors {get;set;}

        public Wedding()
        {
            Vistors = new List<Vistors>();
        }
               
    }
    




}