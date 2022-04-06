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
    
    public void AddItem(Product product, int listId)
    {
        // TODO: Let caller pass these parameters in

        var slProduct = new ShoppingListProduct
        {
            ListId = listId,
            Product = product
        };

        _context.Add(slProduct);
        _context.SaveChanges();
        Console.WriteLine($"\nAdded Product: {product}");
    }
    public void DeleteItem(Product product, int listId)
    {
        // TODO: Let caller pass in listID and product ID to delete
        var products = _context.Products.Include(p => p.ShoppingListProducts)
                  .ThenInclude(ec => ec.List)
                  .Where(p => p.ProductId == product.ProductId);

        _context.RemoveRange(products);

        _context.SaveChanges();
        Console.WriteLine($"\nDeleted Product: {product.ProductId}");
    }

    public List<Product> GetList(int id)
    {
        return _context.Products.Include(e => e.ShoppingListProducts)
            .ThenInclude(ec => ec.List)
            .Where(p => p.ShoppingListProducts.Any(ec => ec.ListId == id)).ToList();
     
    }

    public async Task<ShoppingList> CreateList(string listName, int userId)
    {
        ShoppingList list = new ShoppingList
        {
            Name = listName,
        };
        _context.Add(list);
        _context.SaveChanges();

        User user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

        if (user != null)
        {
             UserShoppingList userList = new UserShoppingList{
                UserId = userId,
                User = user,
                List = list,
                ListId = list.ListId,
            };

            _context.Add(userList);
            _context.SaveChanges();

            return list;
        }
        return null;
       
    }

    public List<ShoppingList> GetUserLists(int userId)
    {
        List<ShoppingList> userLists = _context.ShoppingLists.Where(
            l => l.UserShoppingLists.Any(
                ul => ul.UserId == userId
            )
        ).ToList();
        Console.WriteLine(userLists);
        return userLists;
    }
    
    // ListController
    public async Task ShareListWithUser(int listID, string email)
    {
        User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        ShoppingList list = await _context.ShoppingLists.FirstOrDefaultAsync(l => l.ListId == listID);
        
        Console.WriteLine(user.Email);
        if (user != null)
        {
            UserShoppingList userList = new UserShoppingList{
                UserId = user.UserId,
                User = user,
                List = list,
                ListId = list.ListId,
            };

            _context.Add(userList);
            _context.SaveChanges();
        }
    }

    public bool ShareListWithUser(string ownerId, string listID, string userId)
    {
        return false;
    }
}