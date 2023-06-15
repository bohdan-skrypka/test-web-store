using Domain.Entities;
using System.Data.Entity;

namespace Domain.App_Data
{
    public class EFDataDb:DbContext
    {
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}