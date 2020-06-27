using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace ToDoGrpcService.Services
{
    public class ToDoDataService : ToDoService.ToDoServiceBase
    {
        private readonly ToDoDataContext _dataContext;
        public ToDoDataService(ToDoDataContext dataContext)
        {
            
            _dataContext = dataContext;
           
        }

        /// <summary>
        /// Get All Data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ToDoItems> GetToDo(Empty request, ServerCallContext context)
        {
            ToDoItems objItems = new ToDoItems();

            foreach (var item in _dataContext.ToDoDbItems)
            {
                objItems.ToDoItemList.Add(item);
            }

            return Task.FromResult(objItems);
        }

        /// <summary>
        /// Post Data 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ToDoPostResponse> PostToDoItem(ToDoData request, ServerCallContext context)
        {
             _dataContext.ToDoDbItems.Add(request);
            var result = _dataContext.SaveChanges();
            if (result>0)
            {
                return Task.FromResult(new ToDoPostResponse()
                {
                    Status = true,
                    StatusCode = 100,
                    StatusMessage = "Added Successfully"
                });
            }
            else
            {
                return Task.FromResult(new ToDoPostResponse()
                {
                    Status = false,
                    StatusCode = 500,
                    StatusMessage = "Issue Occured."
                });
            }
            
        }
        /// <summary>
        /// Get Item with the Id
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>

        public override Task<ToDoData> GetToDoItem(ToDoQuery request, ServerCallContext context)
        {
            var result = from data in _dataContext.ToDoDbItems
                         where data.Id == request.Id
                         select data;
            return Task.FromResult(result.First());
            
        }
        /// <summary>
        /// Deletes the Item
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ToDoPostResponse> DeleteItem(ToDoQuery request, ServerCallContext context)
        {

            var item = (from data in _dataContext.ToDoDbItems
                                    where data.Id == request.Id
                                    select data).Single();


              _dataContext.ToDoDbItems.Remove(item);

            var result = _dataContext.SaveChanges();

            if (result > 0)
            {
                return Task.FromResult(new ToDoPostResponse()
                {
                    Status = true,
                    StatusCode = 100,
                    StatusMessage = "Deleted Successfully"
                });
            }
            else
            {
                return Task.FromResult(new ToDoPostResponse()
                {
                    Status = false,
                    StatusCode = 500,
                    StatusMessage = "Issue Occured."
                });
            }
        }
        /// <summary>
        /// Updates the item
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<ToDoPostResponse> PutToDoItem(ToDoPutQuery request, ServerCallContext context)
        {
            _dataContext.ToDoDbItems.Update(request.ToDoDataItem);
            var result = _dataContext.SaveChanges();


            if (result > 0)
            {
                return Task.FromResult(new ToDoPostResponse()
                {
                    Status = true,
                    StatusCode = 100,
                    StatusMessage = "Updated  Successfully "
                });
            }
            else
            {
                return Task.FromResult(new ToDoPostResponse()
                {
                    Status = false,
                    StatusCode = 500,
                    StatusMessage = "Issue Occured."
                });
            }
        }
    }
}
