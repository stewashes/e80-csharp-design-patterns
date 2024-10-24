namespace CSharpCourse.DesignPatterns.Creational.AbstractFactory;

// Abstract Products
public interface IStorageService
{
    string StoreData(string data);
    string RetrieveData(string id);
}

public interface IComputeService
{
    void StartInstance(string instanceId);
    void StopInstance(string instanceId);
}

public interface INetworkService
{
    void CreateVirtualNetwork(string name);
    void ConfigureFirewall(string networkId);
}

// Concrete Products for AWS
public class AwsStorage : IStorageService
{
    public string StoreData(string data)
    {
        return $"Storing data in S3: {data}";
    }

    public string RetrieveData(string id)
    {
        return $"Retrieving data from S3 bucket with ID: {id}";
    }
}

public class AwsCompute : IComputeService
{
    public void StartInstance(string instanceId)
    {
        Console.WriteLine($"Starting EC2 instance: {instanceId}");
    }

    public void StopInstance(string instanceId)
    {
        Console.WriteLine($"Stopping EC2 instance: {instanceId}");
    }
}

public class AwsNetwork : INetworkService
{
    public void CreateVirtualNetwork(string name)
    {
        Console.WriteLine($"Creating AWS VPC: {name}");
    }

    public void ConfigureFirewall(string networkId)
    {
        Console.WriteLine($"Configuring AWS Security Groups for VPC: {networkId}");
    }
}

// Concrete Products for Azure
public class AzureStorage : IStorageService
{
    public string StoreData(string data)
    {
        return $"Storing data in Azure Blob Storage: {data}";
    }

    public string RetrieveData(string id)
    {
        return $"Retrieving data from Azure Blob Storage with ID: {id}";
    }
}

public class AzureCompute : IComputeService
{
    public void StartInstance(string instanceId)
    {
        Console.WriteLine($"Starting Azure VM: {instanceId}");
    }

    public void StopInstance(string instanceId)
    {
        Console.WriteLine($"Stopping Azure VM: {instanceId}");
    }
}

public class AzureNetwork : INetworkService
{
    public void CreateVirtualNetwork(string name)
    {
        Console.WriteLine($"Creating Azure VNet: {name}");
    }

    public void ConfigureFirewall(string networkId)
    {
        Console.WriteLine($"Configuring Azure Network Security Groups: {networkId}");
    }
}

// Abstract Factory
public interface ICloudServiceFactory
{
    IStorageService CreateStorageService();
    IComputeService CreateComputeService();
    INetworkService CreateNetworkService();
}

// Concrete Factories
public class AwsServiceFactory : ICloudServiceFactory
{
    public IStorageService CreateStorageService()
    {
        return new AwsStorage();
    }

    public IComputeService CreateComputeService()
    {
        return new AwsCompute();
    }

    public INetworkService CreateNetworkService()
    {
        return new AwsNetwork();
    }
}

public class AzureServiceFactory : ICloudServiceFactory
{
    public IStorageService CreateStorageService()
    {
        return new AzureStorage();
    }

    public IComputeService CreateComputeService()
    {
        return new AzureCompute();
    }

    public INetworkService CreateNetworkService()
    {
        return new AzureNetwork();
    }
}

// Client code example
public class CloudInfrastructureClient
{
    private readonly ICloudServiceFactory _cloudServiceFactory;

    public CloudInfrastructureClient(ICloudServiceFactory cloudServiceFactory)
    {
        _cloudServiceFactory = cloudServiceFactory;
    }

    public void ProvisionInfrastructure()
    {
        var storage = _cloudServiceFactory.CreateStorageService();
        var compute = _cloudServiceFactory.CreateComputeService();
        var network = _cloudServiceFactory.CreateNetworkService();

        // Use the services
        network.CreateVirtualNetwork("MainNetwork");
        network.ConfigureFirewall("main-network-01");

        compute.StartInstance("web-server-01");

        var dataId = storage.StoreData("Configuration data");
        var retrievedData = storage.RetrieveData(dataId);

        Console.WriteLine(retrievedData);
    }
}
