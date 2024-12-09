
using Microsoft.JSInterop;
using Newtonsoft.Json;
using ShipSelector.Shared;
using System.Net.Http.Headers;
using System.Net.Http.Json;
//using Newtonsoft.Json;
using System.Web;

namespace ShipSelector.Services.UploadDownloadService
{
    public class UploadDownloadServiceClient : IUploadDownloadServiceClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<UploadDownloadServiceClient> _logger;

        public UploadDownloadServiceClient(HttpClient httpClient, ILogger<UploadDownloadServiceClient> logger)
        {
            _http = httpClient;
            _logger = logger;
        }

        public async Task<ServiceResponse<Stream>> GetFileAsStream(UnitForGameSystemDTO unit, int ruleSetId)
        {

            //TODO: copy the file proces sfrom the server code
            return new ServiceResponse<Stream>
            {
                Success = false,
                Message = "NOT implemented yet"
            };

            var pathToURL = Path.Combine("api/Filesave/GetFileAsStream/", unit.ImagePath, ruleSetId.ToString(), unit.Countryobj.Id.ToString());

            var imageStreamRes = await _http.GetAsync(pathToURL);


            if (imageStreamRes is not null)
            {
                return new ServiceResponse<Stream>
                {
                    Data = imageStreamRes.Content.ReadAsStream()
                };
            }
            else
            {
                return new ServiceResponse<Stream>
                {
                    Success = false,
                    Message = "Some issue getting file"
                };
            }
        }

        //public async Task<ServiceResponse<List<bool>>> DeleteFilesFromFileSystem(List<FileDetail> filesToDelete)

        //{
        //    //var response = await _http.DeleteAsync("api/Filesave/deleteFilesFromServer");
        //    //Had to use a Put to do a batch delete. See controller for more details
        //    var response = await _http.PutAsJsonAsync("api/Filesave/deleteFilesFromServer", filesToDelete);

        //    ServiceResponse<List<bool>> newFileDeleteResults = new ServiceResponse<List<bool>>();
        //    if (response != null & response.IsSuccessStatusCode)
        //    {
        //        newFileDeleteResults = await response.Content.ReadFromJsonAsync<ServiceResponse<List<bool>>>();
        //    }

        //    if (newFileDeleteResults != null)
        //    {
        //        return newFileDeleteResults;
        //    }
        //    else
        //    {
        //        //the response wasnt valid, need to pass back a failed response
        //        return new ServiceResponse<List<bool>>
        //        {
        //            Data = new List<bool>(),
        //            Success = false,
        //            Message = "The response from th server was not valid. Not known if files removed"
        //        };
        //    }

        //}

        public async Task<ServiceResponse<List<UploadResult>>> UploadFiles(List<FileUploadDTO> e, int rulesetId, int countryId)
        {
            //This task uses a list of broswerFiles. It is not tirggered as soon as the dialog box is closed, but later in the process
            List<File> files = new List<File>();
            long maxFileSize = Settings.maxFileSize;

            bool upload = false;
            List<UploadResult> uploadResults = new();

            _logger.LogInformation("Uploading files from UploadDownloadServiceClient");
            using var content = new MultipartFormDataContent();
            foreach (var file in e)
            {
                try
                {
                    files.Add(new() { Name = file.FileName });
                    var fileData = file.FileContent;
                    ByteArrayContent byteContent = new ByteArrayContent(fileData);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);
                    content.Add(content: byteContent, name: "\"files\"", fileName: file.FileName);

                    upload = true;

                    _logger.LogInformation("\"files\"");

                }
                catch (Exception ex)
                {
                    _logger.LogInformation("{FileName} not uploaded (Err: 6): {Message}", file.FileName, ex.Message);

                    uploadResults.Add(
                        new()
                        {
                            FileName = file.FileName,
                            ErrorCode = 6,
                            Uploaded = false
                        });
                }

            }



            _logger.LogInformation("upload variable = " + upload);


            //TODO : This all needs to be checked
            if (upload)
            {


                _logger.LogInformation("...about to call FileSave controller. Content = " + JsonConvert.SerializeObject(content));
                HttpResponseMessage response = new HttpResponseMessage();
                bool proceedAfterPost = false;
                try
                {
                    //response = await _http.PostAsync("api/Filesave", content);
                    response = await _http.PostAsync("api/Filesave/FileSaveWithParams/" + rulesetId + "/" + countryId, content);
                    proceedAfterPost = true;
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("...exception thrown = " + ex.Message);
                }

                //var response = await _http.PostAsync("api/Filesave", content);
                if (response.IsSuccessStatusCode == false | proceedAfterPost == false)
                {
                    _logger.LogInformation("{FileName} could not reach FileSave endpoint (Err: 7)");
                }
                else
                {
                    _logger.LogInformation("Got a response from FileSaveController");
                    var newUploadResults = await response.Content.ReadFromJsonAsync<ServiceResponse<List<UploadResult>>>();

                    if (newUploadResults is not null)
                    {
                        return newUploadResults;
                    }
                    else
                    {
                        return new ServiceResponse<List<UploadResult>>
                        {
                            Success = false,
                            Message = "Null response from service on server"
                        };
                    }
                }
            }

            return new ServiceResponse<List<UploadResult>>
            {
                Success = false,
                Message = "Some issue creating file content to pass to server"
            };
        }
    }

    public class File
    {
        public string? Name { get; set; }
    }
}
