using System.ComponentModel.DataAnnotations;

namespace bagit_api.Models;

public class MockShoppingList
{
    [Key]
    public int ListId { get; set; }
    public string? Name { get; set; }
    public string? Notes { get; set; }
    public string? Description { get; set; }
    public bool? IsPublic { get; set; }
    public bool? IsEditable { get; set; }
    
    public virtual ICollection<Product> Products { get; set; }
}