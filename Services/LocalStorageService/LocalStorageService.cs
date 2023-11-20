using ShipSelector.Models;
using Blazored.LocalStorage;

namespace ShipSelector.Services.LocalStorageService
{
    public class StorageService : IStorageService
    {
        private readonly ILocalStorageService _localStorage;

        public StorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }


        public async Task<ServiceResponse<int>> AddUnitsToStorage(List<UnitForGameSystemDTO> units)
        {
            var exsitItems = await _localStorage.GetItemAsync<List<UnitForGameSystemDTO>>("units");

            ServiceResponse<bool> LocStorageCleared = new ServiceResponse<bool>();
            if (exsitItems != null)
            {
                //have to remove the items from the storage service
                LocStorageCleared = await RemoveAllUnitsFromStorage();
            }

            if(LocStorageCleared.Success == true)
            {
                //Only saving the units selected by the units
                List<UnitForGameSystemDTO> newUnits = new List<UnitForGameSystemDTO>();

                newUnits = units
                    .FindAll(x => x.NumberSelected > 0)
                    .ToList();

                await _localStorage.SetItemAsync("units", newUnits);

                var countOfUnitsInStorage = await GetCountOfUnitsStored();

                if(countOfUnitsInStorage.Data > 0)
                {
                    return new ServiceResponse<int>
                    {
                        Data = countOfUnitsInStorage.Data,
                    };
                }
                else
                {
                    return new ServiceResponse<int>
                    {
                        Data = 0,
                        Message ="Items not added to storage",
                        Success = false
                    };
                }

            }
            else
            {
                //storage wasnt deleted. Dont write the values
                return new ServiceResponse<int>
                {
                    Data = 0,
                    Success = false,
                    Message = "Couldnt remove existing units stored in storage"
                };
            }

        }

        public async Task<ServiceResponse<bool>> RemoveAllUnitsFromStorage()
        {
            var units = await _localStorage.GetItemAsync<List<UnitForGameSystemDTO>>("units");

            if (units == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = true,
                };
            }
            else
            {
                await _localStorage.ClearAsync();

                //Checking to make sure the clear worked
                var unitsLeft = await _localStorage.GetItemAsync<List<UnitForGameSystemDTO >>("units");
                if (unitsLeft == null)
                {
                    return new ServiceResponse<bool>
                    {
                        Data = true,
                    };
                }
                else
                {
                    return new ServiceResponse<bool>
                    {
                        Data = false,
                        Success = false,
                        Message = "Couldnt clear the storage"

                    };

                }

            }
        }

        public async Task<ServiceResponse<int>> GetCountOfUnitsStored()
        {
            var units = await _localStorage.GetItemAsync<List<UnitForGameSystemDTO>>("units");
            if (units != null)
            {
                return new ServiceResponse<int> { Data = units.Count };
            }
            else
            {
                return new ServiceResponse<int>
                {
                    Data = 0,
                    Message = "Nothing found",
                    Success = false
                };
            }
        }

        public async Task<ServiceResponse<List<UnitForGameSystemDTO>>> RetrieveAllUnits()
        {
            var units = await _localStorage.GetItemAsync<List<UnitForGameSystemDTO>>("units");
            if (units == null)
            {
                return new ServiceResponse<List<UnitForGameSystemDTO>>
                {
                    Data = new List<UnitForGameSystemDTO>(),
                    Success = false,
                    Message = "No units found"
                };
            }
            else
            {
                return new ServiceResponse<List<UnitForGameSystemDTO>>
                {
                    Data = units
                };

            }

        }
    }
}
