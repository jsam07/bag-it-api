using System.Diagnostics;
using bagit_api.Data;
using Microsoft.EntityFrameworkCore;

namespace bagit_api.Models;
public class BagIt
{
    private readonly BagItDbContext _context;

    public BagIt()
    {
        // TODO: Refactor this 
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

    public string CreateList(string userId)
    {
        // TODO: Create new list for given user
        return "";
    }

    public bool ShareListWithUser(string ownerId, string listID, string userId)
    {
        // TODO: Share list with given user
        return false;
    }
}