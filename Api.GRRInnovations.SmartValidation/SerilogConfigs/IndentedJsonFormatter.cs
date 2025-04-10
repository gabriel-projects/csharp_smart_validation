using Serilog.Events;
using Serilog.Formatting;
using System.Text.Json;

namespace Api.GRRInnovations.SmartValidation.SerilogConfigs
{
    public class IndentedJsonFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            var logObject = new
            {
                Timestamp = logEvent.Timestamp,
                Level = logEvent.Level.ToString(),
                Message = logEvent.RenderMessage(),
                Exception = logEvent.Exception?.ToString(),
                Properties = logEvent.Properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString())
            };

            var json = JsonSerializer.Serialize(logObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            output.WriteLine(json);
        }
    }
}
