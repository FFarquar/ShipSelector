using ShipSelector.Models;

namespace ShipSelector.Services.LocalStorageService
{
    public interface IStorageService
    {
        Task <ServiceResponse<int>> AddUnitsToStorage (List<UnitForGameSystemDTO> units);
        Task <ServiceResponse<bool>> RemoveAllUnitsFromStorage();
        Task<ServiceResponse<List<UnitForGameSystemDTO>>> RetrieveAllUnits();
    }
}
