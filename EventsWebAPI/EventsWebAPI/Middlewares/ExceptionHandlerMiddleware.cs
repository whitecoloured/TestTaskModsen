using EventsWebAPI.Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventsWebAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                switch (e)
                {
                    case NotFoundException ex:
                        await HandleAsync(context, HttpStatusCode.NotFound, ex.message);
                        break;
                    case BadRequestException ex:
                        await HandleAsync(context, HttpStatusCode.BadRequest, ex.message);
                        break;
                    case NotAcceptableException ex:
                        await HandleAsync(context, HttpStatusCode.NotAcceptable, ex.message);
                        break;
                    default:
                        await HandleAsync(context, HttpStatusCode.InternalServerError, e.Message);
                        break;

                }
            }
        }

        private async Task HandleAsync(HttpContext context, HttpStatusCode code, string message)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";
            response.StatusCode = (int)code;
            string jsonErrorResponse = JsonSerializer.Serialize(new { StatusCode = response.StatusCode, Type = code.ToString(), Message = message });

            await response.WriteAsync(jsonErrorResponse);
        }
    }
}
