namespace BARQ.API.Options
{
    public class AuthCookieOptions
    {
        public bool Enabled { get; set; } = false;
        public string Name { get; set; } = "__Host-Auth";
        public string? Domain { get; set; }
        public string Path { get; set; } = "/";
        public string SameSite { get; set; } = "None";
        public bool Secure { get; set; } = true;
    }
}
