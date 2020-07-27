using System;
using System.Net;
using System.Threading.Tasks;
using GoodFood.Recipes.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GoodFood.Recipes.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> logger;
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            this.logger = logger;
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this.next(context);
            }
            catch (RestException ex)
            {
                logger.LogError(ex, "REST ERROR");
                context.Response.StatusCode = (int)ex.Code;

                await WriteErrorResponseAsync(ex.Errors).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "SERVER ERROR");
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await WriteErrorResponseAsync(ex.Message).ConfigureAwait(false);
            }

            async Task WriteErrorResponseAsync(object errors)
            {
                context.Response.ContentType = "application/json";
                if (errors != null)
                {
                    var result = JsonConvert.SerializeObject(
                        new
                        {
                            errors
                        }
                    );

                    await context.Response.WriteAsync(result).ConfigureAwait(false);
                }
            }
        }
    }
}