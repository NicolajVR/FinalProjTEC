namespace skolesystem.Authorization
{
    // Custom attribut brugt til at angive, at en metode tillader anonym adgang
    [AttributeUsage(AttributeTargets.Method)]
    public class AllowAnonymousAttribute : Attribute { }
}