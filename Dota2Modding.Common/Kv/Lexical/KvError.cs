namespace Dota2Modding.Common.Kv.Lexical
{
    public class KvError
    {
        public KvError(int line, string raw, string message)
        {
            Line = line;
            Raw = raw;
            Message = message;
        }

        public int Line { get; }
        public string Raw { get; }
        public string Message { get; }
    }
}
