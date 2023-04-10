using ArdaManager.Application.Features.Currencies.Queries;
using ArdaManager.Application.Interfaces.Approval;
using ArdaManager.Application.Interfaces.Services;
using ArdaManager.Application.Interfaces.Services.Identity;
using ArdaManager.Application.Requests.Approval;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Domain.Contracts;
using ArdaManager.Domain.Entities;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Domain.Enums.Doc;
using ArdaManager.Infrastructure.Contexts;
using ArdaManager.Infrastructure.Models.Identity;
using ArdaManager.Shared.Models;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArdaManager.Application.Extensions;

namespace ArdaManager.Infrastructure.Services.Approval
{
    public class ApprovalService : IApprovalService
    {

        private readonly VappContext _context;
        private readonly RoleManager<VappRole> _roleManager;

        public ApprovalService(
            VappContext context,
            RoleManager<VappRole> roleManager)
        {
            _roleManager = roleManager;
            _context = context;
        }



        public async Task<IResult<BaseDoc>> SubmitForApprovalAsync(SubmitApprovalRequest request)
        {
            // Evrak tipine göre onay senaryosunu bulma
            var approvalScenario = await _context.ApprovalScenarios.Include(a => a.ApprovalSteps).FirstOrDefaultAsync(a => a.DocType == request.DocType);
            
            BaseDoc baseDoc = _context.BaseDocs.Where(x => x.Id == request.DocumentId).FirstOrDefault();
            
            if (baseDoc == null)
            {
                //throw new InvalidOperationException("Onay senaryosu bulunamadı.");
                //return await Result.FailAsync<SubmitForApproveResponse>( "İlgili doküman bulunamadı");
                return await Result<BaseDoc>.FailAsync("Doküman bulunamadı");
            }

            if (approvalScenario == null)
            {
                //throw new InvalidOperationException("Onay senaryosu bulunamadı.");
                baseDoc.DocState = DocState.Approved;
                await _context.SaveChangesAsync();

                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Evrak Onaya tabii olmadığı için otomatik olarak onaylandı");
            }

            // İlk adımdaki rolü alın ve onay sürecini başlatın
            var firstStep = approvalScenario.ApprovalSteps.OrderBy(s => s.StepNumber).FirstOrDefault();
            if (firstStep == null)
            {
                baseDoc.DocState = DocState.Approved;
                await _context.SaveChangesAsync();

                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Evrak Onaya tabii olmadığı için otomatik olarak onaylandı");

            }



            // Dokümanı kaydet
            //_context.Add(document);
            //await _context.SaveChangesAsync();

            // Kullanıcının rolünü kontrol edin ve gerekirse onaylayın
            var userRoles = await _context.UserRoles.Where(ur => ur.UserId == request.UserId).ToListAsync();
            var documentApprovalStatuses = new List<DocumentApprovalStatus>();

            foreach (var step in approvalScenario.ApprovalSteps.OrderBy(s => s.StepNumber))
            {
                var documentApprovalStatus = new DocumentApprovalStatus
                {
                    BaseDocId = baseDoc.Id,
                    ApprovalStepId = step.Id
                };

                // Kullanıcının ilgili adımdaki rolü varsa onayla
                if (userRoles.Any(ur => ur.RoleId == step.UserRoleId))
                {
                    documentApprovalStatus.IsApproved = true;
                    documentApprovalStatus.ApproveDate = DateTime.UtcNow;
                }
                else
                {
                    documentApprovalStatus.IsApproved = false;
                }

                documentApprovalStatuses.Add(documentApprovalStatus);
            }

            // Onay durumlarını kaydet
            _context.DocumentApprovalStatuses.AddRange(documentApprovalStatuses);
            await _context.SaveChangesAsync();




            return await SetDocState(baseDoc.Id);
        }



