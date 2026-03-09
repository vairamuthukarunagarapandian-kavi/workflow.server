namespace PiiSignalRDemo.Models
{
    public class PiiResult
    {
        public string Id { get; set; }
        public bool ContainsPII { get; set; }
        public string Message { get; set; }
        public string RawPrompt { get; set; }
    }
}
