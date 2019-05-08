using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Asset_Automation.Views.ViewModel
{
    public class reset
    {
        [DataType(DataType.Password)]
        public int pin { get; set; }
        [DataType(DataType.Password)]
        [NotMapped]
        public int confirmpin { get; set; }
    }
}