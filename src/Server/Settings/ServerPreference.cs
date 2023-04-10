using System.Linq;
using ArdaManager.Shared.Constants.Localization;
using ArdaManager.Shared.Settings;

namespace ArdaManager.Server.Settings
{
    public record ServerPreference : IPreference
    {
        public string LanguageCode { get; set; } = LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "tr-TR";

        //TODO - add server preferences
    }
}