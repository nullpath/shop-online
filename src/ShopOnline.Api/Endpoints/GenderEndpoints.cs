using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Entities;
using ShopOnline.Application.Mapping;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Endpoints;

internal static class GenderEndpoints
{
    const string GetGenderEndpointName = "GetGender";

    internal static void MapGenderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genders");

        group.MapGet("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            var gender = await context.Genders.FindAsync(id);

            return gender is null ? Results.NotFound() : Results.Ok(gender.ToDto());
        })
        .WithName(GetGenderEndpointName);

        group.MapGet("/", async (ShopDatabaseContext context)
            => await context.Genders
                .AsNoTracking()
                .Select(gender => gender.ToDto())
                .ToListAsync());

        group.MapDelete("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            await context.Genders
                .Where(gender => gender.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/", async (CreateGenderDto dto, ShopDatabaseContext context) =>
        {
            Gender gender = new()
            {
                Name = dto.Name
            };

            context.Genders.Add(gender);
            await context.SaveChangesAsync();

            GenderDto genderDto = gender.ToDto();

            return Results.CreatedAtRoute(GetGenderEndpointName, new { id = genderDto.Id }, genderDto);
        });

        group.MapPut("/{id}", async (int id, UpdateGenderDto dto, ShopDatabaseContext context) =>
        {
            var gender = await context.Genders.FindAsync(id);

            if (gender is null)
                return Results.NotFound();

            gender.Name = dto.Name;
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
