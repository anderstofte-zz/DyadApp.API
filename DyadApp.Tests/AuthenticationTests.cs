using System;
using DyadApp.API.Extensions;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using Xunit;

namespace DyadApp.Tests
{
    public class AuthenticationTests
    {
        [Theory]
        [InlineData("Yr5npg+F5AiicmpSZRwHsEabdUyUYYNErwcEBt5aVCn+1A6R", "Yr5npg+F5AiicmpSZRwHsA==", "Test1234")]
        [InlineData("sgB9BL3ynIPnKQXIbbwtXiZyvu2mDFpIRfEjHMMKZ+IaAgWW", "sgB9BL3ynIPnKQXIbbwtXg==", "78AU!#PFaq")]
        [InlineData("pD6KPGKVAilvGI6ujUebRdbPQOIYMtMhU7aB2SWHIEOF6Mik", "pD6KPGKVAilvGI6ujUebRQ==", "KoDeÆ2Ø_Å")]
        public void Validate_password_where_password_is_correct(string savedPassword, string savedSalt, string submittedPassword)
        {
            var mockUser = new User
            {
                Password = savedPassword,
                Salt = savedSalt
            };

            var isPasswordValid = mockUser.ValidatePassword(submittedPassword);

            Assert.True(isPasswordValid);
        }

        [Theory]
        [InlineData("Yr5npg+F5AiicmpSZRwHsEabdUyUYYNErwcEBt5aVCn+1A6R", "Yr5npg+F5AiicmpSZRwHsA==", "Test12345")]
        [InlineData("sgB9BL3ynIPnKQXIbbwtXiZyvu2mDFpIRfEjHMMKZ+IaAgWW", "sgB9BL3ynIPnKQXIbbwtXg==", "78aU!#PFaq")]
        [InlineData("pD6KPGKVAilvGI6ujUebRdbPQOIYMtMhU7aB2SWHIEOF6Mik", "pD6KPGKVAilvGI6ujUebRQ==", "oDeÆ2Ø_Å")]
        public void Validate_password_where_password_is_incorrect(string savedPassword, string savedSalt,
            string submittedPassword)
        {
            var mockUser = new User
            {
                Password = savedPassword,
                Salt = savedSalt
            };

            var isPasswordValid = mockUser.ValidatePassword(submittedPassword);

            Assert.False(isPasswordValid);
        }

        [Fact]
        public void Refresh_token_expire_before_31_days()
        {
            const int mockUserId = 31;
            var refreshToken = RefreshTokenHelper.Generate(mockUserId);

            Assert.False(refreshToken.ExpirationDate < DateTime.Today.AddDays(30));
        }

        [Fact]
        public void Refresh_token_expire_after_31_days()
        {
            const int mockUserId = 22;
            var refreshToken = RefreshTokenHelper.Generate(mockUserId);

            Assert.True(refreshToken.ExpirationDate < DateTime.Now.AddDays(31));
        }

        [Theory]
        [InlineData(
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiIiLCJpYXQiOjE1OTA1NjgzNzgsImV4cCI6MTYyMjEwNDM3OCwiYXVkIjoiIiwic3ViIjoiIiwidW5pcXVlX25hbWUiOiIxIn0.Gcv-yJeZCD01xeT6vo4JYDRCZ0KssLavaTJ2m47IgWI",
            1)]
        [InlineData(
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiIiLCJpYXQiOjE1OTA1NjgzNzgsImV4cCI6MTYyMjEwNDM3OCwiYXVkIjoiIiwic3ViIjoiIiwidW5pcXVlX25hbWUiOiIzMTIifQ.Br7X_NGsVkvUFEaFb3P3DRBEBp6s5zfU8wAr238vsyY",
            312)]
        [InlineData(
            "eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiIiLCJpYXQiOjE1OTA1NjgzNzgsImV4cCI6MTYyMjEwNDM3OCwiYXVkIjoiIiwic3ViIjoiIiwidW5pcXVlX25hbWUiOiI4NyJ9.4jv3sfH6RH2efQ_YwoVFFjN7kTd_4KnHk6BuGSiEeV4",
            87)]
        public void Get_user_id_from_jwt_token_claims(string token, int expectedUserId)
        {
            var key =
                "qwertyuiopasdfghjklzxcvbnm123456lsh40897fsljlj4324ljk234k3jfsdfsdfsdfd45h34k5hg345lk3hg34jklg345kjhg345lkjh3g4534512312313dffsdf";
            var authenticationTokens = new AuthenticationTokens
            {
                AccessToken = token,
                RefreshToken = "something"
            };

            var actualUserId = authenticationTokens.GetUserIdFromClaims(key);

            Assert.Equal(expectedUserId, actualUserId);
        }
    }
}
