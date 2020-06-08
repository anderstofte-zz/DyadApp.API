using System;
using DyadApp.API.Helpers;
using Xunit;

namespace DyadApp.Tests
{
    public class EncryptionTests
    {
        [Theory]
        [InlineData("This is a simple test", "1RffceozaQtZg8Yy9A0pVOTIAJBVIcyonHTvE6a+CV4=")]
        [InlineData("Text with æ, ø, and Å", "a4zEB1pTPcJnFZWv226e/NJZjwRhgv8YnjksIJijmsA=")]
        public void Encrypted_string_should_equal_argument(string clearText, string cipherText)
        {
            const string encryptionKey = "n2r5u8x/A?D(G+Ka";

            var encryptedClearText = EncryptionHelper.Encrypt(clearText, encryptionKey);

            Assert.Equal(cipherText, encryptedClearText);
        }

        [Theory]
        [InlineData("This is a simple test", "RffceozaQtZg8Yy9A0pVOTIAJBVIcyonHTvE6a+CV4=")]
        [InlineData("Text with æ, ø, and Å.", "a4zEB1pTPcJnFZWv226e/NJZjwRhgv8YnjksIJijmsA=")]
        public void Encrypted_string_should_not_equal_argument(string clearText, string cipherText)
        {
            const string encryptionKey = "n2r5u8x/A?D(G+Ka";

            var encryptedClearText = EncryptionHelper.Encrypt(clearText, encryptionKey);

            Assert.NotEqual(cipherText, encryptedClearText);
        }

        [Theory]
        [InlineData("Test1234")]
        [InlineData("78AU!#PFaq")]
        [InlineData("KoDeÆ2Ø_Å")]
        public void Should_generate_encrypted_string_with_salt(string password)
        {
            var encryptionModel = EncryptionHelper.EncryptWithSalt(password);
            var buffer = new Span<byte>(new byte[encryptionModel.Text.Length]);

            Assert.True(Convert.TryFromBase64String(encryptionModel.Text, buffer, out var bytesWritten));
        }
    }
}