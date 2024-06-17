namespace DataLayer;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public int Stock { get; set; }

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
}
