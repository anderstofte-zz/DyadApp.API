using DyadApp.API.Models;
using System.Collections.Generic;
using System.Linq;
using DyadApp.API.Data.Repositories;
using Xunit;
using Moq;
using Match = DyadApp.API.Models.Match;

namespace DyadApp.Tests
{
    public class ChatTests
    {
        //[Fact]
        //public void Mark_messages_as_read()
        //{
        //    var match = new Match
        //    {
        //        ChatMessages = new List<ChatMessage>
        //        {
        //            new ChatMessage
        //            {
        //                ReceiverId = 2,
        //                IsRead = true,
        //                Message = "Hej :)"
        //            },
        //            new ChatMessage
        //            {
        //                ReceiverId = 1,
        //                IsRead = true,
        //                Message = "heyy :)"
        //            },
        //            new ChatMessage
        //            {
        //                ReceiverId = 1,
        //                IsRead = true,
        //                Message = "Fortæl mig om dig selv"
        //            },
        //            new ChatMessage
        //            {
        //                ReceiverId = 2,
        //                IsRead = true,
        //                Message = "Jeg elsker at gå lange ture bla bla bla."
        //            },
        //            new ChatMessage
        //            {
        //                ReceiverId = 1,
        //                IsRead = false,
        //                Message = "Det lyder helt vildt spændende!"
        //            },
        //            new ChatMessage
        //            {
        //                ReceiverId = 1,
        //                IsRead = false,
        //                Message = "Hvor plejer du at gå ture så?"
        //            },
        //        }
        //    };

        //    var mockRepo = new Mock<IChatRepository>();
        //    mockRepo.Setup(repo => repo.RetrieveChatMessages(1)).ReturnsAsync(match);
            

        //    var receiverId = 1;

        //    var isAnyChatMessageNotRead = chatMessages.Any(x => !x.IsRead);

        //    Assert.False(isAnyChatMessageNotRead);
        //}
    }
}