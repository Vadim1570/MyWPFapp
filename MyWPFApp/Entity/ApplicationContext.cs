using System;
using System.Data.Entity;
using System.IO;

namespace MyWPFapp
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext() : base("DefaultConnection")
        {
            this.Database.Log = EFLogger.WriteToTxtFile;
        }

        public void WriteLogToFile()
        {

        }

        public DbSet<Snapshot> Snapshots { get; set; }
        public DbSet<Winproc> Winprocs { get; set; }
    }

    public static class EFLogger
    {
        public static void WriteToTxtFile(string value)
        {
            using (StreamWriter writer = new StreamWriter("sqlite.log", append: true))
            {
                writer.WriteLine(value);
            }
        }
    }
}