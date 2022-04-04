using bagit_api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;


namespace bagit_api.Hubs;


// [Authorize]
public class ListHub : Hub
{
    private readonly ListController _listController;
    public ListHub()
    {
        _listController = new ListController();
    }
    
    public async Task AddItemToList(string itemName, string quantity)
    {
        _listController.AddItem(itemName, quantity);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetList()));
    }
    public async Task RemoveItemFromList(string name)
    {
        _listController.DeleteItem(name);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetList()));
    }
    
    public async Task GetList(string id) {
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetList()));
    }
    
}