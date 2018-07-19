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
        
        public int? VistsId {get;set;}

        public List<Vistors> Vists {get;set;}


        public Wedding()
        {
            Vists = new List<Vistors>();
        }
               
    }
    




}