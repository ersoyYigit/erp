﻿using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Domain.Entities;
using ArdaManager.Shared.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Application.Interfaces.Common;
using ArdaManager.Application.Requests.Approval;
using ArdaManager.Application.Responses.Approval;

namespace ArdaManager.Application.Interfaces.Approval
{
    public interface IApprovalService 
    {
        Task<IResult<BaseDoc>> SubmitForApprovalAsync(SubmitApprovalRequest request);
        Task<IResult> ApproveStepAsync(ApproveRejectRequest request);
        Task<IResult> RejectStepAsync(ApproveRejectRequest request);
        Task<IResult<List<DocumentApprovalStatusResponse>>> GetDocumentApprovalStatusAsync(int documentId);
        Task<IResult<BaseDoc>> GetDocumentFinalStatusAsync(int documentId);

        Task<IResult<List<DocumentApprovalStatusResponse>>> GetWaitingStatusesByRoleAsync(string roleId);
        Task<IResult> UpdateDocStateAsync(UpdateDocStateRequest request);
    }
}
