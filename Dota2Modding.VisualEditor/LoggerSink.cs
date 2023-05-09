using AvalonDock.Themes;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.InMemory;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Dota2Modding.VisualEditor
{
    public class LoggerSink : ILogEventSink, IDisposable
    {
        private readonly Channel<string> channel = Channel.CreateUnbounded<string>();
        private readonly MessageTemplateTextFormatter formatter = new("[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}");
        private static readonly AsyncLocal<LoggerSink> LocalInstance = new();

        public LoggerSink()
        {
        }

        public IAsyncEnumerable<string> allEvents()
        {
            return channel.Reader.ReadAllAsync();
        }

        public static LoggerSink Instance
        {
            get
            {
                LocalInstance.Value ??= new LoggerSink();

                return LocalInstance.Value;
            }
        }

        public void Dispose()
        {
        }

        public void Emit(LogEvent logEvent)
        {
            var sw = new StringWriter();
            formatter.Format(logEvent, sw);
            channel.Writer.TryWrite(sw.ToString());
        }
    }
}
