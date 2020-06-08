using System;
using System.Collections.Generic;

namespace DyadApp.API.ViewModels
{
    public class MatchDataModel
    {
        public DateTime Created { get; set; }
        public List<ChatMessageDataModel> Messages { get; set; }
    }
}
