
using ArdaManager.Application.Interfaces.Serialization.Settings;
using Newtonsoft.Json;

namespace ArdaManager.Application.Serialization.Settings
{
    public class NewtonsoftJsonSettings : IJsonSerializerSettings
    {
        public JsonSerializerSettings JsonSerializerSettings { get; } = new();
    }
}