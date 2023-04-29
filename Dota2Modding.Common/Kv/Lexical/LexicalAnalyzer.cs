using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Dota2Modding.Common.Kv.Lexical
{
    public class LexicalAnalyzer : IDisposable
    {
        private readonly StreamReader _reader;
        public LexicalAnalyzer(Stream content)
        {
            _reader = new StreamReader(content);
        }

        public async IAsyncEnumerable<IToken> ReadAllTokens([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var line = 1;
            var content = await _reader.ReadLineAsync(cancellationToken);
            while (content is not null)
            {
                for (int i = 0; i < content.Length; i++)
                {
                    var ch = content[i];
                    if (ch == '\t') yield return Tab.Of(line);
                    else if (ch == ' ') yield return Space.Of(line);
                    else if (ch == '{') yield return BlockBegin.Of(line);
                    else if (ch == '}') yield return BlockEnd.Of(line);

                    else if (content.Length - 1 == i)
                    {
                        yield return Invalid.Of(content, line);
                        break;
                    }
                    else if (ch == '/' && content[i + 1] == '/')
                    {
                        yield return Comment.Of(content[(i + 2)..], line);
                        break;
                    }

                    else if (ch == '"')
                    {
                        string val = "";
                        for (int j = i + 1; j < content.Length; j++)
                        {
                            i = j;
                            char c = content[j];

                            if (c == '"') break;

                            val += c;
                        }

                        yield return String.Of(val, line);
                        continue;
                    }
                    else
                    {
                        yield return Invalid.Of(content, line);
                        break;
                    }
                }
                yield return NewLine.Of(line);
                line += 1;
                content = await _reader.ReadLineAsync(cancellationToken);
            }
            yield return End.Of(line);
        }

        public void Dispose()
        {
            using var __reader = _reader;
            GC.SuppressFinalize(this);
        }
    }
}
