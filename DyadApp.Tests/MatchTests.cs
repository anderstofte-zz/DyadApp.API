using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Models;
using DyadApp.API.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace DyadApp.Tests
{
    [TestClass]
    public class MatchTests
    {
        [Theory]
        [InlineData(5)]
        [InlineData(23)]
        [InlineData(342)]
        [InlineData(10)]
        public async Task Create_awaiting_match_if_user_has_none_awaiting_matches_already(int userId)
        {
            var awaitingMatches = new List<AwaitingMatch>
            {
                new AwaitingMatch
                {
                    UserId = 1
                },
                new AwaitingMatch
                {
                    UserId = 2
                },
                new AwaitingMatch
                {
                    UserId = 3
                },
                new AwaitingMatch
                {
                    UserId = 4
                }
            };

            var mockMatchRepo = new Mock<IMatchRepository>();
            var mockUserRepo = new Mock<IUserRepository>();
            mockMatchRepo.Setup(repo => repo.GetAwaitingMatchByUserId(userId))
                .ReturnsAsync(awaitingMatches.SingleOrDefault(x => x.UserId == userId ));

            mockMatchRepo.Setup(repo => repo.AddAwaitingMatch(It.IsAny<AwaitingMatch>()))
                .Callback<AwaitingMatch>(aw =>
                {
                    awaitingMatches.Add(aw);
                }).Returns(Task.CompletedTask);
            var mockMatchService = new MatchService(mockUserRepo.Object, mockMatchRepo.Object);


            await mockMatchService.AddToAwaitingMatch(userId);
            var isAwaitingMatchAdded = awaitingMatches.Any(x => x.UserId == userId);


            Assert.True(isAwaitingMatchAdded);
        }


    }
}