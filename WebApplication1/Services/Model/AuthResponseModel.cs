namespace WebApplication1.Services.Model
{
    public class AuthResponseModel
    {
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

    }
}
