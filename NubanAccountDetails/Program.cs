using NubanAccountDetails.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IGetDetails, GetDetails>();
builder.Services.AddTransient<IPaystack, Paystack>();
builder.Services.AddTransient<IFlutterwave, Flutterwave>();

builder.Services.AddHttpClient();
builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("",builder =>
    builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
}*/
    app.UseSwagger();
    app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseRouting();
app.UseAuthorization();
app.UseCors();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.MapControllers();

app.Run();
