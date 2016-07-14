using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Tracing;

namespace TimeService.Logging
{
    public class SerilogTraceWriter: ITraceWriter
    {
        private static readonly Dictionary<TraceLevel, LogEventLevel> levels = new Dictionary<TraceLevel, LogEventLevel>() {
                { TraceLevel.Debug, LogEventLevel.Debug},
                { TraceLevel.Error, LogEventLevel.Error},
                { TraceLevel.Fatal, LogEventLevel.Fatal},
                { TraceLevel.Info, LogEventLevel.Information},
                { TraceLevel.Warn, LogEventLevel.Warning},
            };

        public void Trace(HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level == TraceLevel.Off) return;
            if (!levels.ContainsKey(level)) return;

            var eventLevel = levels[level];
            var contextLogger = Serilog.Log.ForContext(Constants.SourceContextPropertyName, category);

            if (contextLogger.IsEnabled(eventLevel))
            {
                var traceRecord = new TraceRecord(request, category, level);
                traceAction(traceRecord);

                string requestUri = null;
                string method = null;
                string message = null;

                if (traceRecord.Request != null)
                {
                    if (traceRecord.Request.RequestUri != null)
                    {
                        requestUri = traceRecord.Request.RequestUri.ToString();
                    }

                    if (traceRecord.Request.Method != null)
                    {
                        method = traceRecord.Request.Method.ToString();
                    }
                }

                if (traceRecord.Exception != null)
                {
                    message = traceRecord.Exception.GetBaseException().Message;
                }
                else
                {
                    message = traceRecord.Message;
                }

                contextLogger.Write(eventLevel, "{Method:l} {Uri} {Kind} {Operator:l} {Operation:l} {Message}",
                    method ?? string.Empty,
                    requestUri ?? string.Empty,
                    traceRecord.Operator ?? string.Empty,
                    traceRecord.Operation ?? string.Empty,
                    traceRecord.Kind.ToString(),
                    message ?? string.Empty
                );
            }
        }
    }
}