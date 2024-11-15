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
