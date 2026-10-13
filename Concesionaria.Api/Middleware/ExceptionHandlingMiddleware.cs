using System.Net;
using FluentValidation; 
using Concesionaria.Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;

namespace TuProyecto.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex) 
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .Select(e => new 
                    { 
                        PropertyName = e.PropertyName, 
                        ErrorMessage = e.ErrorMessage 
                    });

                await context.Response.WriteAsJsonAsync(new { Errors = errors });
            }
            catch (ImageProcessingException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
            }
            catch (FileStorageException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    new { Message = "El almacenamiento de archivos no está disponible." });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unhandled exception processing {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(
                    new { Message = "Ocurrió un error interno en el servidor." });
            }
        }
    }
}
