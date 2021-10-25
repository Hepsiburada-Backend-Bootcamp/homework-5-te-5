using AutoMapper;
using Ecommerce.Application.Dtos;
using Ecommerce.Domain.Dtos;
using Ecommerce.Domain.Models;
using Ecommerce.Domain.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class UserService : IUserService
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration;

        public UserService(UserManager<User> userManager, IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }


        public async Task<Response> RegisterUserAsync(RegisterUserDto model)
        {
            if (model == null)
                throw new NullReferenceException("Register model is null");

            if (model.Password != model.ConfirmPassword)
                return new Response
                {
                    Message = "Confirm password doesnt match the password",
                    IsSuccess = false,
                };

            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
                return new Response
                {
                    Message = "User created successfully!",
                    IsSuccess = true
                };

            return new Response
            {
                Message = "User did not create.",
                IsSuccess = false,
                Errors = result.Errors.Select(e => e.Description)
            };
        }

        public async Task<Response> LoginUserAsync(LoginUserDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return new Response
                {
                    Message = "There is no user with that email address",
                    IsSuccess = false
                };
            }

            var result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
                return new Response
                {
                    Message = "Invalid password",
                    IsSuccess = false
                };

            var roleList = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("Email",model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };

            foreach (var item in roleList)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Issuer"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new Response
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpireDate = token.ValidTo

            };
        }

        public async Task AssignAdminRoleToUser(LoginUserDto model)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                IdentityRole role = new IdentityRole { Name = "Admin" };
                await _roleManager.CreateAsync(role);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            await _userManager.AddToRoleAsync(user, "Admin");
        }


    }
}