        // Belirli bir adımın onaylanması
        public async Task<IResult> ApproveStepAsync(ApproveRejectRequest request)
        {
            var approvalStatus = await _context.DocumentApprovalStatuses
                .Include(das => das.ApprovalStep)
                .FirstOrDefaultAsync(das => das.BaseDocId == request.DocumentId && das.ApprovalStep.StepNumber == request.StepNumber);

            if (approvalStatus == null)
            {
                return await Result.FailAsync("Onay durumu bulunamadı.");
                //throw new InvalidOperationException("Onay durumu bulunamadı.");
            }

            var userRoles = await _context.UserRoles.Where(ur => ur.UserId == request.UserId).ToListAsync();

            // Kullanıcının ilgili adımdaki rolü varsa onayla
            if (userRoles.Any(ur => ur.RoleId == approvalStatus.ApprovalStep.UserRoleId))
            {
                approvalStatus.IsApproved = true;
                approvalStatus.ApproveDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await SetDocState(request.DocumentId);
                return await Result.SuccessAsync("Onay eklendi");
            }
            else
            {
                return await Result.FailAsync("Kullanıcının bu adımı onaylama yetkisi yok.");
                
            }
        }
        
        
        // Belirli bir adımın reddedilmesi
        public async Task<IResult> RejectStepAsync(ApproveRejectRequest request)
        {
            var approvalStatus = await _context.DocumentApprovalStatuses
                .Include(das => das.ApprovalStep)
                .FirstOrDefaultAsync(das => das.BaseDocId == request.DocumentId && das.ApprovalStep.StepNumber == request.StepNumber);

            if (approvalStatus == null)
            {
                //throw new InvalidOperationException("Onay durumu bulunamadı.");
                return await Result.FailAsync("Onay durumu bulunamadı.");
            }

            var userRoles = await _context.UserRoles.Where(ur => ur.UserId == request.UserId).ToListAsync();

            // Kullanıcının ilgili adımdaki rolü varsa reddet
            if (userRoles.Any(ur => ur.RoleId == approvalStatus.ApprovalStep.UserRoleId))
            {
                approvalStatus.IsApproved = false;
                approvalStatus.ApproveDate = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                await SetDocState(request.DocumentId);
                return await Result.SuccessAsync("Onay talebi rededildi");
            }
            else
            {
                return await Result.FailAsync("Kullanıcının bu adımı reddetme yetkisi yok.");
                //throw new InvalidOperationException("Kullanıcının bu adımı reddetme yetkisi yok.");
            }
        }


        public async Task<IResult> UpdateDocStateAsync(UpdateDocStateRequest request)
        {
            BaseDoc baseDoc = _context.BaseDocs.Where(x => x.Id == request.BaseDocId).FirstOrDefault();

            if (baseDoc == null)
            {
                //throw new InvalidOperationException("Onay senaryosu bulunamadı.");
                //return await Result.FailAsync<SubmitForApproveResponse>( "İlgili doküman bulunamadı");
                return await Result<BaseDoc>.FailAsync("Doküman bulunamadı");
            }
            baseDoc.DocState = request.NewDocState;

            await _context.SaveChangesAsync();

            return await Result.SuccessAsync(message: $"{baseDoc.DocNo} numaralı dokümanın statüsü \"{baseDoc.DocState.ToDescriptionStringVapp()}\" olarak güncellendi");

        }


        // Belirli bir dokümanın onay durumunu sorgulamak
        public async Task<IResult<List<DocumentApprovalStatusResponse>>> GetDocumentApprovalStatusAsync(int documentId)
        {
            var approvalStatuses = await _context.DocumentApprovalStatuses
                .Include(das => das.ApprovalStep)
                .Include(das => das.BaseDoc)
                .Where(das => das.BaseDocId == documentId)
                .OrderBy(das => das.ApprovalStep.StepNumber)
                .ToListAsync();

            List<DocumentApprovalStatusResponse> response = new List<DocumentApprovalStatusResponse>();
            foreach (var item in approvalStatuses)
            {
                DocumentApprovalStatusResponse stat = new DocumentApprovalStatusResponse() { 
                    ApprovalStepId = item.ApprovalStepId,
                    ApproveDate= item.ApproveDate,
                    BaseDocId = item.BaseDocId,
                    DocDate = item.BaseDoc.DocDate,
                    DocNo = item.BaseDoc.DocNo,
                    DocState = item.BaseDoc.DocState,
                    DocType = item.BaseDoc.DocType,
                    Id= item.Id,
                    IsApproved = item.IsApproved,
                    StepNumber = item.ApprovalStep.StepNumber,
                    UserRoleId= item.ApprovalStep.UserRoleId,
                    UserRoleName = _roleManager.Roles.Where(x=>x.Id ==item.ApprovalStep.UserRoleId).FirstOrDefault().Name,
                    UserRoleDepartment = _roleManager.Roles.Where(x=>x.Id ==item.ApprovalStep.UserRoleId).FirstOrDefault().Department,
                };
                response.Add(stat);
            }

            return await Result<List<DocumentApprovalStatusResponse>>.SuccessAsync(response);
        }

