using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Components;

namespace BlazorClient.CodeFiles
{
    public partial class ToDoOperation : ComponentBase
    {
        public bool ShowModel = false;
        public bool ShowAlert = false;
        public bool ShowModeletePopup = false;
        public string OperationStatusText = "";
        public string PopupTitle = "";
        public int noOfRows = 0;
        public int noOfCoumns = 4;
       

        public BlazorClient.Data.ToDoDataItem ToDoDataItem = new BlazorClient.Data.ToDoDataItem();
        public string ActionText = "";

        public ToDoGrpcService.ToDoItems toDoItems = new ToDoGrpcService.ToDoItems();

        public string DeleteItemId { get; set; }

        [Inject]
        protected BlazorClient.Services.ToDoDataService ToDoService { get; set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                await GetToDoListAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        protected async Task GetToDoListAsync()
        {

            toDoItems = await ToDoService.GetToDoList();
            var temoRowCount = toDoItems.ToDoItemList.Count() / noOfCoumns;
            noOfRows = toDoItems.ToDoItemList.Count() % noOfCoumns == 0 ? temoRowCount : temoRowCount + 1;
           
        }

        protected async Task ShowEditForm(int Id)
        {
            Console.Write(Id);
            PopupTitle = "To Do Edit";
            ActionText = "Update";
            ToDoDataItem = await ToDoService.GetToDoItemAsync(Id);
            ShowModel = true;
        }

        protected void ShowAddpopup()
        {
            ToDoDataItem = new Data.ToDoDataItem() { Title = "", Description = "", Status = false, Id = 0 };
            PopupTitle = "To Do Add";
            ActionText = "Add";
            ShowModel = true;

        }
        protected void ShowDeletePopup(string Id)
        {
            Console.Write(Id);
            DeleteItemId = Id;
            ShowModeletePopup = true;
        }

        protected async Task PostDataAsync()
        {
            try
            {
                bool status = false;
                if (ToDoDataItem.Id > 0)
                {
                    status = await ToDoService.UpdateToDoData(this.ToDoDataItem);

                }
                else
                {
                    status = await ToDoService.AddToDoData(this.ToDoDataItem);
                }
                await Reload(status);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

        }

        public async Task DeleteDataAsync()
        {

            var operationStatus = await ToDoService.DeleteDataAsync(DeleteItemId);
            await Reload(operationStatus);
        }

        protected async Task Reload(bool status)
        {
            ShowModeletePopup = false;
            ShowModel = false;
            await GetToDoListAsync();
            ShowAlert = true;
            if (status)
            {
                OperationStatusText = "1";
            }
            else
            {
                OperationStatusText = "0";
            }

        }

        protected  void DismissPopup()
        {
            ShowModel = false;
            ShowAlert = false;
            ShowModeletePopup = false;
          
        }

    }
}