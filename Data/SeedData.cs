using bagit_api.Services;
using Microsoft.Extensions.Options;

namespace bagit_api.Data;
using bagit_api.Models;

public class SeedData
{
    public static ShoppingList GetList() {
        return new ShoppingList
        {   
            ListId = 1,
            Name =  "Test Shopping List",
        };
    }
    
    public static User GetUser() {
        SecurityService security = new SecurityService(Options.Create(new HashingOptions()));
        return new User
        {   
            UserId = 1,
            Username = "test_user",
            Email = "test@gmail.com",
            Password = security.Hash("123456")
        };
    }
}