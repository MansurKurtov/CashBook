using AvastInfrastructureRepository.ResponseCoreData.Enums;
using AvastInfrastructureRepository.ResponseCoreData.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace AuthService.Jwt
{
    public class CustomAuthorizeAttribute : TypeFilterAttribute
    {
        public CustomAuthorizeAttribute(string permission)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { permission };
        }
    }

    public class AuthorizeActionFilter : IAsyncActionFilter
    {
        private readonly string _permission;
        public AuthorizeActionFilter(string permission)
        {
            _permission = permission;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            //var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type.ToLower().Contains(modul));

            string permission_list = "";
            if (context.HttpContext.User.FindFirst("Permissions") != null)
                permission_list = context.HttpContext.User.FindFirst("Permissions").Value;

            var hasClaim = permission_list.Contains(_permission);

            //   hasClaim = true; // vaqtincha formalarni yasab bo`lgandan keyin o`chiriladi

            if (!hasClaim)
            {
                context.HttpContext.Response.StatusCode = 401;
                var result = new ResponseCoreData(ResponseStatusCode.Unauthorized);
                context.Result = new ObjectResult(result);
            }
            else
            {
                await next();
            }

        }
    }
}
