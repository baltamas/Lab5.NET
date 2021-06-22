using Microsoft.AspNetCore.Identity;
using MultimediaCenter.ViewModels;
using MultimediaCenter.ViewModels.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultimediaCenter.Services
{
    public interface IAuthManagementService
    {
        Task<ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>> RegisterUser(RegisterRequest registerRequest);
        Task<bool> ConfirmUserRequest(ConfirmUserRequest confirmUserRequest);
        Task<ServiceResponse<LoginResponse, string>> LoginUser(LoginRequest loginRequest);
    }
}
