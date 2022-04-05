using bagit_api.Data;
using bagit_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace bagit_api.Controllers;

public class ListController : Controller
{
   
    private readonly BagItDbContext _context;

    public ListController()
    {
        var optionsBuilder = new DbContextOptionsBuilder<BagItDbContext>();
        optionsBuilder.UseSqlite("DataSource=wwwroot/app.db;Cache=Shared");

        _context = new BagItDbContext(optionsBuilder.Options);
    }
    
    public void AddItem(string itemName, string quantity)
    {
        // TODO: Let caller pass these parameters in
        int listId = 1;
        int userId = 1;
        
        var product = new Product
        {
            Name = itemName,
            Quantity = int.Parse(quantity)
        };

        var slProduct = new ShoppingListProduct
        {
            ListId = listId,
            Product = product
        };

        _context.Add(slProduct);
        _context.SaveChanges();
        Console.WriteLine($"\nAdded Product: {itemName} x{quantity}");
    }
    public void DeleteItem(string name)
    {
        // TODO: Let caller pass in listID and product ID to delete
        _context.Products.RemoveRange(_context.Products.Where(
            p => p.Name == name
        ));

        _context.SaveChanges();
        Console.WriteLine($"\nDeleted Product: {name}");
    }

    public List<Product> GetList()
    {
        // TODO: Let call pass in ListID
        return _context.Products.ToList();
    }

    public ShoppingList CreateList(string listName, int userId)
    {
        // TODO: Create new list for given user
        ShoppingList list = new ShoppingList()
        {
            Name = listName,
        };
        User user = _context.Users.FirstOrDefault(u => u.UserId == userId);
        list.UserShoppingLists.Add(new UserShoppingList(){
            UserId = user.UserId,
            User = user,
            ListId = list.ListId,
            List = list
        });
        _context.ShoppingLists.Add(list);
        _context.SaveChanges();

        return list;
    }

    public List<ShoppingList> GetUserLists(int userId)
    {
        List<ShoppingList> userLists = _context.ShoppingLists.Where(
            l => l.UserShoppingLists.Any(
                ul => ul.UserId == userId
            )
        ).ToList();

        return userLists;
    }

    public bool ShareListWithUser(string ownerId, string listID, string userId)
    {
        // TODO: Share list with given user
        return false;
    }
}