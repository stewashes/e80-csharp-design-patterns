using CSharpCourse.DesignPatterns.Creational.Prototype;
using Mapster;
using System.Text.Json;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.PrototypeTests;

public class ProductPrototypeTests
{
    [Fact]
    public void ManualClone()
    {
        // This is our prototype.
        var curtain1 = new Product("Medium window curtain", "DIY World",
            new Size(45, 140, 1));

        // Let's create a new product with some slight changes.
        var curtain2 = new Product(
            "Big window curtain", curtain1.Brand,
            new Size(curtain1.Size.Width, 160, curtain1.Size.Depth));

        // Make sure the references are different.
        Assert.NotSame(curtain1, curtain2);
        Assert.NotSame(curtain1.Size, curtain2.Size);

        // Make sure both products have the correct properties.
        Assert.Equal("Medium window curtain", curtain1.Name);
        Assert.Equal("DIY World", curtain1.Brand);
        Assert.Equal(45, curtain1.Size.Width);
        Assert.Equal(140, curtain1.Size.Height);
        Assert.Equal(1, curtain1.Size.Depth);

        Assert.Equal("Big window curtain", curtain2.Name);
        Assert.Equal("DIY World", curtain2.Brand);
        Assert.Equal(45, curtain2.Size.Width);
        Assert.Equal(160, curtain2.Size.Height);
        Assert.Equal(1, curtain2.Size.Depth);
    }

    [Fact]
    public void CloneablePerson()
    {
        // This is our prototype.
        var curtain1 = new CloneableProduct("Medium window curtain", "DIY World",
            new CloneableSize(45, 140, 1));

        // Let's create a new product with some slight changes.
        var curtain2 = (CloneableProduct)curtain1.Clone();
        curtain2.Name = "Big window curtain";
        curtain2.Size.Height = 160;

        // Make sure the references are different.
        Assert.NotSame(curtain1, curtain2);
        Assert.NotSame(curtain1.Size, curtain2.Size);

        // Make sure both products have the correct properties.
        Assert.Equal("Medium window curtain", curtain1.Name);
        Assert.Equal("DIY World", curtain1.Brand);
        Assert.Equal(45, curtain1.Size.Width);
        Assert.Equal(140, curtain1.Size.Height);
        Assert.Equal(1, curtain1.Size.Depth);

        Assert.Equal("Big window curtain", curtain2.Name);
        Assert.Equal("DIY World", curtain2.Brand);
        Assert.Equal(45, curtain2.Size.Width);
        Assert.Equal(160, curtain2.Size.Height);
        Assert.Equal(1, curtain2.Size.Depth);
    }

    [Fact]
    public void CopyConstructorClone()
    {
        // This is our prototype.
        var curtain1 = new Product("Medium window curtain", "DIY World",
            new Size(45, 140, 1));

        // Let's create a new product with some slight changes.
        var curtain2 = new Product(curtain1);
        curtain2.Name = "Big window curtain";
        curtain2.Size.Height = 160;

        // Make sure the references are different.
        Assert.NotSame(curtain1, curtain2);
        Assert.NotSame(curtain1.Size, curtain2.Size);

        // Make sure both products have the correct properties.
        Assert.Equal("Medium window curtain", curtain1.Name);
        Assert.Equal("DIY World", curtain1.Brand);
        Assert.Equal(45, curtain1.Size.Width);
        Assert.Equal(140, curtain1.Size.Height);
        Assert.Equal(1, curtain1.Size.Depth);

        Assert.Equal("Big window curtain", curtain2.Name);
        Assert.Equal("DIY World", curtain2.Brand);
        Assert.Equal(45, curtain2.Size.Width);
        Assert.Equal(160, curtain2.Size.Height);
        Assert.Equal(1, curtain2.Size.Depth);
    }

    [Fact]
    public void ExplicitDeepCopy()
    {
        // This is our prototype.
        var curtain1 = new ProductPrototype("Medium window curtain", "DIY World",
            new SizePrototype(45, 140, 1));

        // Let's create a new product with some slight changes.
        var curtain2 = curtain1.DeepCopy();
        curtain2.Name = "Big window curtain";
        curtain2.Size.Height = 160;

        // Make sure the references are different.
        Assert.NotSame(curtain1, curtain2);
        Assert.NotSame(curtain1.Size, curtain2.Size);

        // Make sure both products have the correct properties.
        Assert.Equal("Medium window curtain", curtain1.Name);
        Assert.Equal("DIY World", curtain1.Brand);
        Assert.Equal(45, curtain1.Size.Width);
        Assert.Equal(140, curtain1.Size.Height);
        Assert.Equal(1, curtain1.Size.Depth);

        Assert.Equal("Big window curtain", curtain2.Name);
        Assert.Equal("DIY World", curtain2.Brand);
        Assert.Equal(45, curtain2.Size.Width);
        Assert.Equal(160, curtain2.Size.Height);
        Assert.Equal(1, curtain2.Size.Depth);
    }

