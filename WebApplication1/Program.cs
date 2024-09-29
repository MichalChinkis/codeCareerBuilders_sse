var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapGet("/time", async context =>
{
    context.Response.Headers.Add("Content-Type", "text/event-stream");
    context.Response.Headers.Add("Cache-Control", "no-cache");
    context.Response.Headers.Add("Connection", "keep-alive");

    while (true)
    {
        var currentTime = DateTime.Now.ToString("HH:mm:ss");
        await context.Response.WriteAsync($"data: {currentTime}\n\n");
        await context.Response.Body.FlushAsync();  // דחיפת המידע לדפדפן
        await Task.Delay(1000); // לחכות שנייה בין הודעות
    }
});

app.UseStaticFiles(); // כדי שנוכל להגיש את ה-HTML מ-wwwroot

app.Run();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
