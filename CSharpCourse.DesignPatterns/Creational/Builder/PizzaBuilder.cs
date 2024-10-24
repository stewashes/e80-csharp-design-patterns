namespace CSharpCourse.DesignPatterns.Creational.Builder;

internal enum PizzaDough
{
    Thin,
    Regular,
    ThickCrust
}

internal enum PizzaSauce
{
    Tomato,
    Marinara,
    White
}

internal record Pizza
{
    public PizzaDough Dough { get; set; } = PizzaDough.Regular;
    public PizzaSauce Sauce { get; set; } = PizzaSauce.Tomato;
    public List<string> Toppings { get; } = [];
}

internal class PizzaBuilder
{
    private readonly Pizza _pizza = new();

    public void SetDough(PizzaDough dough)
    {
        _pizza.Dough = dough;
    }

    public void SetSauce(PizzaSauce sauce)
    {
        _pizza.Sauce = sauce;
    }

    public void AddTopping(string topping)
    {
        _pizza.Toppings.Add(topping);
    }

    public Pizza Build()
    {
        // We could also create the object here instead, for
        // example if we wanted to perform validation.
        // Another option is to set defaults and validate
        // each change individually.
        return _pizza;
    }
}

internal class FluentPizzaBuilder
{
    private readonly Pizza _pizza = new();

    private FluentPizzaBuilder() { }

    public static FluentPizzaBuilder CreatePizza() => new();

    public FluentPizzaBuilder WithDough(PizzaDough dough)
    {
        _pizza.Dough = dough;
        return this;
    }

    public FluentPizzaBuilder WithSauce(PizzaSauce sauce)
    {
        _pizza.Sauce = sauce;
        return this;
    }

    public FluentPizzaBuilder AddTopping(string topping)
    {
        _pizza.Toppings.Add(topping);
        return this;
    }

    public Pizza Build()
    {
        return _pizza;
    }
}
