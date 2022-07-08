using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core;
using System.Diagnostics;
using System.Text.Json;

namespace NLayer.API
{
    public static class UseCustomExceptionHandler
    {
        public static void UseCustomException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(config =>
            {
                //Run Devre KEsici Middlawera
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    //IExceptionHandlerFeature üzerinden uygulamada fırlatılan hataları yakalıyoruz.
                    IExceptionHandlerFeature exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    //exceptionFeature hatalarında eger client taraflı bir hata ise 400,NoFoundException ise 404, bunun dışında bir hata işe 500 atıyoruz.
                    int statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };

                    // context.Response.StatusCode a yukarıdan gelen statuscode u atıyoruz.
                    context.Response.StatusCode = statusCode;

                    
                    CustomResponseDto<NoContentDto> response;
                    if (statusCode==404)
                    {
                         response = CustomResponseDto<NoContentDto>.Fail(statusCode, "An Error Occurred");
                    }
                    //Kendi yazdıgımız CustomResponseDto nun fail methoduna statuscode ve error u veruyoruz.
                    response = CustomResponseDto<NoContentDto>.Fail(statusCode, exceptionFeature.Error.Message);


                    // Response u WriteAsync ile yazdırıyoruz. JsonSerializer.Serialize(response) ile response u json dizesine dönüştürdük.
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response)); 
               });
            });
        }
    }
}
