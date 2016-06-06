﻿using Microsoft.EntityFrameworkCore;
using OpenIddict;

namespace openiddicttest.Models
{
    public class ApplicationDbContext : OpenIddictContext<ApplicationUser, ApplicationRole>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
