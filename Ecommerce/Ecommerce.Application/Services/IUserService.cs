using Ecommerce.Application.Dtos;
using Ecommerce.Domain.Dtos;
using Ecommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public interface IUserService
    {
        Task<Response> RegisterUserAsync(RegisterUserDto model);
        Task<Response> LoginUserAsync(LoginUserDto model);
        Task AssignAdminRoleToUser(LoginUserDto model);
    }
}
