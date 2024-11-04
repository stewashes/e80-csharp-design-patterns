using CSharpCourse.DesignPatterns.Structural.Composite;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.CompositeTests;

public class ExpressionTests
{
    [Fact]
    public void Expression()
    {
        var expression = new BinaryExpression(
            new BinaryExpression(
                new NumberExpression(5),
                new NumberExpression(3),
                "+",
                (a, b) => a + b
            ),
            new NumberExpression(2),
            "*",
            (a, b) => a * b
        );

        var result = expression.Evaluate();
        var stringRepresentation = expression.ToString();

        Assert.Equal(16, result); // (5 + 3) * 2 = 16
        Assert.Equal("((5 + 3) * 2)", stringRepresentation);
    }
}
