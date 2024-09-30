using System;

namespace Analytics
{
    public interface IEntity
    {
        string Identifier { get; }  // We use string to support both characters, words and sentences.
        int Weight { get; set; }
        DateTime LastUsed { get; set; }
        int ErrorCount { get; set; }
    }
}