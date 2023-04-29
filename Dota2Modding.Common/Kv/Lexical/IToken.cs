namespace Dota2Modding.Common.Kv.Lexical
{
    public interface IToken
    {
        public TokenType TokenType { get; }
        public string Value { get; }

        public int Line { get; }

        public bool IsSpace()
        {
            return TokenType switch
            {
                TokenType.Space or TokenType.Tab or TokenType.Comment or TokenType.NewLine => true,
                _ => false,
            };
        }
    }

    public struct NewLine : IToken
    {
        public TokenType TokenType => TokenType.NewLine;

        public string Value => "\n";

        public int Line { get; set; }
        public static NewLine Of(int line) => new() { Line = line };
    }


    public struct End : IToken
    {
        public TokenType TokenType => TokenType.End;

        public string Value => "\0";

        public int Line { get; set; }
        public static End Of(int line) => new() { Line = line };
    }

    public struct BlockBegin : IToken
    {
        public TokenType TokenType => TokenType.BlockBegin;

        public string Value => "{";

        public int Line { get; set; }

        public static BlockBegin Of(int line) => new() { Line = line };
    }

    public struct BlockEnd : IToken
    {
        public TokenType TokenType => TokenType.BlockEnd;

        public string Value => "}";

        public int Line { get; set; }

        public static BlockEnd Of(int line) => new() { Line = line };
    };

    public struct Tab : IToken
    {
        public TokenType TokenType => TokenType.Tab;

        public string Value => "\t";

        public int Line { get; set; }
        public static Tab Of(int line) => new() { Line = line };
    };

    public struct Space : IToken
    {
        public TokenType TokenType => TokenType.Space;

        public string Value => " ";

        public int Line { get; set; }
        public static Space Of(int line) => new() { Line = line };
    };

    public struct Invalid : IToken
    {
        public TokenType TokenType => TokenType.InvalidLine;

        public string Value { get; set; }

        public int Line { get; set; }

        public static Invalid Of(string content, int line) => new() { Value = content, Line = line };
    }
    public struct Comment : IToken
    {
        public TokenType TokenType => TokenType.Comment;

        public string Value { get; set; }

        public int Line { get; set; }

        public static Comment Of(string content, int line) => new() { Value = content, Line = line };
    }

    public struct String : IToken
    {
        public TokenType TokenType => TokenType.String;

        public string Value { get; set; }

        public int Line { get; set; }

        public static String Of(string content, int line) => new() { Value = content, Line = line };
    }

}
