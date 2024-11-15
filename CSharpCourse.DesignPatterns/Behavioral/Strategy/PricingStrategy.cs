namespace CSharpCourse.DesignPatterns.Behavioral.Strategy;

internal record Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
}

internal class ItemPriceCalculator
{
    public async Task<Item> ApplyPriceAsync(Item item, string couponCode = "")
        => throw new NotImplementedException();
}
