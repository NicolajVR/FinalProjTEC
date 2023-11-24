namespace skolesystem.Authorization
{
    public class AppSettings
    {
        // Hemmelig nøgle, der bruges til at generere og validere JWT-token
        public string Secret { get; set; }
    }
}