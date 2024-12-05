//using NavalGame.Client.Services.UploadDownloadService;
using ShipSelector.Models;
using ShipSelector.Services.UnitsandListsServiceClient;
using System.Net.Http.Json;
using ShipSelector.Models;
using System.Text.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;

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

        public async Task<ServiceResponse<List<Country>>> GetListOfCountriesForSelectedUnitsInGameSystem(int ruleSetId)
        {
            //this function will get a list of countries that have actually had units assigned

            ServiceResponse<List<Country>> fullCountryList = await GetListOfCountries();

            var randomid = Guid.NewGuid().ToString();
            var gameSystemSpecificURL = $"Data/gameSystemSpecific.json?{randomid}";
            var GameSystemUnitJs = await _http.GetFromJsonAsync<GameSystemUnitSpecificDetail[]>(gameSystemSpecificURL);
            //var GameSystemUnitJs = await _http.GetFromJsonAsync<GameSystemUnitSpecificDetail[]>("Data/gameSystemSpecific.json");
            var UnitJS = await _http.GetFromJsonAsync<Unit[]>("Data/units.json");
            List<GameSystemUnitSpecificDetail> gameSpecList = GameSystemUnitJs.ToList();
            List<Unit> unitList = UnitJS.ToList();

            List<Country> filteredCountryList = new List<Country>();
            //TODO: This query works well when joining 2 tables and usign a where clause to filter selections
            foreach (var c in fullCountryList.Data)
            {
                var query = unitList
                                .Join(gameSpecList,
                                    ut => ut.Id,
                                    gsu => gsu.UnitId,
                                    (ut, gsu) => new {Unit = ut,  GameSystemUnitSpecificDetail = gsu})
                                .Where(x => x.Unit.CountryId == c.Id && x.GameSystemUnitSpecificDetail.RulesetId == ruleSetId);


                if (query.Count() > 0)
                {
                    filteredCountryList.Add(c);
                }
            }

            return new ServiceResponse<List<Country>>
            {
                Data = filteredCountryList
            };
        }

        public async Task<ServiceResponse<List<Country>>> GetListOfCountries()
        {
            var result = await _http.GetFromJsonAsync<Country[]>("Data/countries.json");

            if (result != null && result.Length != 0)
            {
                return new ServiceResponse<List<Country>>
                {
                    Data = result.ToList()
                };
            }
            else
            {
                return new ServiceResponse<List<Country>>
                {
                    Data = new List<Country>(),
                    Success = false,
                    Message = "No countries found"
                };

            }
            //            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Country>>>("api/unitdetails/countries");
            //            return result;
        }



        public async Task<ServiceResponse<List<SubUnitTypeDTO>>> GetListOfSubUnits()
        {

            var rawSubunit = await _http.GetStringAsync("Data/subUnitTypes.json");
            var subUnits = JsonConvert.DeserializeObject<List<SubUnitType>>(rawSubunit,

               new JsonSerializerSettings
               {
                   NullValueHandling = NullValueHandling.Ignore,
                   Converters =
                   {
                       new ColorConverter()
                   }
               });

            //Need to add the unit type object for subunit

            var unitTypesJS = await _http.GetFromJsonAsync<UnitType[]>("Data/unitTypes.json");

            List<UnitType> unitTypeList = unitTypesJS.ToList();

            var _includeUnitType = subUnits
                    .Join(unitTypeList,
                        su => su.UnitTypeId,
                            ut => ut.Id,
                                (su, ut) => new { SubUnitType = su, UnitType = ut });

            //if (subUnits != null && subUnits.Count > 0)
                if (_includeUnitType != null)

                {
                List<SubUnitTypeDTO> subunitresponse = new List<SubUnitTypeDTO>();
                foreach (var item in _includeUnitType)
                {
                    var subunit = new SubUnitTypeDTO
                    {
                        Id = item.SubUnitType.Id,
                        SubUnitName = item.SubUnitType.SubUnitName,
                        UnitTypeName = item.UnitType.Name,
                        UnitTypeId = item.UnitType.Id,
                        RGBDetails = new RGBDetails()
                        {
                            R = item.SubUnitType.PrintColour.R,
                            G = item.SubUnitType.PrintColour.G,
                            B = item.SubUnitType.PrintColour.B,
                        }
                    };
                    subunitresponse.Add(subunit);
                }
                return new ServiceResponse<List<SubUnitTypeDTO>>
                {
                    Data = subunitresponse
                };


                //List<SubUnitTypeDTO> subunitresponse = new List<SubUnitTypeDTO>();
                //foreach (var item in subUnits)
                //{
                //    var subunit = new SubUnitTypeDTO
                //    {
                //        Id = item.Id,
                //        SubUnitName = item.SubUnitName,
                //        UnitTypeName = item.UnitType.Name,
                //        UnitTypeId = item.UnitType.Id,
                //        RGBDetails = new RGBDetails()
                //        {
                //            R = item.PrintColour.R,
                //            G = item.PrintColour.G,
                //            B = item.PrintColour.B,
                //        }
                //    };
                //    subunitresponse.Add(subunit);
                //}
                //return new ServiceResponse<List<SubUnitTypeDTO>>
                //{
                //    Data = subunitresponse
                //};

            }
            else
            {
                return new ServiceResponse<List<SubUnitTypeDTO>>
                {
                    Data = new List<SubUnitTypeDTO>(),
                    Success = false,
                    Message = "No Sub units found"
                };

            }
            //var result = await _http.GetFromJsonAsync<ServiceResponse<List<SubUnitTypeDTO>>>("api/unitdetails/subunits");
            //return result;


        }

        public async Task<ServiceResponse<List<UnitType>>> GetListOfUnitTypes()
        {

            var result = await _http.GetFromJsonAsync<UnitType[]>("Data/unitTypes.json");

            if (result != null && result.Length != 0)
            {
                return new ServiceResponse<List<UnitType>>
                {
                    Data = result.ToList()
                };
            }
            else
            {
                return new ServiceResponse<List<UnitType>>
                {
                    Data = new List<UnitType>(),
                    Success = false,
                    Message = "No Unit types found"
                };

            }
            //var result = await _http.GetFromJsonAsync<ServiceResponse<List<UnitType>>>("api/unitdetails/unittypes");
            //return result;
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

            var result = await _http.GetFromJsonAsync<RuleSet[]>("Data/ruleSets.json");

            if (result != null && result.Length != 0)
            {
                var rs = result
                    .Where(item => item.Id == rulesetId)
                    .FirstOrDefault();
                return new ServiceResponse<RuleSet>
                {
                    Data = rs,
            };
            }
            else
            {
                return new ServiceResponse<RuleSet>
                {
                    Data = new RuleSet(),
                    Success = false,
                    Message = "No rulesets found"
                };

            }

            //var result = await _http.GetFromJsonAsync<ServiceResponse<RuleSet>>("api/UnitDetails/getRuleSet/" + rulesetId);
            //return result;
            //asd
        }

        public async Task<ServiceResponse<List<UnitForGameSystemDTO>>> GetListofAllGameUnitsForRuleset(int rulesetId, bool onlyReturnUnitsInCollection)
        {
            //var result = await _http.GetFromJsonAsync<ServiceResponse<List<UnitForGameSystemDTO>>>("api/unitdetails/allunitsbyGameSystemandType/" + rulesetId + "/" + onlyReturnUnitsInCollection);
            var CountriesJs = await _http.GetFromJsonAsync<Country[]>("Data/countries.json");
            var UnitsJs = await _http.GetFromJsonAsync<Unit[]>("Data/units.json");

            var rawSubunit = await _http.GetStringAsync("Data/subUnitTypes.json");
            //  var message = JsonConvert.DeserializeObject<List<SubUnitType>>(rawSubunit);

            var SubUnitTypesJs = JsonConvert.DeserializeObject<List<SubUnitType>>(rawSubunit,
               new JsonSerializerSettings
               {
                   NullValueHandling = NullValueHandling.Ignore,
                   Converters =
                   {
                       new ColorConverter()
                   }
               });

            //TODO: Is the colour correct? Does it need to be 64bit int

            var GameSystemUnitJs = await _http.GetFromJsonAsync<GameSystemUnitSpecificDetail[]>("Data/gameSystemSpecific.json");

            List<GameSystemUnitSpecificDetail> gameSpecList = GameSystemUnitJs.ToList();
            List<Country> countryList = CountriesJs.ToList();
            List<Unit> unitList = UnitsJs.ToList();
            List<SubUnitType> subUnitList = SubUnitTypesJs.ToList();

            //Get a GameSystemUnitSpecificDetail object with the Unit object
            //var _GSWithUnit = GameSystemUnitJs
            //        .Join(UnitsJs,
            //                gsu => gsu.UnitId,
            //                    u => u.Id,
            //                        (gsu, u) => new { GameSystemUnitSpecificDetail = gsu, Unit = u});

            //this works
            var _GSWithUnit = gameSpecList
                    .Join(unitList,
                        gsu => gsu.UnitId,
                            u => u.Id,
                            (gsu, u) => new { GameSystemUnitSpecificDetail = gsu, Unit = u })
                    .Where(t => t.GameSystemUnitSpecificDetail.RulesetId == rulesetId);


            //https://asusualcoding.wordpress.com/2018/03/10/join-multiple-lists-using-linq-in-c/
            //This doesnt woek, gets stuck in a loop
            //var _GSWithUnit1 = gameSpecList
            //        .Join(unitList, gsu => gsu.UnitId, u => u.Id, (gsu, u) => new { GameSystemUnitSpecificDetail = gsu, Unit = u })
            //        .Join(countryList, gsu => gsu.Unit.CountryId, country => country.Id, (gsu, country) => new { GameSystemUnitSpecificDetail = gsu, Country = country})
            //        .Where(t => t.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.RulesetId == rulesetId);

            //This adds Subunits to the query
            var _includeSubUnit = _GSWithUnit
                    .Join(subUnitList,
                        gsu => gsu.Unit.SubUnitTypeId,
                            su => su.Id,
                                (gsu, su) => new { GameSystemUnitSpecificDetail = gsu, SubUnitType = su });

            //This works based on the above query
            //foreach (var item in _includeSubUnit)
            //{

            //    Console.WriteLine();
            //    Console.WriteLine("SubUnitName = " + item.SubUnitType.SubUnitName.ToString());
            //    Console.WriteLine("subUnitTypeObj = " + item.SubUnitType.ToString());
            //    Console.WriteLine("Name_ClassName = " + item.GameSystemUnitSpecificDetail.Unit.Name_ClassName.ToString());
            //    Console.WriteLine("Ruleset ID = " + item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.RulesetId.ToString());
            //    Console.WriteLine("Image path = " + item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.ImagePath.ToString());
            //}

            //Now need to add countries
            var _includeCountries = _includeSubUnit
                    .Join(countryList,
                        gsu => gsu.GameSystemUnitSpecificDetail.Unit.CountryId,
                            c => c.Id,
                                (gsu, c) => new { GameSystemUnitSpecificDetail = gsu, Country = c });

            

            //foreach (var item in _includeCountries)
            //{

            //    Console.WriteLine();
            //    Console.WriteLine("SubUnitName = " + item.GameSystemUnitSpecificDetail.SubUnitType.SubUnitName.ToString());
            //    Console.WriteLine("subUnitTypeObj = " + item.GameSystemUnitSpecificDetail.SubUnitType.ToString());
            //    Console.WriteLine("Name_ClassName = " + item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.Unit.Name_ClassName.ToString());
            //    Console.WriteLine("Ruleset ID = " + item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.RulesetId.ToString()); ;
            //    Console.WriteLine("Image path = " + item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.ImagePath.ToString());
            //    Console.WriteLine("Country name  = " + item.Country.CountryName.ToString());
            //}



            //TODO: Add in subunit type to this, some how or add another query

            List<UnitForGameSystemDTO> tempList = new List<UnitForGameSystemDTO>();    
            foreach (var item in _includeCountries)
                    {
                        var ShipsSubsInClassCleaned = item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.Unit.ShipsSubsInClass;
                            if (ShipsSubsInClassCleaned != null)
                            {
                                ShipsSubsInClassCleaned = ShipsSubsInClassCleaned.Trim();
                             }
                UnitForGameSystemDTO ug = new UnitForGameSystemDTO(){
                            Id = item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.Unit.Id,
                            Name_ClassName = item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.Unit.Name_ClassName,
                            subUnitTypeObj = item.GameSystemUnitSpecificDetail.SubUnitType,
                            Alliance = await ReturnAllianceName(item.Country.AllianceId),
                            NumberinClass_shipSub = item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.Unit.NumberinClass_shipSub,
                            Countryobj = item.Country,
                           Cost = item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.Cost,

                            ImagePath = item.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.GameSystemUnitSpecificDetail.ImagePath,
                            ShipsSubsInClass = ShipsSubsInClassCleaned,
                            NumberSelected = 0,
                            AllowUnlimitedSelection = false,
                            AddButtonDisabled = false,
                            RemoveButtonDisabled = true,
                            styleForAddButton = Settings.styleForButtonAvailable,
                            styleForRemoveButton = Settings.styleForButonNotAvailable,
                            FilterVisible = false

                        };

                        tempList.Add(ug);
                }



            ////Before returning the data, we have to implement some changes to it based on the onlyReturnUnitsInCollection and rulesetId flags

            ////If using my collection, each ship or sub will be returned as a unique entity, so it can only be selected once

            List<UnitForGameSystemDTO> listToReturn = new List<UnitForGameSystemDTO>();

            foreach (var unit in tempList)
            {
                //Get a list of unique ship and subs if using local collection
                //if (onlyReturnUnitsInCollection == true && (unit.subUnitTypeObj.UnitTypeId == 1 || unit.subUnitTypeObj.UnitTypeId == 3))
                if (onlyReturnUnitsInCollection == true && (unit.subUnitTypeObj.UnitTypeId == 1 || unit.subUnitTypeObj.UnitTypeId == 3) && unit.ShipsSubsInClass != null)
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
                        Console.WriteLine("Adding "+ newUnit.ShipsSubsInClass);

                        listToReturn.Add(newUnit);
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
                    listToReturn.Add(unit);
                }
            }

            List<UnitForGameSystemDTO> SortedList = listToReturn.OrderBy(o => o.subUnitTypeObj.SubUnitName).ToList();

            ServiceResponse<List<UnitForGameSystemDTO>> SrRespone = new ServiceResponse<List<UnitForGameSystemDTO>>()
            {
                //Data = listToReturn
                Data = SortedList
            };

            //TODO: UP TO HERE. Need a bit more processing to create duplicates and set the max amount of ships that can be selected. See below
            return SrRespone;


        }

        private async Task<string> ReturnAllianceName(int allianceID)
        {
            //This could be made an array of this service method to stop the call to the json file
            var alliances = await _http.GetFromJsonAsync<Alliance[]>("Data/alliances.json");

            string name;
            foreach (var item in alliances)
            {
                if(item.Id == allianceID)
                {
                    return item.Name;
                }
            }

            return "Unknown alliance";
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


        public async Task<ServiceResponse<List<OrderCard>>> GetBroadSideOrderCards()
        {
            var result = await _http.GetFromJsonAsync<OrderCard[]>("Data/broadsides_order_cards.json");

            if (result != null && result.Length != 0)
            {

                return new ServiceResponse<List<OrderCard>>
                {
                    Data = result.ToList()
                };
            }
            else
            {
                return new ServiceResponse<List<OrderCard>>
                {
                    Data = new List<OrderCard>(),
                    Success = false,
                    Message = "No orders found"
                };

            }

        }
    }

    class ColorConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(Color));
        }

        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteValue(((Color)value).ToArgb());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return Color.FromArgb(Convert.ToInt32(reader.Value));
        }
    }

}
