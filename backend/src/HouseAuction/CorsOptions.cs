namespace HouseAuction
{
    public class CorsOptions
    {
        public List<string> AllowedOrigins { get; set; }

        public CorsOptions()
        {
            AllowedOrigins = new List<string>();
        }
    }
}
