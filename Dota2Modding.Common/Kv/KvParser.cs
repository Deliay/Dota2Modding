using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dota2Modding.Common.Kv.Lexical;
using Dota2Modding.Common.Kv.Node;

namespace Dota2Modding.Common.Kv
{
    public class KvParser : IDisposable
    {
        private readonly LexerReader reader;
        private readonly CancellationTokenSource csc;

        public KvParser(LexicalAnalyzer analyzer, CancellationToken token)
        {
            this.csc = CancellationTokenSource.CreateLinkedTokenSource(token);
            this.reader = new LexerReader(analyzer.ReadAllTokens(csc.Token), csc.Token);
        }

        private async ValueTask<List<KvElement>> ReadPartialList()
        {
            var list = new List<KvElement>();

            while (reader.Test(TokenType.String))
            {
                var name = reader.Value.Value;
                await reader.ExpectOrThrow(TokenType.String);

                if (reader.Test(TokenType.String))
                {
                    list.Add(new KvString() { Name = name, Value = reader.Value.Value });
                    await reader.ExpectOrThrow(TokenType.String);
                }
                else if (await reader.ExpectOrThrow(TokenType.BlockBegin))
                {
                    list.Add(new KvList() { Name = name, Value = await ReadPartialList() });
                    await reader.ExpectOrThrow(TokenType.BlockEnd);
                }
                else
                {
                    reader.Failure(TokenType.String, TokenType.BlockBegin);
                }
            }
            return list;
        }

        private async ValueTask<KvElement> Read()
        {
            if (!reader.TestOrThrow(TokenType.String))
            {
                return KvElement.Null;
            }
            return new KvList()
            {
                Name = "",
                Value = await ReadPartialList()
            };
        }

        public async ValueTask<KvElement> Parse()
        {
            await reader.MoveAsync();
            return await Read();
        }

        public void Dispose()
        {
            using var _csc = csc;
            GC.SuppressFinalize(this);
        }

        public static async ValueTask<KvElement> Parse(LexicalAnalyzer analyzer, CancellationToken cancellationToken)
        {
            return await (new KvParser(analyzer, cancellationToken).Parse());
        }

        public static async ValueTask<KvElement> Parse(string path, CancellationToken cancellationToken)
        {
            using var analyzer = new LexicalAnalyzer(File.OpenRead(path));
            return await Parse(analyzer, cancellationToken);
        }
    }
}
