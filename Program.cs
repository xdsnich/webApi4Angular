var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProfileDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddScoped<IProfileInterface, ProfileCRUD>();

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAngularClient",
        policy => {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.ContentSecurityPolicy = "default-src *; script-src *; style-src *; img-src *; connect-src *;";
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<ProfileDb>();
    db.Database.EnsureCreated();
}

app.UseCors("AllowAngularClient");

app.MapGet("/profile", async (IProfileInterface profile) => Results.Ok(await profile.GetProfileAsync()))
    .Produces<List<Profile>>(StatusCodes.Status200OK)
    .WithName("GetAllProfiles")
    .WithTags("Getters");

app.MapGet("/profile/{id}", async (IProfileInterface profile, int id) => await profile.GetProfileAsync(id) is Profile profile1
    ? Results.Ok(profile1)
    : Results.NotFound())
    .Produces<List<Profile>>(StatusCodes.Status200OK)
    .WithName("GetAllHotels")
    .WithTags("Getters");

app.MapPost("/profile", async ([FromBody] Profile profileBody, IProfileInterface profile, HttpResponse response) => {
    await profile.InsertProfileAsync(profileBody);
    await profile.SaveAsync();
    return Results.Created($"/profile/{profileBody.id}", profileBody);
})
    .WithName("CreateProfile")
    .WithTags("Creaters");

app.MapPut("/profile/{id}", async ([FromBody] Profile profile1, IProfileInterface profile, int id) => {
    await profile.UpdateProfileAsync(profile1, id);
    await profile.SaveAsync();
    return Results.NoContent();
})
    .WithName("UpdateProfile")
    .WithTags("Updaters");

app.MapDelete("/profile/{id}", async (IProfileInterface profile, int id) => {
    await profile.DeleteProfileAsync(id);
    await profile.SaveAsync();
})
    .WithName("DeleteProfile")
    .WithTags("Deleters");

app.UseHttpsRedirection();
app.Run();
