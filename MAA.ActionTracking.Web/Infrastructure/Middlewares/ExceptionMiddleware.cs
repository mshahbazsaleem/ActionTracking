using MAA.ActionTracking.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MAA.ActionTracking.Web.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {

            //try
            //{
                await _next(httpContext);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError($"An error has occurred: {ex}");
            //    await HandleExceptionAsync(httpContext, ex);
            //}
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
          
            if (context.Request.Headers.ContainsKey("x-requester") && context.Request.Headers["x-requester"].ToString().Equals("Datatable"))
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                ApiException ex = exception as ApiException;
                var error  = JsonConvert.SerializeObject(new GridDataViewModel<object>
                {
                    draw = 1,
                    data = null,
                    recordsFiltered = 0,
                    recordsTotal = 0,
                    error = new ErrorViewModel()
                    {
                        ResponseText = ex.Content,
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = exception.Message

                    }
                });

                return context.Response.WriteAsync(error);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                ValidationApiException ex = exception as ValidationApiException;
                return context.Response.WriteAsync(new ErrorViewModel()
                {
                    ResponseText = ex != null ? JsonConvert.SerializeObject(ex.Content) : exception.Message,
                    StatusCode = context.Response.StatusCode,
                    Message = exception.Message

                }.ToString());
            }
        }
    }
}
