using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Asset_Automation.Models
{
    [Table("Asset_table")]
    public class Addasset
    {
        [Key]
        public int id { get; set; }
        [Required(ErrorMessage ="please Enter asset")]
        [DataType(DataType.Text,ErrorMessage ="please enter text")]
        public string asset { get; set; }
        [Required(ErrorMessage = "please Enter capacity")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="please enter valid number")]
        public int capacity { get; set; }
        public string state { get; set; }
    
        
    }
   
}