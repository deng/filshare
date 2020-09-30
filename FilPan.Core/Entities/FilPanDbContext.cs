using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilPan.Entities
{
    public class FilPanDbContext : DbContext
    {
        public FilPanDbContext(DbContextOptions<FilPanDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<UploadLog> UploadLogs { get; set; }

        public DbSet<FileName> FileNames { get; set; }

        public DbSet<FileCid> FileCids { get; set; }

        public DbSet<FileMd5> FileMd5s { get; set; }
    }
}
