using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Asset_Automation.Models
{
    [Table("book_table")]
    public class BookModel
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        [Required(ErrorMessage ="please select department")]
        public string department { get; set; }
        [Required(ErrorMessage ="please select asset")]
        public string asset { get; set; }
        public string date { get; set; }
        [Required(ErrorMessage ="please select members")]
        public int members { get; set; }
        public TimeSpan intime { get; set;}
        public TimeSpan outtime { get; set;}
        public string status { get; set; }

     


    }
}