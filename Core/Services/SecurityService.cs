using Core.Services.Abstract;
using Core.Services.Extension;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Core.Services
{
    public class SecurityService : ISecurityService
    {
        private readonly RNGCryptoServiceProvider _rngCryptoServiceProvider;

        public SecurityService(IConfiguration configuration)
        {
            _rngCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        public string ComputeHash(string plainText)
        {
            var saltBytes = new byte[4];
            _rngCryptoServiceProvider.GetNonZeroBytes(saltBytes);

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] plainTextWithSaltBytes =
            new byte[plainTextBytes.Length + saltBytes.Length];

            for (int i = 0; i < plainTextBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainTextBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[plainTextBytes.Length + i] = saltBytes[i];
            }

            HashAlgorithm hash = new SHA512Managed();


            byte[] hashBytes = hash.ComputeHash(plainTextWithSaltBytes);

            byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashWithSaltBytes[i] = hashBytes[i];
            }

            for (int i = 0; i < saltBytes.Length; i++)
            {
                hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

            }

            return Convert.ToBase64String(hashWithSaltBytes);
        }

        public byte[] Encrypt(string publicKey, string plain)
        {
            byte[] encrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtensions.FromXmlString(rsa, publicKey);
                encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(plain), true);
            }
            return encrypted;
        }

        public string Decrypt(string privateKey, byte[] encrypted)
        {
            byte[] decrypted;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtensions.FromXmlString(rsa, privateKey);
                //rsa.FromXmlString(privateKey);
                decrypted = rsa.Decrypt(encrypted, true);
            }
            return Encoding.UTF8.GetString(decrypted);
        }

    }
}
