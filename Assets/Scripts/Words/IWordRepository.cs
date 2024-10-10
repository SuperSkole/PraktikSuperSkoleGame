using System.Collections.Generic;
using Analytics;

namespace Words
{
    public interface IWordRepository
    {
        List<WordData> GetWordsByLength(WordLength length);
    }
}