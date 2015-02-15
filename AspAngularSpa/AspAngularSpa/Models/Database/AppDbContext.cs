using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AspAngularSpa.Models.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("AppDbContext")
        {

        }
        public DbSet<User> Users { get; set; }
    }
}