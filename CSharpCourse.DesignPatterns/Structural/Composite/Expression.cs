﻿namespace CSharpCourse.DesignPatterns.Structural.Composite;

internal interface IExpression
{
    double Evaluate();
    string ToString();
}

internal class NumberExpression : IExpression
{
    private readonly double value;

    public NumberExpression(double value)
    {
        this.value = value;
    }

    public double Evaluate() => value;

    public override string ToString() => value.ToString();
}

internal class BinaryExpression : IExpression
{
    private readonly IExpression left;
    private readonly IExpression right;
    private readonly string operatorSymbol;
    private readonly Func<double, double, double> operation;

    public BinaryExpression(
        IExpression left,
        IExpression right,
        string operatorSymbol,
        Func<double, double, double> operation)
    {
        this.left = left;
        this.right = right;
        this.operatorSymbol = operatorSymbol;
        this.operation = operation;
    }

    public double Evaluate() => operation(left.Evaluate(), right.Evaluate());

    public override string ToString() => $"({left} {operatorSymbol} {right})";
}
