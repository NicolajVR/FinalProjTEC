
using skolesystem.Controllers;
using skolesystem.Repository;

namespace skolesystem.Authorization
{
    // Middleware til behandling af JWT-autorisation
    public class JwtMiddleware
    {
        // Delegeret til det næste middelvare i pipeline
        private readonly RequestDelegate _next;

        // Konstruktør, der modtager det næste middleware
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Metode, der eksekveres, når middleware er aktiveret
        public async Task Invoke(HttpContext context, IUsersRepository userRepository, IJwtUtils jwtUtils)
        {
            // Henter JWT-token fra autorisationshovedet i HTTP-anmodningen
            string token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            // Validerer JWT-token og henter brugerens ID
            int? userId = jwtUtils.ValidateJwtToken(token);

            if (userId is not null)
            {
                // Ved succesfuld validering vedhæftes brugeren til context
                var user = await userRepository.GetById(userId.Value);
                context.Items["User"] = UserController.MapUserTouserResponse(user);
            }

            // Kalder det næste middleware i pipeline
            await _next(context);
        }
    }
}