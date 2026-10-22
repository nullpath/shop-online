using ErrorOr;

namespace ShopOnline.Api.Extensions;

internal static class ErrorOrExtensions
{
    internal static IResult ToProblem(this List<Error> errors)
    {
        if (errors.Any(error => error.Type == ErrorType.NotFound))
            return Results.NotFound();

        if (errors.All(error => error.Type == ErrorType.Validation))
            return Results.ValidationProblem(errors
                .GroupBy(error => error.Code)
                .ToDictionary(group => group.Key, group => group.Select(error => error.Description).ToArray()));

        return Results.Problem();
    }
}
