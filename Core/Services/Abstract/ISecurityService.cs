using System;

namespace Core.Services.Abstract
{
    public interface ISecurityService
    {
        string ComputeHash(string word);
        byte[] Encrypt(string publicKey, string plain);
        string Decrypt(string privateKey, byte[] encrypted);
    }
}
