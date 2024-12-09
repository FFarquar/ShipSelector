using Microsoft.JSInterop;
using ShipSelector.Models;
using ShipSelector.Shared;
namespace ShipSelector.Services.UploadDownloadService
{
    public interface IUploadDownloadServiceClient
    {

        Task<ServiceResponse<Stream>> GetFileAsStream(UnitForGameSystemDTO unit, int rulesetId);

        //Task<ServiceResponse<List<bool>>> DeleteFilesFromFileSystem(List<FileDetail> filesToDelete);

        Task<ServiceResponse<List<UploadResult>>> UploadFiles(List<FileUploadDTO> e, int rulesetId, int countryId);
    }
}
