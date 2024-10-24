using CSharpCourse.DesignPatterns.Creational.Builder;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.BuilderTests;

public class PizzaBuilderTests
{
    [Fact]
    public void RegularBuilder()
    {
        var pizzaBuilder = new PizzaBuilder();
        pizzaBuilder.SetDough(PizzaDough.ThickCrust);
        pizzaBuilder.SetSauce(PizzaSauce.Tomato);
        pizzaBuilder.AddTopping("Mozzarella");
        pizzaBuilder.AddTopping("Basil");
        
        var pizza = pizzaBuilder.Build();
        
        Assert.Equal(PizzaDough.ThickCrust, pizza.Dough);
        Assert.Equal(PizzaSauce.Tomato, pizza.Sauce);
        Assert.Equal(2, pizza.Toppings.Count);
        Assert.Contains("Mozzarella", pizza.Toppings);
        Assert.Contains("Basil", pizza.Toppings);
    }

    [Fact]
    public void FluentBuilder()
    {
        var pizza = FluentPizzaBuilder
            .CreatePizza()
            .WithDough(PizzaDough.ThickCrust)
            .WithSauce(PizzaSauce.Tomato)
            .AddTopping("Mozzarella")
            .AddTopping("Basil")
            .Build();
        
        Assert.Equal(PizzaDough.ThickCrust, pizza.Dough);
        Assert.Equal(PizzaSauce.Tomato, pizza.Sauce);
        Assert.Equal(2, pizza.Toppings.Count);
        Assert.Contains("Mozzarella", pizza.Toppings);
        Assert.Contains("Basil", pizza.Toppings);
    }
}
