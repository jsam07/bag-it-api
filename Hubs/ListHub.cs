using Microsoft.AspNetCore.SignalR;
using bagit_api.Models;


namespace bagit_api.Hubs;


public class ListHub : Hub
{
    private readonly BagIt _bagit;
    public ListHub()
    {
        _bagit = new BagIt();
    }
    
    public async Task AddItemToList(string itemName, string quantity)
    {
        _bagit.AddItem(itemName, quantity);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_bagit.GetList()));
    }
    public async Task RemoveItemFromList(string name)
    {
        _bagit.DeleteItem(name);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_bagit.GetList()));
    }
    
    public async Task GetList(string id) {
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_bagit.GetList()));
    }
    
}