using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MinimumJumps
{
    public class MinimumJumpsContext : DbContext
    {
        public DbSet<MinimumJumpHistory> MinimumJumpHistories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=minimumjumpsdb.sqlite");
    }

    public class MinimumJumpHistory
    {
        [Key]
        public long Id { get; set; }
        public string entries { get; set; } // list of entries stored as string for simplicity
        public int shortestPath { get; set; }
    }
}