    [Fact]
    public void SerializationClone()
    {
        // This is our prototype.
        var curtain1 = new ProductPrototype("Medium window curtain", "DIY World",
            new SizePrototype(45, 140, 1));

        // Let's create a new product with some slight changes.
        var curtain2 = JsonSerializer.Deserialize<ProductPrototype>(
            JsonSerializer.Serialize(curtain1))!;
        curtain2.Name = "Big window curtain";
        curtain2.Size.Height = 160;

        // Note: a more performant way would be to use
        // protobuf-net for serialization, since it serializes to
        // a binary format, which is faster than JSON.

        // Make sure the references are different.
        Assert.NotSame(curtain1, curtain2);
        Assert.NotSame(curtain1.Size, curtain2.Size);

        // Make sure both products have the correct properties.
        Assert.Equal("Medium window curtain", curtain1.Name);
        Assert.Equal("DIY World", curtain1.Brand);
        Assert.Equal(45, curtain1.Size.Width);
        Assert.Equal(140, curtain1.Size.Height);
        Assert.Equal(1, curtain1.Size.Depth);

        Assert.Equal("Big window curtain", curtain2.Name);
        Assert.Equal("DIY World", curtain2.Brand);
        Assert.Equal(45, curtain2.Size.Width);
        Assert.Equal(160, curtain2.Size.Height);
        Assert.Equal(1, curtain2.Size.Depth);
    }

    [Fact]
    public void MapperClone()
    {
        // We can use a mapper like AutoMapper or Mapster to perform
        // a deep clone of objects. Here is an example with Mapster.

        // This is our prototype.
        var curtain1 = new ProductPrototype("Medium window curtain", "DIY World",
            new SizePrototype(45, 140, 1));

        // Let's create a new product with some slight changes.
        var curtain2 = curtain1.Adapt<ProductPrototype>();
        curtain2.Name = "Big window curtain";
        curtain2.Size.Height = 160;

        // Make sure the references are different.
        Assert.NotSame(curtain1, curtain2);
        Assert.NotSame(curtain1.Size, curtain2.Size);

        // Make sure both products have the correct properties.
        Assert.Equal("Medium window curtain", curtain1.Name);
        Assert.Equal("DIY World", curtain1.Brand);
        Assert.Equal(45, curtain1.Size.Width);
        Assert.Equal(140, curtain1.Size.Height);
        Assert.Equal(1, curtain1.Size.Depth);

        Assert.Equal("Big window curtain", curtain2.Name);
        Assert.Equal("DIY World", curtain2.Brand);
        Assert.Equal(45, curtain2.Size.Width);
        Assert.Equal(160, curtain2.Size.Height);
        Assert.Equal(1, curtain2.Size.Depth);
    }

    [Fact]
    public void RecordWithKeywordClone()
    {
        // Records are immutable reference types that are perfect for
        // the Prototype pattern. We can use the 'with' keyword to
        // create a new object with some modifications.

        // This is our prototype.
        var curtain1 = new Product("Medium window curtain", "DIY World",
            new Size(45, 140, 1));

        // Let's create a new product with some slight changes using the 'with' keyword.
        var curtain2 = curtain1 with
        {
            Name = "Big window curtain",
            Size = curtain1.Size with { Height = 160 }
        };

        // Make sure the references are different.
        Assert.NotSame(curtain1, curtain2);
        Assert.NotSame(curtain1.Size, curtain2.Size);

        // Make sure both products have the correct properties.
        Assert.Equal("Medium window curtain", curtain1.Name);
        Assert.Equal("DIY World", curtain1.Brand);
        Assert.Equal(45, curtain1.Size.Width);
        Assert.Equal(140, curtain1.Size.Height);
        Assert.Equal(1, curtain1.Size.Depth);

        Assert.Equal("Big window curtain", curtain2.Name);
        Assert.Equal("DIY World", curtain2.Brand);
        Assert.Equal(45, curtain2.Size.Width);
        Assert.Equal(160, curtain2.Size.Height);
        Assert.Equal(1, curtain2.Size.Depth);
    }
}
