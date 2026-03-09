namespace PiiSignalRDemo.Models
{
    public class PiiRequest
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Text { get; set; }
        public string TabId { get; set; }
    }
}
