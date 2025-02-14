using IMS.Plugins.InMemory;
using IMS.UseCases.Inventories;
using IMS.UseCases.Inventories.Interfaces;
using IMS.UseCases.PluginInterfaces;
using IMS.WebApp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents();

// This is where we register the dependencies inside the dependency injection container (which is the service collection)
// We use different methods (Singleton, Transient, etc.) to indicate our DI lifetime.

// For this method, AddSingleton, means that the first time that this object is required it will create it. Than, that info will be
// stored within the application. Which means, this object will leave as long as the application or until it is disposed. Which means
// the next time the application needs InventoryRepository, it won't need to be created again.
builder.Services.AddSingleton<IInventoryRepository, InventoryRepository>();
// For this method, AddTransient, this object is created everytime when the program requires it. Whether it's a class or razor
// component that needs the usecase, it's going to create a new object.
builder.Services.AddTransient<IViewInventoriesByNameUseCase, ViewProductsByNameUseCase>();
builder.Services.AddTransient<IAddInventoryUseCase, AddInventoryUseCase>();
builder.Services.AddTransient<IEditInventoryUseCase, EditInventoryUseCase>();
builder.Services.AddTransient<IViewInventoryByIdUseCase, ViewInventoryByIdUseCase>();
builder.Services.AddTransient<IDeleteInventoryUseCase, DeleteInventoryUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>();

app.Run();
