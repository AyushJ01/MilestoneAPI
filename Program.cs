var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


////Add GoogleAuthentication
//builder.Services.AddAuthentication().AddGoogle(GoogleOptions => {
//    GoogleOptions.ClientId = configuration["Authentication:Google:ClientId"];
//    GoogleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
//});

builder.Services.AddSingleton<Dictionary<int, string>>();
builder.Services.AddSingleton<Dictionary<string, string>>();

//builder.Services.AddDbContext<ToDoContext>(options =>
//  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(o =>
   o.AddPolicy(
    "ReactPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    }
    ));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("ReactPolicy");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapControllerRoute(
   name: "default",
    pattern: "{controller}/{action}/{id?}"
    );


app.Run();
