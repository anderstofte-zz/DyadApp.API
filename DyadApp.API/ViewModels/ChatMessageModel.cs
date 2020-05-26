using System;

namespace DyadApp.API.ViewModels
{
    public class ChatMessageModel
    {
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime Sent { get; set; }
    }
}