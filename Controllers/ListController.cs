﻿using bagit_api.Data;
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
        // TODO: Let call pass in ListID
        // return list when it matches id 
        // var list = _context.ShoppingLists.Where(l => l.ListId == id).FirstOrDefault();
        // var products = list.ShoppingListProducts.Where(l => l.ListId == id);
        // return products.Where(l => l.ListId == id);

        // return _context.ShoppingLists
        //     .Include(p => p.ShoppingListProducts)
        //     .ThenInclude(ShoppingListProduct => ShoppingListProduct.Product)
        //     .Where(p => p.ListId == id).ToList();

        // return await _context.ShoppingLists.Where(p => p.ListId == id).Include(p => p.ShoppingListProducts).ThenInclude(p => p.Product).ToListAsync();

        // _context.ShoppingLists.Include(ec => ec.ShoppingListProducts)
        //             .Include(ec => ec.ListId)
        //             .Select(ec => ec.ShoppingListProducts)

        return _context.Products.Include(e => e.ShoppingListProducts)
                  .ThenInclude(ec => ec.List).ToList();

    }

    public async Task<ShoppingList> CreateList(string listName, int userId)
    {

        //   var product = new Product
        // {
        //     Name = itemName,
        //     Quantity = int.Parse(quantity)
        // };

        // var slProduct = new ShoppingListProduct
        // {
        //     ListId = listId,
        //     Product = product
        // };

        // _context.Add(slProduct);
        // _context.SaveChanges();
        
        // TODO: Create new list for given user
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

        return userLists;
    }

    public bool ShareListWithUser(string ownerId, string listID, string userId)
    {
        // TODO: Share list with given user
        return false;
    }
}