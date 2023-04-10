using ArdaManager.Shared.Settings;
using System.Threading.Tasks;
using ArdaManager.Shared.Wrapper;

namespace ArdaManager.Shared.Managers
{
    public interface IPreferenceManager
    {
        Task SetPreference(IPreference preference);

        Task<IPreference> GetPreference();

        Task<IResult> ChangeLanguageAsync(string languageCode);
    }
}