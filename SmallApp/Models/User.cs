namespace SmallApp.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();

        public SendUser ToSend()
        {
            return new SendUser(Id, Name, Comments);
        }
    }
    

    // SendUser is needed for send only needed information and to hide private data e.g. passwords
    public record struct SendUser(long Id, string Name, List<Comment> Comments);
}
