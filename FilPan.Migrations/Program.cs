using Microsoft.EntityFrameworkCore;
using System;

namespace FilPan.Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var dbContext = new FilPanDbContextFactory().CreateDbContext(args))
            {
                dbContext.Database.Migrate();
            }
            Console.WriteLine("Done");
        }
    }
}
