namespace FactoryPulse_Core.Entities;
using System.ComponentModel.DataAnnotations;

public class Inventory
{
    [Key]
    public int ItemID { get; set; } 
    public string Barcode { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Location { get; set; } = string.Empty;
}
