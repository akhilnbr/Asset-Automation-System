using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Asset_Automation.Models
{
    [Table("reg_tb")]
    public class register
    {
        public int id { get; set; }
        [Required(ErrorMessage ="Name field is required")]
       // [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid name")]
        [DataType(DataType.Text,ErrorMessage ="only alphabet is allowed")]
        public string name { get; set; }
        //[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Enter a valid name")]
        [Required(ErrorMessage ="Last name is required")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Phone field is required")]
        [DataType(DataType.PhoneNumber, ErrorMessage="enter phone number")]
        [RegularExpression(@"^\S*([0]|\+91[\-\s]?)?[789]\d{9}$", ErrorMessage = "Entered Mobile No is not valid.")]
        public string phone { get; set; }
        [Required(ErrorMessage ="please select department")]
        public string department { get; set; }
       [EmailAddress(ErrorMessage ="Enter valid email address")]
        [Required(ErrorMessage ="enter email adddress")]
        public string email { get; set; }
        [Required(ErrorMessage = "passsword field is required")]
        [Range(1000,9999,ErrorMessage ="please select a pin of length 4")]
        public int pin { get; set; }
        [NotMapped]
        public int confirmpin { get; set; }
       [NotMapped]
        public string check { get; set; }
    }
}