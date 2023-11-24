using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using skolesystem.DTOs;

namespace skolesystem.Authorization
{
    // Brugerdefineret autorisationsattribut, der kan anvendes på klasser eller metoder
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        // Roller, der har adgang til den pågældende ressource eller handling
        private readonly int[] _roles = { 1, 2, 3 };

        // Konstruktør, der tillader at specificere roller ved oprettelse af attributten
        public AuthorizeAttribute(params int[] roles)
        {
            _roles = roles ?? Array.Empty<int>();
        }

        // Metode kaldet under autorisationsprocessen
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Kontrollerer, om handlingen er markeret som AllowAnonymous og undlader autorisationslogik i så fald
            bool allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
            {
                return;
            }

            // Autorisationslogik
            UserReadDto user = (UserReadDto)context.HttpContext.Items["User"];

            if (user == null || (_roles.Any() && !_roles.Contains(user.role_id)))
            {
                // Returnerer en uautoriseret fejl, hvis brugeren ikke har de nødvendige roller
                context.Result = new JsonResult(new { message = "Uautoriseret" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}