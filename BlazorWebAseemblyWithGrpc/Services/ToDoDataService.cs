using Grpc.Net.Client;
using System;
using System.Threading.Tasks;
using ToDoGrpcService;
namespace BlazorClient.Services
{
    public class ToDoDataService
    {
        ToDoService.ToDoServiceClient _toDoServiceClient;
        public ToDoDataService(ToDoService.ToDoServiceClient toDoServiceClient)
        {
            _toDoServiceClient = toDoServiceClient;
        }

        public async Task<bool> AddToDoData(Data.ToDoDataItem toDoDataItem)
        {
            var todoData = new ToDoGrpcService.ToDoData()
            {
                Status = toDoDataItem.Status,
                Title = toDoDataItem.Title,
                Description = toDoDataItem.Description
            };

            var response = await _toDoServiceClient.PostToDoItemAsync(todoData, null);
            return response.Status;

        }
        public async Task<bool> UpdateToDoData(Data.ToDoDataItem toDoDataItem)
        {
         
            var updateData = new ToDoGrpcService.ToDoPutQuery
            {
                Id = toDoDataItem.Id,
                ToDoDataItem = new ToDoGrpcService.ToDoData()
                {
                    Id = toDoDataItem.Id,
                    Status = toDoDataItem.Status,
                    Title = toDoDataItem.Title,
                    Description = toDoDataItem.Description
                }
            };
            var response = await _toDoServiceClient.PutToDoItemAsync(updateData, null);
            return response.Status;
        }
        public async Task<bool> DeleteDataAsync(string ToDoId)
        {
           
            var response = await _toDoServiceClient.DeleteItemAsync(new ToDoGrpcService.ToDoQuery() { Id = Convert.ToInt32(ToDoId) }, null);
            return response.Status;
        }
        public async Task<ToDoGrpcService.ToDoItems> GetToDoList()
        {
            
            return await _toDoServiceClient.GetToDoAsync(new Google.Protobuf.WellKnownTypes.Empty(), null);

        }

        public async Task<Data.ToDoDataItem> GetToDoItemAsync(int id)
        {
           
            var todoItem = await _toDoServiceClient.GetToDoItemAsync(new ToDoGrpcService.ToDoQuery() { Id = Convert.ToInt32(id) }, null);

            

            return new Data.ToDoDataItem() { Title = todoItem.Title, Description = todoItem.Description, Status = todoItem.Status, Id = todoItem.Id };

        }
    }
}