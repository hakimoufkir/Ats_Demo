using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Ats_Demo.Application.Exceptions; // Import your custom exceptions

namespace Ats_Demo.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                EmployeeNotFoundException => (int)HttpStatusCode.NotFound,
                KeyNotFoundException => (int)HttpStatusCode.NotFound,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                EmployeeUpdateException => (int)HttpStatusCode.BadRequest,
                EmployeeCreationException => (int)HttpStatusCode.BadRequest,
                _ => (int)HttpStatusCode.InternalServerError // Default case for unknown exceptions
            };

            var response = new
            {
                Success = false,
                StatusCode = statusCode,
                Message = exception.Message
            };

            var jsonResponse = JsonConvert.SerializeObject(response);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(jsonResponse);
        }
    }
}