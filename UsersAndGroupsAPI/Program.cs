using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersAndGroupsAPI.Db;
using UsersAndGroupsAPI.Interfaces;
using UsersAndGroupsAPI.Models;
using UsersAndGroupsAPI.Services;
using UsersAndGroupsAPI.Validators;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UsersAndGroups") ?? "Data Source=UsersAndGroups.db";
builder.Services.AddSqlite<UsersAndGroupsContext>(connectionString);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGroupService, GroupService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/groups", async (IGroupService groupService) =>
    await groupService.GetAllGroups());


app.MapGet("/groups/{id}", async (int id, IGroupService groupService) =>
       await groupService.GetGroupById(id)
        is Group group
            ? Results.Ok(group)
            : Results.NotFound());

app.MapPost("/groups", async ([FromBody] CreateGroupRequest req, IGroupService groupService) =>
{
    var newGroup = await groupService.CreateNewGroup(req);

    return Results.Created($"/groups/{newGroup?.Id}", newGroup);
});

app.MapGet("/users", async (IUserService userService) =>
    await userService.GetUsers());

app.MapGet("/users/{id}", async (int id, IUserService userService) =>
    await userService.GetUser(id)
        is UserModel user
            ? Results.Ok(user)
            : Results.NotFound());

app.MapPost("/users", async (Validated<CreateUserRequest> req, IUserService userService, IGroupService groupService) =>
{
    // deconstruct to bool & CreateUserRequest
    if (req.IsValid)
    {
        if (groupService.GetGroupById(req.Value.GroupId) != null)
        {
            try
            {
                var newUser = await userService.CreateNewUser(req.Value);
                if (newUser == null || newUser.Id == 0)
                {
                    return Results.BadRequest($"User can belong to only one grup");
                }
                return Results.Created($"/users/{newUser?.Id}", newUser);

            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
        return Results.BadRequest($"There is no group with given id: {req.Value.GroupId}");
    }
    return Results.ValidationProblem(req.Errors);

});

app.MapPost("/users/addusertogroup", async ([FromBody] AddUserToGroupRequest req, IUserService userService) =>
{   
   var userToGroup = await userService.AddUserToGroup(req.UserId, req.GroupId);
   var user = await userService.GetUser(req.UserId);

    if (userToGroup != null)
    {
        return Results.Ok();

    }
    else
    {
        return Results.BadRequest($"User with id {req.UserId} already belongs to group  {user.Group}");
    }
});

app.Run();




