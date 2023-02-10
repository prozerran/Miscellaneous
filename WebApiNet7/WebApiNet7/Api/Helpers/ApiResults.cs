using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApiNet7.Api.Helpers
{
    public static class ApiResults
    {
        public static ProblemHttpResult Problem(string title, string detail)
        {
            return TypedResults.Problem(title: title, detail: detail, statusCode: 400);
        }
    }
}