        // Belirli bir dokümanın onay durumunu sorgulamak
        public async Task<IResult<List<DocumentApprovalStatusResponse>>> GetWaitingStatusesByRoleAsync(string roleId)
        {
            var approvalStatuses = await _context.DocumentApprovalStatuses
                .Include(das => das.ApprovalStep)
                .Include(das => das.BaseDoc)
                .Where(das => das.ApprovalStep.UserRoleId == roleId && !das.ApproveDate.HasValue)
                .OrderBy(das => das.ApprovalStep.StepNumber)
                .ToListAsync();

            List<DocumentApprovalStatusResponse> response = new List<DocumentApprovalStatusResponse>();
            foreach (var item in approvalStatuses)
            {
                DocumentApprovalStatusResponse stat = new DocumentApprovalStatusResponse() { 
                    ApprovalStepId = item.ApprovalStepId,
                    ApproveDate= item.ApproveDate,
                    BaseDocId = item.BaseDocId,
                    DocDate = item.BaseDoc.DocDate,
                    DocNo = item.BaseDoc.DocNo,
                    DocState = item.BaseDoc.DocState,
                    DocType = item.BaseDoc.DocType,
                    Id= item.Id,
                    IsApproved = item.IsApproved,
                    StepNumber = item.ApprovalStep.StepNumber,
                    UserRoleId= item.ApprovalStep.UserRoleId,
                    UserRoleName = _roleManager.Roles.Where(x=>x.Id ==item.ApprovalStep.UserRoleId).FirstOrDefault().Name,
                    UserRoleDepartment = _roleManager.Roles.Where(x=>x.Id ==item.ApprovalStep.UserRoleId).FirstOrDefault().Department,
                };
                response.Add(stat);
            }

            return await Result<List<DocumentApprovalStatusResponse>>.SuccessAsync(response);
        }




        public async Task<IResult<BaseDoc>> GetDocumentFinalStatusAsync(int documentId)
        {
            BaseDoc baseDoc = _context.BaseDocs.Where(x => x.Id == documentId).FirstOrDefault();

            if (baseDoc == null)
            {
                //throw new InvalidOperationException("Onay senaryosu bulunamadı.");
                //return await Result.FailAsync<SubmitForApproveResponse>( "İlgili doküman bulunamadı");
                return await Result<BaseDoc>.FailAsync("Doküman bulunamadı");
            }

            var approvalStatuses = await _context.DocumentApprovalStatuses
                .Include(das => das.ApprovalStep)
                .Where(das => das.BaseDocId == documentId)
                .OrderBy(das => das.ApprovalStep.StepNumber)
                .ToListAsync();

            if (approvalStatuses.Count == 0)
            {
                //return await Result.FailAsync("Onay durumu bulunamadı.");
                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Evrak Onaya tabii olmadığı için otomatik olarak onaylandış");
                //throw new InvalidOperationException("Onay durumları bulunamadı.");
            }

            bool hasRejectedStep = false;
            bool hasPendingStep = false;

            foreach (var status in approvalStatuses)
            {
                if (!status.IsApproved && status.ApproveDate.HasValue)
                {
                    hasRejectedStep = true;
                    break;
                }
                else if (!status.IsApproved)
                {
                    hasPendingStep = true;
                }
            }

            if (hasRejectedStep)
            {
                //return await Result<DocState>.SuccessAsync(DocState.Rejected, "Doküman Reddedilmiş");
                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Doküman Reddedilmiş");
                //return DocState.Rejected;
            }
            else if (hasPendingStep)
            {
                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Doküman onay sürecinde bekliyor");
                //return DocumentFinalStatus.Pending;
            }
            else
            {
                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Doküman Onaylanmış");
                //return DocumentFinalStatus.Approved;
            }
        }




        private async Task<IResult<BaseDoc>> SetDocState(int documentId)
        {
            BaseDoc baseDoc = _context.BaseDocs.Where(x => x.Id == documentId).FirstOrDefault();

            if (baseDoc == null)
            {
                //throw new InvalidOperationException("Onay senaryosu bulunamadı.");
                //return await Result.FailAsync<SubmitForApproveResponse>( "İlgili doküman bulunamadı");
                return await Result<BaseDoc>.FailAsync("Doküman bulunamadı");
            }

            var approvalStatuses = await _context.DocumentApprovalStatuses
                .Include(das => das.ApprovalStep)
                .Where(das => das.BaseDocId == documentId)
                .OrderBy(das => das.ApprovalStep.StepNumber)
                .ToListAsync();

            
            bool hasRejectedStep = false;
            bool hasPendingStep = false;

            foreach (var status in approvalStatuses)
            {
                if (!status.IsApproved && status.ApproveDate.HasValue)
                {
                    hasRejectedStep = true;
                    break;
                }
                else if (!status.IsApproved)
                {
                    hasPendingStep = true;
                }
            }

            if (hasRejectedStep)
            {

                baseDoc.DocState = DocState.Rejected;
                await _context.SaveChangesAsync();

                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Doküman Reddedilmiş");
                //return DocState.Rejected;
            }
            else if (hasPendingStep)
            {
                try
                {
                    baseDoc.DocState = DocState.Waiting;
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return await Result<BaseDoc>.FailAsync(ex.Message);
                }
                

                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Doküman onay sürecinde bekliyor");
                //return DocumentFinalStatus.Pending;
            }
            else
            {

                baseDoc.DocState = DocState.Approved;
                await _context.SaveChangesAsync();

                return await Result<BaseDoc>.SuccessAsync(baseDoc, "Doküman Onaylanmış");
                //return DocumentFinalStatus.Approved;
            }
        }
    }
}
