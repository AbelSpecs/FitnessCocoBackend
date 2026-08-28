namespace InsightCore.Infrastructure.Options
{
    public class R2Settings
    {
        // Cloudflare R2 service URL. Some configs use 'ServiceUrl' while others use 'Endpoint'.
        // Bindings are case-insensitive; accept both names for compatibility.
        public string? Endpoint { get; set; }
        public string? ServiceUrl { get; set; }
        public string? AccessKeyId { get; set; }
        public string? SecretAccessKey { get; set; }
        public string? BucketName { get; set; }
        public string? Region { get; set; }
        // Optional token if used by some setups (not normally required for R2)
        public string? Token { get; set; }
    }
}
