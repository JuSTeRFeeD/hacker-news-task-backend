namespace api_hacker_news_ASP.NET_Core.Models
{
    public class Comment
    {
        public string By { get; set; }
        public int Id { get; set; }
        public int[] Kids { get; set; }
        public int Parent { get; set; }
        public string Text { get; set; }
        public int Time { get; set; }
        public string Type { get; set; }
    }
}