namespace DyadApp.API.ViewModels
{
    public class NewChatMessageModel
    {
        public int MatchId { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}