using Asset_Automation.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Asset_Automation
{
    public class DbModel:DbContext
    {
        
        public DbModel():base("DefaultConnection")
        {

        }
        public DbSet<Addasset> addasset { get; set; }
        public DbSet<register> regmodel { get; set; }
        public DbSet<BookModel> bookm { get; set; }
       
    }
}