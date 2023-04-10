using ArdaManager.Application.Interfaces;
using ArdaManager.Application.Interfaces.Approval;
using ArdaManager.Application.Interfaces.Services.Identity;
using ArdaManager.Application.Responses.Approval;
using ArdaManager.Application.Responses.Identity;
using ArdaManager.Domain.Entities.Approval;
using ArdaManager.Infrastructure.Contexts;
using ArdaManager.Shared.Models;
using ArdaManager.Shared.Wrapper;
using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArdaManager.Infrastructure.Services.Approval
{
    public class ApprovalScenarioService : IApprovalScenarioService
    {
        
        private readonly VappContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public ApprovalScenarioService(
            VappContext context,
            IMapper mapper,
            IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IResult<List<ApprovalScenarioResponse>>> GetAllAsync()
        {
            
            var scenarios = await _context.ApprovalScenarios
                .Include(a => a.ApprovalSteps)
                .ToListAsync();

            //var result = _mapper.Map<ApprovalScenarioResponse>(scenarios);
            var result = _mapper.Map<List<ApprovalScenarioResponse>>(scenarios);
            return await Result<List<ApprovalScenarioResponse>>.SuccessAsync(result);


        }

        public async Task<IResult<ApprovalScenarioResponse>> GetByIdAsync(int id)
        {
            var data = await _context.ApprovalScenarios
                .Include(a => a.ApprovalSteps)
                .FirstOrDefaultAsync(a => a.Id == id);
            var result = _mapper.Map<ApprovalScenarioResponse>(data);
            return await Result<ApprovalScenarioResponse>.SuccessAsync(result);
        }

        public async Task<IResult> UpsertAsync(ApprovalScenarioResponse approvalScenario)
        {
            if (approvalScenario.Id == 0)
            {
                var approvalEntity = _mapper.Map<ApprovalScenario>(approvalScenario);
                _context.ApprovalScenarios.Add(approvalEntity);
                await _context.SaveChangesAsync();
                return await Result.SuccessAsync("Senaryo eklendi");
            }
            else
            {
                var approvalEntity = _mapper.Map<ApprovalScenario>(approvalScenario);
                
                approvalEntity.ApprovalSteps = _mapper.Map<ICollection<ApprovalStep>>(approvalScenario.ApprovalSteps);

                // Önce mevcut senaryonun mevcut adımlarını veritabanından alın
                var existingSteps = await _context.ApprovalSteps.Where(x => x.ApprovalScenarioId == approvalEntity.Id).ToListAsync();

                // Yeni adımların ID'lerini bir listeye alın
                var newStepIds = approvalEntity.ApprovalSteps.Select(x => x.Id).ToList();

                // Silinen adımları tespit edin ve veritabanından kaldırın
                var stepsToDelete = existingSteps.Where(x => !newStepIds.Contains(x.Id)).ToList();
                _context.ApprovalSteps.RemoveRange(stepsToDelete);

                // ApprovalEntity'yi güncelleyin
                _context.Entry(approvalEntity).State = EntityState.Modified;

                // Tüm değişiklikleri kaydedin
                await _context.SaveChangesAsync();

                return await Result.SuccessAsync("Senaryo güncellendi");
            }
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            var result = await GetByIdAsync(id);
            if (!result.Succeeded)
            {
                return await Result.FailAsync("Senaryo bulunamadı");
            }
            try
            {
                // Dto'yu ApprovalScenario nesnesine geri dönüştürün
                var approvalScenario = _mapper.Map<ApprovalScenario>(result.Data);

                // İzlenen nesneyi bulun ve silin
                var trackedEntity = _context.ApprovalScenarios.Local.FirstOrDefault(e => e.Id == approvalScenario.Id);
                if (trackedEntity != null)
                {
                    _context.ApprovalScenarios.Remove(trackedEntity);
                }
                else
                {
                    _context.ApprovalScenarios.Remove(approvalScenario);
                }

                await _context.SaveChangesAsync();
                return await Result.SuccessAsync("Senaryo silindi");
            }
            catch (Exception)
            {
                return await Result.FailAsync("Senaryo silinirken bir hata meydana geldi");
            }

        }
    }
}
