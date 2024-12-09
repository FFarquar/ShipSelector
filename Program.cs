global using ShipSelector.Models;
global using ShipSelector.Pages;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShipSelector;
using ShipSelector.Services.LocalStorageService;
using ShipSelector.Services.UnitsandListsServiceClient;
using ShipSelector.Services.UploadDownloadService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IUnitsandListsServiceClient, UnitsandListsServiceClient>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IUploadDownloadServiceClient, UploadDownloadServiceClient>();

if (builder.HostEnvironment.IsDevelopment())
{
    Console.WriteLine("In development");
}
await builder.Build().RunAsync();


