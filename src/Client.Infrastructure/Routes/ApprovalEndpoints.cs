using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Client.Infrastructure.Routes
{
    public static class ApprovalEndpoints
    {
        public static string SubmitForApproval = BaseEndpoint.Server + "api/Approval/submit-for-approval";
        public static string ApproveStep = BaseEndpoint.Server + "api/Approval/approve-step";
        public static string RejectStep = BaseEndpoint.Server + "api/Approval/reject-step";
        public static string UpdateDocState = BaseEndpoint.Server + "api/Approval/update-docstate";


        public static string GetDocumentApprovalStatus(int documentId)
        {
            return BaseEndpoint.Server + $"api/Approval/get-document-approval-status/{documentId}";
        }
        public static string GetWaitingStatusesByRole(string roleId)
        {
            return BaseEndpoint.Server + $"api/Approval/get-waiting-statuses/{roleId}";
        }

        public static string GetDocumentFinalStatus(int documentId)
        {
            return BaseEndpoint.Server + $"api/Approval/get-document-final-status/{documentId}";
        }

    }
}
