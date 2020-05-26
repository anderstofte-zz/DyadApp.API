using System.Collections.Generic;

namespace DyadApp.API.ViewModels
{
    public class MatchConversationModel
    {
        public int MatchId { get; set; }
        public int UserId { get; set; }
        public int OtherPersonsUserId { get; set; }
        public string Name { get; set; }
        public List<ChatMessageModel> ChatMessages { get; set; }
    }
}