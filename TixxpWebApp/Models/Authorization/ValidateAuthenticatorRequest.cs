namespace Tixxp.WebApp.Models.Authorization
{
    public class ValidateAuthenticatorRequest
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }
}
