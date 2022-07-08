using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayer.Core;

namespace NLayer.API
{
    //Yazdıgımız filter sayesinde Controllerda action method çalişmadan id yakalayıp kontrol ediyoruz.
    public class NotFoundFilter<T> : IAsyncActionFilter where T : BaseEntity
    {
        private readonly IService<T> _service;

        public NotFoundFilter(IService<T> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //context.ActionArguments.Values.FirstOrDefault() methodun ilk Argumani alıyoruz.
            object idValue = context.ActionArguments.Values.FirstOrDefault();

            if (idValue == null)
            {
                await next.Invoke();
                return;
            }

            int id = (int)idValue;

            //Bu id Sahip Entity varmı kontrol ediyoruz.
            bool anyEntity = await _service.AnyAsync(x=>x.Id==id);

            if (anyEntity)
            {
                await next.Invoke();
                return;
            }

            //contex.Result a NotfoundObjectResult un parametsine  CustomResponseDto<NoContentDto>.fail() veriyoruz.
            context.Result = new NotFoundObjectResult(CustomResponseDto<NoContentDto>.Fail(404, $"{typeof(T).Name}({id}) Not Found"));
        }
    }
}
