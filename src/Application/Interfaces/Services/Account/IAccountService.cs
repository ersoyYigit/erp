﻿using ArdaManager.Application.Interfaces.Common;
using ArdaManager.Application.Requests.Identity;
using ArdaManager.Shared.Wrapper;
using System.Threading.Tasks;

namespace ArdaManager.Application.Interfaces.Services.Account
{
    public interface IAccountService : IService
    {
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest model, string userId);

        Task<IResult> ChangePasswordAsync(ChangePasswordRequest model, string userId);

        Task<IResult<string>> GetProfilePictureAsync(string userId);

        Task<IResult<string>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, string userId);
    }
}