﻿using System;
using System.Collections.Generic;
using System.Linq;
using DyadApp.API.Models;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Converters
{
    public static class MatchConverter
    {
        public static List<MatchViewModel> ToMatchViewToModel(this List<Match> matches, int userId)
        {
            var matchList = new List<MatchViewModel>();

            foreach (var match in matches)
            {
                var model = new MatchViewModel();
                foreach (var userMatch in match.UserMatches.Where(userMatch => userMatch.UserId != userId))
                {
                    model.Name = userMatch.User.Name;
                    model.ProfileImage = Convert.ToBase64String(userMatch.User.ProfileImage);
                    model.UserId = userMatch.UserId;
                }

                model.MatchId = match.MatchId;
                model.LastMessage = match.ChatMessages.OrderByDescending(x => x.Created).Select(x => x.Message)
                    .FirstOrDefault();
                model.LastMessageTimeStamp = match.ChatMessages.OrderByDescending(x => x.Created).Select(x => x.Created)
                    .FirstOrDefault();
                model.MatchCreated = match.Created;
                model.UnreadMessages = match.ChatMessages.Count(x => !x.IsRead);
                matchList.Add(model);
            }

            return matchList;
        }

        public static MatchConversationModel ToChatMessageModels(this Match match, int userId)
        {
            var chatMessages = match.ChatMessages
                .OrderBy(x => x.Created)
                .Select(chatMessage => new ChatMessageModel {Message = chatMessage.Message, UserId = chatMessage.SenderId, Sent = chatMessage.Created}).ToList();

            return new MatchConversationModel
            {
                MatchId = match.MatchId,
                UserId = userId,
                OtherPersonsUserId = match.UserMatches.Where(x => x.UserId != userId).Select(x => x.UserId).FirstOrDefault(),
                Name = match.UserMatches.Where(x => x.UserId != userId).Select(x => x.User.Name).FirstOrDefault(),
                ChatMessages = chatMessages
            };
        }
    }
}