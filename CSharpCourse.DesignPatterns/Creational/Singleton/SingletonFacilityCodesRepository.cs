namespace CSharpCourse.DesignPatterns.Creational.Singleton;

internal interface IFacilityCodesRepository
{
    bool IsFacilityCodeSupported(string facilityCode);
    IEnumerable<string> GetSupportedFacilityCodes();
    void AddFacilityCode(string facilityCode);
    void RemoveFacilityCode(string facilityCode);
}
