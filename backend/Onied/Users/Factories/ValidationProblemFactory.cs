using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Users.Factories;

public class ValidationProblemFactory
{
    public static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription)
    {
        return TypedResults.ValidationProblem(new Dictionary<string, string[]>
        {
            { errorCode, [errorDescription] }
        });
    }

    public static ValidationProblem CreateValidationProblem(IdentityResult result)
    {
        // We expect a single error code and description in the normal case.
        // This could be golfed with GroupBy and ToDictionary, but perf! :P
        Debug.Assert(!result.Succeeded);
        var errorDictionary = new Dictionary<string, string[]>(1);

        foreach (var error in result.Errors)
        {
            string[] newDescriptions;

            if (errorDictionary.TryGetValue(error.Code, out var descriptions))
            {
                newDescriptions = new string[descriptions.Length + 1];
                Array.Copy(descriptions, newDescriptions, descriptions.Length);
                newDescriptions[descriptions.Length] = error.Description;
            }
            else
            {
                newDescriptions = [error.Description];
            }

            errorDictionary[error.Code] = newDescriptions;
        }

        return TypedResults.ValidationProblem(errorDictionary);
    }
}
