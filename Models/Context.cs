using Microsoft.EntityFrameworkCore;

namespace LoginReg.Models
{



    public class MyContext : DbContext
    {


        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<User> users { get; set; }
        public DbSet<Wedding> weddings { set; get; }

        public DbSet<Vistors> Vistors {get;set;}




    }


}
