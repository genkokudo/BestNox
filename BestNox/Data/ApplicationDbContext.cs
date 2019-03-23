using System;
using System.Collections.Generic;
using System.Text;
using BestNox.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BestNox.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<MyTest> MyTests { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
