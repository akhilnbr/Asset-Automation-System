using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Asset_Automation.Models
{

    [Table("capacity")]
    public class capacityy
    {
       
              [Key]
            public int id { get; set; }
            public string capacity { get; set; }
            public int assetid { get; set; }
        
    }
}