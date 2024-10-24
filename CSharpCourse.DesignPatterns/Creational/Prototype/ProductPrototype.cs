using System.Runtime.Serialization.Formatters.Binary;

namespace CSharpCourse.DesignPatterns.Creational.Prototype;

#region Manual Clone
internal record Product
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public Size Size { get; set; }

    public Product(string name, string brand, Size size)
    {
        Name = name;
        Brand = brand;
        Size = size;
    }

    #region Copy Constructor
    // Copy constructors originate from C++ and are not very idiomatic in C#.
    public Product(Product product)
    {
        Name = product.Name;
        Brand = product.Brand;
        Size = new Size(product.Size);
    }
    #endregion
}

internal record Size
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }

    public Size(double width, double height, double depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }

    #region Copy Constructor
    // Copy constructors originate from C++ and are not very idiomatic in C#.
    public Size(Size size)
    {
        Width = size.Width;
        Height = size.Height;
        Depth = size.Depth;
    }
    #endregion
}
#endregion

#region ICloneable
internal class CloneableProduct : ICloneable
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public CloneableSize Size { get; set; }

    public CloneableProduct(string name, string brand, CloneableSize size)
    {
        Name = name;
        Brand = brand;
        Size = size;
    }
    
    // With ICloneable, we never know if it's going to perform
    // shallow or deep cloning since it's not specified.
    // ---
    // ICloneable is old and the Clone method returns an object
    // (there were no generics at the time), so the consumer needs to
    // cast the result to the correct type.
    public object Clone()
    {
        // If we return new CloneableProduct(Name, Brand, Size)
        // that would perform a shallow copy, and the reference to
        // the Size would be the same in both objects.
        return new CloneableProduct(Name, Brand, (CloneableSize)Size.Clone());
    }
}

internal class CloneableSize : ICloneable
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }

    public CloneableSize(double width, double height, double depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }
    
    // With ICloneable, we never know if it's going to perform
    // shallow or deep cloning since it's not specified.
    // ---
    // ICloneable is old and the Clone method returns an object
    // (there were no generics at the time), so the consumer needs to
    // cast the result to the correct type.
    public object Clone()
    {
        return new CloneableSize(Width, Height, Depth);
    }
}
#endregion

#region IPrototype
// We build a custom IPrototype interface that supports deep copying.
// Con: ALL classes in the nested hierarchy must implement IPrototype.
internal interface IPrototype<out T> // Covariant
{
    #region Covariance
    // Covariance means that you can use a more derived type
    // than originally specified. We can assign an IPrototype<DerivedProduct>
    // (or simply a DerivedProduct) to an IPrototype<Product> variable.
    #endregion
    T DeepCopy();
}

internal class ProductPrototype : IPrototype<ProductPrototype>
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public SizePrototype Size { get; set; }

    public ProductPrototype(string name, string brand, SizePrototype size)
    {
        Name = name;
        Brand = brand;
        Size = size;
    }

    public ProductPrototype DeepCopy()
    {
        return new ProductPrototype(Name, Brand, Size.DeepCopy());
    }
}

internal class SizePrototype : IPrototype<SizePrototype>
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }

    public SizePrototype(double width, double height, double depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }

    public SizePrototype DeepCopy()
    {
        return new SizePrototype(Width, Height, Depth);
    }
}
#endregion
