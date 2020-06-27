using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoGrpcService
{
    public class ToDoGenerator
    {
        private readonly ToDoDataContext ctx;
        public ToDoGenerator(ToDoDataContext dbContext)
        {
            ctx = dbContext;
        }

        public  void ToDoDataSeed()
        {

            if (ctx.ToDoDbItems.Any())
            {
                return;
            }

            ctx.ToDoDbItems.Add(new ToDoData()
            {
                Title="Wake up",Description="Wake up at 6 AM",Status=true
            });
            ctx.ToDoDbItems.Add(new ToDoData()
            {
                Title = "Catch a Train",
                Description = "Wake up at 8.30 AM",
                Status = true
            });
            ctx.ToDoDbItems.Add(new ToDoData()
            {
                Title = "Meeting",
                Description = "Meetng at 10 AM",
                Status = true
            });
            ctx.SaveChanges();
        }
    }
}
