using ShipSelector.Models;
using ShipSelector.Shared;

namespace ShipSelector.Services.UnitsandListsServiceClient
{
    public interface IUnitsandListsServiceClient
    {
        Task<ServiceResponse<List<RuleSet>>> GetRuleSets();
        Task<ServiceResponse<RuleSet>> GetRuleSet(int rulesetId);
        Task<ServiceResponse<List<Country>>> GetListOfCountries();
        Task<ServiceResponse<List<Country>>> GetListOfCountriesForSelectedUnitsInGameSystem(int gameSystem);
        Task<ServiceResponse<List<UnitType>>> GetListOfUnitTypes();

        Task<ServiceResponse<List<SubUnitTypeDTO>>> GetListOfSubUnits();
        Task<ServiceResponse<int>> AddUnit(Unit unitToAdd);

        
        
        Task<ServiceResponse<Unit>> GetUnitWithoutChildObjects(int unitId);
        Task<ServiceResponse<List<GameSystemUnitSpecificDetail>>> GetGameSystemUnitSpecificDetails(int unitId);
        Task<ServiceResponse<List<UnitForGameSystemDTO>>> GetListofAllGameUnitsForRuleset(int rulesetId, bool onlyReturnUnitsInCollection);
        Task SetListOfUnits(int rulesetId, bool onlyReturnUnitsInCollection);

        List<UnitForGameSystemDTO> UnitList {get;set;}
        Task<ServiceResponse<List<UnitWithGameSystemDetails>>> GetListofAllGameUnitsWithGameSpecDetails();
        Task<ServiceResponse<int>> UpdateUnit(Unit unit);
        Task<ServiceResponse<bool>> DeleteUnit(int unitId);
        Task<ServiceResponse<List<OrderCard>>> GetBroadSideOrderCards();
        Task<ServiceResponse<List<DamageCardData>>> GetBroadSideDamageCards();


        Task<ServiceResponse<int>> AddGameSystemUnitSpecificDetail(GameSystemUnitSpecificDetail gamespefic, List<FileUploadDTO> filesToUploadDTO, int countryId);
    }
}
