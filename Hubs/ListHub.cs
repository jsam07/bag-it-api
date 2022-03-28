using Microsoft.AspNetCore.SignalR;
using bagit_api.Models;

namespace bagit_api.Hubs;

public class ListHub : Hub
{
    public async Task AddItemToList(string itemName, string quantity)
    {
        TestList.List.AddItem(itemName, quantity);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(TestList.List.GetList()));
    }
    public async Task RemoveItemFromList(string name)
    {
        TestList.List.DeleteItem(name);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(TestList.List.GetList()));
    }

    public async Task GetList(string id) {
        Console.WriteLine("getlist");
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(TestList.List.GetList()));
    }
}