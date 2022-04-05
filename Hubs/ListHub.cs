using System.IdentityModel.Tokens.Jwt;
using bagit_api.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using bagit_api.Models;


namespace bagit_api.Hubs;


[Authorize]
public class ListHub : Hub
{
    private readonly ListController _listController;
    public ListHub()
    {

        _listController = new ListController();
    }
    
    public async Task AddItemToList(string product, int listId)
    {
        var deserializedProduct = System.Text.Json.JsonSerializer.Deserialize<Product>(product);

        _listController.AddItem(deserializedProduct, listId);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetList()));
    }
    public async Task RemoveItemFromList(string product, int listId)
    {
        var deserializedProduct = System.Text.Json.JsonSerializer.Deserialize<Product>(product);

        _listController.DeleteItem(deserializedProduct, listId);
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetList()));
    }

    public async Task GetUserLists() {
        Console.WriteLine("Getuser lists called");
        await Clients.All.SendAsync("ListsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetUserLists(UserIdFromToken())));
    }

    public async Task NewList(string ListName) {
        await _listController.CreateList(ListName, UserIdFromToken());
        await Clients.All.SendAsync("ListsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetUserLists(UserIdFromToken())));
    }
    
    public async Task GetList(string id) {
        await Clients.All.SendAsync("ItemsUpdated", System.Text.Json.JsonSerializer.Serialize(_listController.GetList()));
    }
    
    public int UserIdFromToken() {
        var token = Context.GetHttpContext()?.Request.Query["access_token"].ToString();
        var handler = new JwtSecurityTokenHandler();
        var deserialiedToken = handler.ReadJwtToken(token);
        var userId = deserialiedToken.Claims.First(claim => claim.Type == "id");

        return int.Parse(userId.Value);
    }

}