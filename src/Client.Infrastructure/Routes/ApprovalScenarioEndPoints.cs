using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class ApprovalScenarioEndPoints
    {
        public static string GetAllScenarios = BaseEndpoint.Server + "api/ApprovalScenario/GetAllScenariosAsync";


        //public static string GetAllScenarios = BaseEndpoint.Server + "api/Scenario/GetAllScenariosAsync";

        public static string GetScenarioById(int id)
        {
            return BaseEndpoint.Server + $"api/ApprovalScenario/GetScenarioByIdAsync/{id}";
        }

        public static string UpsertScenario = BaseEndpoint.Server + "api/ApprovalScenario/UpsertScenarioAsync";

        public static string DeleteScenario(int scenarioId)
        {
            return BaseEndpoint.Server + $"api/ApprovalScenario/DeleteScenarioAsync/{scenarioId}";
        }

    }
}
