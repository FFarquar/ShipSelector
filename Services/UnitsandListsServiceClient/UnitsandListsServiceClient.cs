//using NavalGame.Client.Services.UploadDownloadService;
using ShipSelector.Models;
using ShipSelector.Services.UnitsandListsServiceClient;
using System.Net.Http.Json;
using ShipSelector.Models;
using System.Text.Json;

namespace ShipSelector.Services.UnitsandListsServiceClient
{
    public class UnitsandListsServiceClient : IUnitsandListsServiceClient
    {
        private readonly HttpClient _http;

        public UnitsandListsServiceClient(HttpClient http)
        {
            _http = http;
        }
        public List<UnitForGameSystemDTO> UnitList { get; set; } = new List<UnitForGameSystemDTO>(); //a list that contains all units. The printpage will have access to this

        public async Task<ServiceResponse<int>> AddUnit(Unit unitToAdd)
        {
            var result = await _http.PostAsJsonAsync("api/unitdetails/addUnit", unitToAdd);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        //public async Task<ServiceResponse<int>> AddGameSystemUnitSpecificDetail(GameSystemUnitSpecificDetail gamespefic, List<FileUploadDTO> browserFiles, int countryId)
        //{

        //    var filesUpload = new ServiceResponse<List<UploadResult>>();
        //    if (browserFiles.Count > 0)
        //    {
        //        filesUpload = await _UDSC.UploadFiles(browserFiles, gamespefic.RulesetId, countryId);
        //        if (!filesUpload.Success)
        //        {
        //            //the files weren't uploaded
        //            var sr = new ServiceResponse<int>
        //            {
        //                Message = "The files weren't uploaded correctly. Image not added " + filesUpload.Message,
        //                Success = false
        //            };
        //            return sr;
        //        }
        //    }

        //    var result = await _http.PostAsJsonAsync("api/unitdetails/addgamespecificDetailsForUnit", gamespefic);

        //    return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        //}

        public async Task<ServiceResponse<List<Country>>> GetListOfCountries()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Country>>>("api/unitdetails/countries");
            return result;
        }



        public async Task<ServiceResponse<List<SubUnitTypeDTO>>> GetListOfSubUnits()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<SubUnitTypeDTO>>>("api/unitdetails/subunits");
            return result;


        }

        public async Task<ServiceResponse<List<UnitType>>> GetListOfUnitTypes()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<UnitType>>>("api/unitdetails/unittypes");
            return result;
        }

        public async Task<ServiceResponse<List<RuleSet>>> GetRuleSets()
        {
            var result = await _http.GetFromJsonAsync<RuleSet[]>("Data/ruleSets.json");

            if(result != null && result.Length != 0)
            {
                return new ServiceResponse<List<RuleSet>>
                {
                    Data = result.ToList()
                };
            }
            else
            {
                return new ServiceResponse<List<RuleSet>>
                {
                    Data = new List<RuleSet>(),
                    Success = false,
                    Message = "No rulesets found"
                };

            }

        }

        public async Task<ServiceResponse<Unit>> GetUnitWithoutChildObjects(int unitId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Unit>>("api/unitdetails/getUnitNoChildObjects/" + unitId);
            return result;
        }

        public async Task<ServiceResponse<List<GameSystemUnitSpecificDetail>>> GetGameSystemUnitSpecificDetails(int unitId)
        {

            var result = await _http.GetFromJsonAsync<ServiceResponse<List<GameSystemUnitSpecificDetail>>>("api/unitdetails/getGameSystemUnitSpecificDetails/" + unitId);
            return result;

        }

        public async Task<ServiceResponse<RuleSet>> GetRuleSet(int rulesetId)
        {

            var result = await _http.GetFromJsonAsync<ServiceResponse<RuleSet>>("api/UnitDetails/getRuleSet/" + rulesetId);
            return result;

        }

        public async Task<ServiceResponse<List<UnitForGameSystemDTO>>> GetListofAllGameUnitsForRuleset(int rulesetId, bool onlyReturnUnitsInCollection)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<UnitForGameSystemDTO>>>("api/unitdetails/allunitsbyGameSystemandType/" + rulesetId + "/" + onlyReturnUnitsInCollection);

            //Before returning the data, we have to implement some changes to it based on the onlyReturnUnitsInCollection and rulesetId flags

            //If using my collection, each ship or sub will be returned as a unique entity, so it can only be selected once

            //if (onlyReturnUnitsInCollection && (unitTypeId == 1 || unitTypeId == 2))
            //{

            List<UnitForGameSystemDTO> unitList = new List<UnitForGameSystemDTO>();

            foreach (var unit in result.Data)
            {
                //Get a list of unique ship and subs if using local collection
                if (onlyReturnUnitsInCollection == true && (unit.subUnitTypeObj.UnitTypeId == 1 || unit.subUnitTypeObj.UnitTypeId == 3))
                {
                    List<string> shipList = new List<string>();
                    shipList = unit.ShipsSubsInClass.Split(",").ToList();

                    foreach (var ship in shipList)
                    {
                        //assign all details from returned server value
                        UnitForGameSystemDTO newUnit = new UnitForGameSystemDTO()
                        {
                            //TODO: The ID is problematic as it will be the same for multiple units
                            Id = unit.Id,
                            Name_ClassName = unit.Name_ClassName,
                            Countryobj = unit.Countryobj,
                            Alliance = unit.Alliance,
                            NumberinClass_shipSub = 1,
                            Cost = unit.Cost,
                            ImagePath = unit.ImagePath,
                            ShipsSubsInClass = ship.Trim(),
                            NumberSelected = 0,
                            AllowUnlimitedSelection = false,
                            AddButtonDisabled = false,
                            RemoveButtonDisabled = true,
                            styleForAddButton = Settings.styleForButtonAvailable,
                            styleForRemoveButton = Settings.styleForButonNotAvailable,
                            FilterVisible = false,
                            subUnitTypeObj = unit.subUnitTypeObj
                            //Country = unit.Country,
                            //SubUnitType = unit.SubUnitType,
                        };

                        unitList.Add(newUnit);
                    }

                }
                else
                {

                    unit.styleForAddButton = Settings.styleForButtonAvailable;
                    unit.styleForRemoveButton = Settings.styleForButonNotAvailable;
                    unit.AddButtonDisabled = false;
                    unit.RemoveButtonDisabled = true;
                    unit.FilterVisible = false;

                    //TODO: have to deal with the allowunlimited selection issue
                    //AllowUnlimitedSelection = false,
                    unitList.Add(unit);
                }
            }

            result.Data = unitList;
            return result;
            //}


        }

        public async Task SetListOfUnits(int rulesetId, bool onlyReturnUnitsInCollection)
        {
            var resultUnits = await GetListofAllGameUnitsForRuleset(rulesetId, onlyReturnUnitsInCollection);
            UnitList.Clear();
            UnitList = resultUnits.Data;
            

        }

        public async Task<ServiceResponse<List<UnitWithGameSystemDetails>>> GetListofAllGameUnitsWithGameSpecDetails()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List< UnitWithGameSystemDetails >>>("api/UnitDetails/allunitsWithGameSpecDetails");
            return result;
        }

        public async Task<ServiceResponse<int>> UpdateUnit(Unit unit)
        {
            var result = await _http.PutAsJsonAsync("api/UnitDetails/updateUnit", unit);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

        }

        public async Task<ServiceResponse<bool>> DeleteUnit(int unitId)
        {
            //TODO: Not tested yet
            var result = await _http.DeleteAsync("/api/unitDetails/" + unitId);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
