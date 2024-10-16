﻿using System.Net;
using System.Text.Json;

namespace _04_Inventory.Api.Middlewares
{
    public class ExceptionHandLingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandLingMiddleware> _logger;

        public ExceptionHandLingMiddleware(RequestDelegate next, ILogger<ExceptionHandLingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new DTOS.MessageResponseDTO();
            switch (exception)
            {
                case ApplicationException ex:
                    if (ex.Message.Contains("Invalid token"))
                    {
                        response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        errorResponse.Message = null != exception.InnerException ? exception.InnerException.Message : exception.Message;
                        break;
                    }
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = null != exception.InnerException ? exception.InnerException.Message : exception.Message;
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = null != exception.InnerException ? exception.InnerException.Message : exception.Message;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = null != exception.InnerException ? exception.InnerException.Message : exception.Message;
                    break;
            }
            _logger.LogError(string.Format("HttpStatusCode : {0} Error: {1}", response.StatusCode, null != exception.InnerException ? exception.InnerException.Message : exception.Message));
            var errorResponseArr = errorResponse.Message.Split(":;");
            errorResponse.Message = errorResponseArr[0];
            //errorResponse.Type = MessagesResponseTypes.Danger.Key;
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }

    }

}
