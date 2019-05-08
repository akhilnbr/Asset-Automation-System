using Asset_Automation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Asset_Automation.ViewModel
{
    public class BookinView
    {

        public BookinView()
        {
            book = new List<BookModel>();
            confirm = new List<BookModel>();
        }
 
       [Required(ErrorMessage ="please select asset")]
        public string asset { get; set; }
        public string date { get; set; }
        [Required(ErrorMessage ="please enter members")]
        public int members { get; set; }
        public TimeSpan intime { get; set; }
        public TimeSpan outtime { get; set; }

       public IList<BookModel> book { get; set; }
        public IList<BookModel> confirm { get; set; }
    }
    
   
  


}