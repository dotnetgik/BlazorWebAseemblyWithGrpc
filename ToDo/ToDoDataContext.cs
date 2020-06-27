using Microsoft.EntityFrameworkCore;

namespace ToDoGrpcService
{
    public class ToDoDataContext:DbContext
    {
        

        public ToDoDataContext(DbContextOptions<ToDoDataContext> options):base(options)
        {

        }

       
        public DbSet<ToDoData> ToDoDbItems { get; set; }
    }
}