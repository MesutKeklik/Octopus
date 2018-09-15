using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repositories.Abstract
{
    public interface IDataRepository
    {
        void SaveWords(List<WeightedWord> words);
        List<DecryptedWord> GetWordList();
    }
}
