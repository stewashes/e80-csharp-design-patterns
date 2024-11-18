namespace CSharpCourse.DesignPatterns.Behavioral.TemplateMethod;

internal record StandardizedData
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public decimal Value { get; set; }
    public DateTime Timestamp { get; set; }
}

internal interface IRawAdsData
{
    
}

internal class GoogleAdsData : IRawAdsData
{
    public required string AdId { get; set; }
    public required string CampaignName { get; set; }
    public required string AdName { get; set; }
    public double Value { get; set; }
    public DateTime Time { get; set; }
}

internal class MetaAdsData : IRawAdsData
{
    public long Id { get; set; }
    public required string AdGroupName { get; set; }
    public decimal Value { get; set; }
    public long TimestampUtc { get; set; }
}

// Abstract base class that defines the template method
internal abstract class AdsDataCollector
{
    // Template method that defines the algorithm's skeleton
    public async Task<IEnumerable<StandardizedData>> CollectDataAsync()
    {
        Console.WriteLine($"Starting data collection from {GetSourceName()}");

        // 1. Validate connection
        await ValidateConnectionAsync();

        // 2. Retrieve raw data
        var rawData = await FetchRawDataAsync();

        // 3. Transform to standard format
        var standardizedData = TransformData(rawData);

        // 4. Validate transformed data
        ValidateTransformedData(standardizedData);

        Console.WriteLine($"Completed data collection from {GetSourceName()}");
        return standardizedData;
    }

    // Abstract methods to be implemented by concrete classes
    protected abstract string GetSourceName();
    protected abstract Task ValidateConnectionAsync();
    protected abstract Task<IEnumerable<IRawAdsData>> FetchRawDataAsync();
    protected abstract IEnumerable<StandardizedData> TransformData(IEnumerable<IRawAdsData> rawData);

    // Hook method that can be overridden if needed
    protected virtual void ValidateTransformedData(IEnumerable<StandardizedData> data)
    {
        foreach (var item in data)
        {
            if (string.IsNullOrWhiteSpace(item.Id))
            {
                throw new ValidationException($"Invalid data: Missing ID in {GetSourceName()}");
            }

            if (item.Timestamp == default)
            {
                throw new ValidationException($"Invalid data: Missing timestamp in {GetSourceName()}");
            }
        }
    }
}

public class ValidationException(string message) : Exception(message)
{

}

internal class GoogleAdsDataCollector : AdsDataCollector
{
    private readonly string _apiKey;

    public GoogleAdsDataCollector(string apiKey)
    {
        _apiKey = apiKey;
    }

    protected override string GetSourceName() => "Google Ads";

    protected override async Task ValidateConnectionAsync()
    {
        // Simulate API health check
        await Task.Delay(100);

        Console.WriteLine("Google Ads API connection validated");
    }

    protected override async Task<IEnumerable<IRawAdsData>> FetchRawDataAsync()
    {
        // Simulate Google Ads API call
        await Task.Delay(200);

        return [];
    }

    protected override IEnumerable<StandardizedData> TransformData(IEnumerable<IRawAdsData> rawData)
    {
        return rawData
            .Cast<GoogleAdsData>()
            .Select(item => new StandardizedData
            {
                Id = item.AdId,
                Name = $"{item.CampaignName} - {item.AdName}",
                Value = (decimal)item.Value,
                Timestamp = item.Time
            });
    }
}

internal class MetaAdsDataCollector : AdsDataCollector
{
    private readonly string _apiKey;

    public MetaAdsDataCollector(string apiKey)
    {
        _apiKey = apiKey;
    }

    protected override string GetSourceName() => "Meta Ads";

    protected override async Task ValidateConnectionAsync()
    {
        // Simulate API health check
        await Task.Delay(100);

        Console.WriteLine("Meta Ads API connection validated");
    }

    protected override async Task<IEnumerable<IRawAdsData>> FetchRawDataAsync()
    {
        // Simulate Meta Ads API call
        await Task.Delay(200);

        return [];
    }

    protected override IEnumerable<StandardizedData> TransformData(IEnumerable<IRawAdsData> rawData)
    {
        return rawData
            .Cast<MetaAdsData>()
            .Select(item => new StandardizedData
            {
                Id = item.Id.ToString(),
                Name = item.AdGroupName,
                Value = item.Value,
                Timestamp = DateTimeOffset.FromUnixTimeSeconds(item.TimestampUtc).DateTime
            });
    }

    // Overriding the hook method to add extra validation
    protected override void ValidateTransformedData(IEnumerable<StandardizedData> data)
    {
        base.ValidateTransformedData(data);

        foreach (var item in data)
        {
            if (item.Value <= 0)
            {
                throw new ValidationException($"Invalid data: Non-positive value in {GetSourceName()}");
            }
        }
    }
}
