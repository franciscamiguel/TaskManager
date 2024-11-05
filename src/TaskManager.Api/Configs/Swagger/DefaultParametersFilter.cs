using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskManager.Api.Configs.Swagger;

public class DefaultParametersFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (parameter is null) return;

        parameter.Description ??= context.ApiParameterDescription.ModelMetadata.Description;

        if (context.ApiParameterDescription.RouteInfo != null)
            parameter.Required |= !context.ApiParameterDescription.RouteInfo.IsOptional;
    }
}