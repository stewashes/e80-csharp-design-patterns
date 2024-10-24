using CSharpCourse.DesignPatterns.Creational.AbstractFactory;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.AbstractFactoryTests;

public class CloudInfrastructureTests
{
    [Fact]
    public void AwsFactory()
    {
        var factory = new AwsServiceFactory();

        var storage = factory.CreateStorageService();
        var compute = factory.CreateComputeService();
        var network = factory.CreateNetworkService();

        Assert.IsType<AwsStorage>(storage);
        Assert.IsType<AwsCompute>(compute);
        Assert.IsType<AwsNetwork>(network);
    }

    [Fact]
    public void AzureFactory()
    {
        var factory = new AzureServiceFactory();

        var storage = factory.CreateStorageService();
        var compute = factory.CreateComputeService();
        var network = factory.CreateNetworkService();

        Assert.IsType<AzureStorage>(storage);
        Assert.IsType<AzureCompute>(compute);
        Assert.IsType<AzureNetwork>(network);
    }

    [Fact]
    public void AwsClient()
    {
        var factory = new AwsServiceFactory();
        var client = new CloudInfrastructureClient(factory);

        client.ProvisionInfrastructure();
        Assert.True(true); // suppresses xUnit warning
    }

    [Fact]
    public void AzureClient()
    {
        var factory = new AzureServiceFactory();
        var client = new CloudInfrastructureClient(factory);

        client.ProvisionInfrastructure();
        Assert.True(true); // suppresses xUnit warning
    }

    [Theory]
    [InlineData("test-data")]
    public void AwsStorage(string testData)
    {
        var factory = new AwsServiceFactory();
        var storage = factory.CreateStorageService();

        var result = storage.StoreData(testData);
        var retrieveResult = storage.RetrieveData("test-id");

        Assert.Contains("S3", result);
        Assert.Contains(testData, result);
        Assert.Contains("S3 bucket", retrieveResult);
    }

    [Theory]
    [InlineData("test-data")]
    public void AzureStorage(string testData)
    {
        var factory = new AzureServiceFactory();
        var storage = factory.CreateStorageService();

        var result = storage.StoreData(testData);
        var retrieveResult = storage.RetrieveData("test-id");

        Assert.Contains("Azure Blob Storage", result);
        Assert.Contains(testData, result);
        Assert.Contains("Azure Blob Storage", retrieveResult);
    }

    [Fact]
    public void ProviderSwitch()
    {
        var AwsFactory = new AwsServiceFactory();
        var azureFactory = new AzureServiceFactory();

        var AwsClient = new CloudInfrastructureClient(AwsFactory);
        var azureClient = new CloudInfrastructureClient(azureFactory);

        AwsClient.ProvisionInfrastructure();
        azureClient.ProvisionInfrastructure();
        Assert.True(true); // suppresses xUnit warning
    }
}
