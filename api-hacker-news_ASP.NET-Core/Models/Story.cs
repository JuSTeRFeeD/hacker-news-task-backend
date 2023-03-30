namespace api_hacker_news_ASP.NET_Core.Models
{
    public class Story
    {
        public string By { get; set; }
        public int Descendants { get; set; }
        public uint Id { get; set; }
        public uint Score { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public uint[] Kids { get; set; }
        public uint Time { get; set; }
    }
}