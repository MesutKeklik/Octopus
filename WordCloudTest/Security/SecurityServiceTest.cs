using Core.Services;
using Core.Services.Abstract;
using System;
using System.IO;
using Xunit;

namespace WordCloudTest.Security
{
    public class SecurityServiceTest
    {
        private readonly ISecurityService _security;
        private readonly string privateKey = File.ReadAllText("Security\\privateKey.txt");
        private readonly string publicKey = File.ReadAllText("Security\\publicKey.txt");

        const string PLAIN_TEXT = "This is encryption test";

        public SecurityServiceTest()
        {
            _security = new SecurityService();
        }

        [Fact]
        public void IsHashComputedCorrectly()
        {
            var hashValue = _security.ComputeHash("Compute");
            Assert.True(_security.VerifyHash("Compute", hashValue));
        }

        /// <summary>
        /// Test asymmectric encryption/decryption same time
        /// </summary>
        [Fact]
        public void IsEncrpytionAndDecryptionCorrect()
        {
            var encryptedWord = _security.Encrypt(publicKey, PLAIN_TEXT);
            Assert.Equal(PLAIN_TEXT, _security.Decrypt(privateKey, encryptedWord));
        }

    }
}
