namespace PM_Application.Authorization
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int TokenValidityMins { get; set; }
    }
}
