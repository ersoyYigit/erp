using System.Text.Json;
using ArdaManager.Application.Interfaces.Serialization.Options;

namespace ArdaManager.Application.Serialization.Options
{
    public class SystemTextJsonOptions : IJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; } = new();
    }
}