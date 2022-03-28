namespace bagit_api.Data;
using bagit_api.Models;

public class SeedData
{
    public static ShoppingList GetList() {
        return new ShoppingList
        {   
            ListId = 1,
            Name =  "Test Shopping List"
        };
    }
    
    public static User GetUser() {
        return new User
        {   
            UserId = 1,
            Username = "test_user",
            Email = "test@gmail.com"
        };
    }
}