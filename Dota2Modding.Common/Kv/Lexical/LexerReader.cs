using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Kv.Lexical
{
    public class LexerReader
    {

        public readonly IAsyncEnumerator<IToken> iter;

        public List<KvError> Errors { get; } = new();

        public LexerReader(IAsyncEnumerable<IToken> tokens, CancellationToken cancellationToken)
        {
            iter = tokens.GetAsyncEnumerator(cancellationToken);
        }


        public async ValueTask MoveAsync()
        {
            await iter.MoveNextAsync();
            while (iter.Current.IsSpace())
            {
                await iter.MoveNextAsync();
            }
        }

        public bool Test(params TokenType[] types)
        {
            return types.Contains(iter.Current.TokenType);
        }

        public void Failure(params TokenType[] types)
        {
            Errors.Add(new(iter.Current.Line, iter.Current.Value, $"Invalid token at Ln{iter.Current.Line}, expect {string.Join(',', types)}, actual {iter.Current.Value}"));
        }

        public bool TestOrThrow(params TokenType[] types)
        {
            if (!types.Contains(iter.Current.TokenType))
            {
                Failure(types);
                return false;
            }

            return true;
        }

        public async ValueTask<bool> Expect(params TokenType[] types)
        {
            if (types.Contains(iter.Current.TokenType))
            {
                await MoveAsync();
                return true;
            }

            return false;
        }

        public async ValueTask<bool> ExpectOrThrow(params TokenType[] types)
        {
            if (await Expect(types))
            {
                return true;
            }

            Failure(types);
            return false;
        }

        public IToken Value => iter.Current;
    }
}
