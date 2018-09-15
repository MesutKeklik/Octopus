using Core.Services.Abstract;
using Data.Repositories.Abstract;
using Microsoft.Extensions.Caching.Memory;
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
        private readonly IMemoryCache _cacheService;
        private List<DecryptedWord> _decryptedWords;

        public DataRepository(ISecurityService securityService, IMemoryCache cacheService)
        {
            _securityService = securityService;
            _cacheService = cacheService;
            _keyPair = GetKeys();
            _decryptedWords = GetWordList();
        }

        public List<DecryptedWord> GetWordList()
        {
            if (!_cacheService.TryGetValue<List<DecryptedWord>>("WordList", out List<DecryptedWord> result))
            {
                result = new List<DecryptedWord>();
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    var existingWords = context.Words;
                    foreach (var item in existingWords)
                    {
                        result.Add(new DecryptedWord { SaltedHashWord = item.SaltedHashWord, Frequency = item.Frequency, Word = _securityService.Decrypt(_keyPair.PrivateKey, item.EncyrptedWord) });
                    }
                }
                _cacheService.Set<List<DecryptedWord>>("WordList", result);
            }
            return result;
        }

        public void SaveWords(List<WeightedWord> words)
        {
            using (ApplicationDBContext context = new ApplicationDBContext())
            {
                if (!_cacheService.TryGetValue<List<Word>>("TableContent", out List<Word> tableContent))
                {
                    tableContent = context.Words.ToList();
                    _cacheService.Set<List<Word>>("TableContent", tableContent);
                }

                foreach (var item in words)
                {
                    var dWord = _decryptedWords.FirstOrDefault(x => x.Word == item.Word);
                    if (dWord != null)
                    {
                        var eWord = tableContent.FirstOrDefault(x=>x.SaltedHashWord == dWord.SaltedHashWord);
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
                        tableContent.Add(dbItem);
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

                _cacheService.Remove("TableContent");
                _cacheService.Set<List<Word>>("TableContent", tableContent);
            }

            _cacheService.Remove("WordList");
            _cacheService.Set<List<DecryptedWord>>("WordList", _decryptedWords); //decryption is so expensive it should be kept in cache, and cache has to be updated every SaveProcess
        }

        public KeyPair GetKeys()
        {
            if (!_cacheService.TryGetValue<KeyPair>("CryptoKeys", out KeyPair result))
            {
                using (ApplicationDBContext context = new ApplicationDBContext())
                {
                    result = context.KeyPairs.FirstOrDefault();
                    if (result == null)
                    {
                        string privateKey = File.ReadAllText("Keys\\privateKey.txt");
                        string publicKey = File.ReadAllText("Keys\\publicKey.txt");
                        result = new KeyPair { Id = Guid.NewGuid(), PublicKey = publicKey, PrivateKey = privateKey };
                        context.Add(result);
                        context.SaveChanges();
                    }
                }
                _cacheService.Set<KeyPair>("CryptoKeys", result);
            }
            return result;
        }
    }
}
