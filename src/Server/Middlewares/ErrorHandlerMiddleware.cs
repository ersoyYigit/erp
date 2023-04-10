using ArdaManager.Application.Exceptions;
using ArdaManager.Shared.Wrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace ArdaManager.Server.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                _logger.LogError(error, "An unhandled exception occurred while processing the request.");

                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = await Result<string>.FailAsync(error.Message);

                switch (error)
                {
                    case ApiException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        _logger.LogInformation(e, "ApiException occurred.");
                        break;

                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        _logger.LogInformation(e, "KeyNotFoundException occurred.");
                        break;

                    case UnauthorizedAccessException e:
                        // unauthorized access error
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        _logger.LogInformation(e, "UnauthorizedAccessException occurred.");
                        break;

                    case InvalidOperationException e:
                        // invalid operation error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        _logger.LogInformation(e, "InvalidOperationException occurred.");
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogError(error, "Unhandled exception occurred.");
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}