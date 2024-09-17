using UserManager.Extensions;
using UserManager.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddContext(builder.Configuration);  
builder.Services.AddIdentity();  
builder.Services.AddMapper();  
builder.Services.AddServices();
builder.Services.AddRazorPages();
builder.Services.AddMiddlewares();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CheckUserLockoutMiddleware>();

app.MapRazorPages();

app.Run();