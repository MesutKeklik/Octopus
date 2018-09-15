using Core.Services.Abstract;
using Data.Repositories.Abstract;
using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Data
{
    public class DataRepository : IDataRepository
    {
        private KeyPair _keyPair;
        private readonly ISecurityService _securityService;
        private List<DecryptedWord> _decryptedWords;

        public DataRepository(ISecurityService securityService)
        {
            _securityService = securityService;
            _keyPair = GetKeys();
            _decryptedWords = GetWordList();
        }

        public List<DecryptedWord> GetWordList()
        {
            List<DecryptedWord> result = new List<DecryptedWord>();
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var existingWords = context.Words;
                foreach (var item in existingWords)
                {
                    result.Add(new DecryptedWord { SaltedHashWord = item.SaltedHashWord, Frequency = item.Frequency, Word = _securityService.Decrypt(_keyPair.PrivateKey, item.EncyrptedWord) });
                }
            }
            return result;
        }


        public void SaveWords(List<WeightedWord> words)
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                foreach (var item in words)
                {
                    var dWord = _decryptedWords.FirstOrDefault(x => x.Word == item.Word);
                    if (dWord != null)
                    {
                        var eWord = context.Words.FirstOrDefault(x=>x.SaltedHashWord == dWord.SaltedHashWord);
                        eWord.Frequency += item.Weight;
                        dWord.Frequency += item.Weight;
                        context.Entry(eWord).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                    {
                        var dbItem = new Word
                        {
                            SaltedHashWord = _securityService.ComputeHash(item.Word),
                            EncyrptedWord = _securityService.Encrypt(_keyPair.PublicKey, item.Word),
                            Frequency = item.Weight
                        };
                        
                        context.Add(dbItem);

                        var dItem = new DecryptedWord
                        {
                            SaltedHashWord = dbItem.SaltedHashWord,
                            Word = item.Word,
                            Frequency = item.Weight
                        };
                        _decryptedWords.Add(dItem);
                    }
                }
                context.SaveChanges();
            }
        }

        public KeyPair GetKeys()
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                var keyPair = context.KeyPairs.FirstOrDefault();
                if (keyPair == null)
                {
                    string privateKey = File.ReadAllText("Keys\\privateKey.txt");
                    string publicKey = File.ReadAllText("Keys\\publicKey.txt");
                    keyPair = new KeyPair { Id = Guid.NewGuid(), PublicKey = publicKey, PrivateKey = privateKey };
                    context.Add(keyPair);
                    context.SaveChanges();
                }
                return keyPair;
            }
        }
    }
}
