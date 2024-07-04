namespace SmallApp.Models
{
    public class Comment
    {
        // this class represents any additional information about user
        public long Id { get; set; }
        public long UserId { get; set; }
        public string Text { get; set; }

        public SendComment ToSend()
        {
            return new SendComment(Id, UserId, Text);
        }
    }

    // same as SendUser - can be used to hide private data
    public record struct SendComment(long Id, long UserId, string Text);
}
