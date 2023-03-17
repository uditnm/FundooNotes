using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.FundoContext
{
    public class FundoAppContext:DbContext
    {
        public FundoAppContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NotesEntity> Notes { get; set; }
        public DbSet<CollaboratorEntity> Collaborators { get; set; }
    }
}
