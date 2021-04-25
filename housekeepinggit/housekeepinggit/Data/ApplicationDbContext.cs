using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using housekeepinggit.Models;

namespace housekeepinggit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<housekeepinggit.Models.Location> Location { get; set; }
        public DbSet<housekeepinggit.Models.Task> Task { get; set; }
    }
}
