using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BARQ.API.Swagger
{
    public class StandardResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Responses == null)
            {
                operation.Responses = new OpenApiResponses();
            }

            void Ensure(string code, string description)
            {
                if (!operation.Responses.ContainsKey(code))
                {
                    operation.Responses.Add(code, new OpenApiResponse { Description = description });
                }
            }

            Ensure("400", "Bad Request");
            Ensure("401", "Unauthorized");
            Ensure("403", "Forbidden");
            Ensure("404", "Not Found");
            Ensure("415", "Unsupported Media Type");
            Ensure("500", "Internal Server Error");

            if (!operation.Responses.ContainsKey("default"))
            {
                operation.Responses.Add("default", new OpenApiResponse { Description = "Unexpected error" });
            }
        }
    }
}
