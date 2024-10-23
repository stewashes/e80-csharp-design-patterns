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
