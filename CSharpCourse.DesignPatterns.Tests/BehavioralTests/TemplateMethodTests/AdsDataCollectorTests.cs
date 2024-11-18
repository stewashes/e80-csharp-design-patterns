using CSharpCourse.DesignPatterns.Behavioral.TemplateMethod;
using Moq;
using Moq.Protected;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.TemplateMethodTests;

public class AdsDataCollectorTests
{
    [Fact]
    public async Task AdsDataCollector()
    {
        var googleAdsDataCollector = new Mock<GoogleAdsDataCollector>("api_key")
        {
            CallBase = true
        };

        // Careful! When the name of the FetchRawDataAsync method
        // changes, this test will fail. We cannot do otherwise since
        // this method is protected.
        googleAdsDataCollector
            .Protected()
            .Setup<Task<IEnumerable<IRawAdsData>>>("FetchRawDataAsync")
            .ReturnsAsync(() =>
            [
                new GoogleAdsData()
                {
                    AdId = "123",
                    CampaignName = "Summer Sale",
                    AdName = "Ad 1",
                    Value = 100.0,
                    Time = DateTime.UtcNow - TimeSpan.FromDays(180)
                },
                new GoogleAdsData()
                {
                    AdId = "456",
                    CampaignName = "Winter Sale",
                    AdName = "Ad 2",
                    Value = 200.0,
                    Time = DateTime.UtcNow
                }
            ]);

        var data = await googleAdsDataCollector.Object.CollectDataAsync();

        Assert.Equal(2, data.Count());
        Assert.Equal(300m, data.Sum(x => x.Value));

        var metaAdsDataCollector = new Mock<MetaAdsDataCollector>("api_key")
        {
            CallBase = true
        };

        metaAdsDataCollector
            .Protected()
            .Setup<Task<IEnumerable<IRawAdsData>>>("FetchRawDataAsync")
            .ReturnsAsync(() =>
            [
                new MetaAdsData() {
                    Id = 123,
                    AdGroupName = "Summer Sale",
                    Value = 100.0m,
                    TimestampUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 180 * 24 * 60 * 60
                },
                new MetaAdsData() {
                    Id = 456,
                    AdGroupName = "Winter Sale",
                    Value = 200.0m,
                    TimestampUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
                }
            ]);

        data = await metaAdsDataCollector.Object.CollectDataAsync();

        Assert.Equal(2, data.Count());
        Assert.Equal(300m, data.Sum(x => x.Value));
    }
}
