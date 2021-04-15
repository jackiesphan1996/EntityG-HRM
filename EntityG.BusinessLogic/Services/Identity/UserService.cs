using AutoMapper;
using EntityG.BusinessLogic.Exceptions;
using EntityG.BusinessLogic.Services.Interfaces.Identity;
using EntityG.BusinessLogic.Services.Interfaces.Shared;
using EntityG.Contracts.Requests.Identity;
using EntityG.Contracts.Requests.Shared;
using EntityG.Contracts.Responses.Identity;
using EntityG.Contracts.Responses.Shared;
using EntityG.EntityFramework.Entities.Identity;
using EntityG.Shared.Constants.Role;
using EntityG.Shared.Wrapper;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EntityG.BusinessLogic.Services.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMailService _mailService;

        public UserService(
            UserManager<ApplicationUser> userManager, 
            IMapper mapper, 
            RoleManager<IdentityRole> roleManager, 
            IMailService mailService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _mailService = mailService;
        }

        private IMapper _mapper;

        public async Task<PagingResult<UserResponse>> GetAllAsync(int page, int pageSize, string keySearch)
        {
            IQueryable<ApplicationUser> query =  _userManager.Users.AsQueryable();

            if (!string.IsNullOrEmpty(keySearch))
            {
                query = query.Where(x => x.UserName.Contains(keySearch)
                                         || x.Email.Contains(keySearch)
                                         || x.FirstName.Contains(keySearch)
                                         || x.LastName.Contains(keySearch)
                                         || x.PhoneNumber.Contains(keySearch));
            }

            int totalData = await query.CountAsync();

            List<ApplicationUser> users = query.OrderByDescending(x => x.CreatedOn).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = _mapper.Map<List<UserResponse>>(users);

            return new PagingResult<UserResponse>
            {
                Data = result,
                TotalCount = totalData
            };
        }

        public async Task<IResult> RegisterAsync(RegisterRequest request, string origin)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                return Result.Fail($"Username '{request.UserName}' is already taken.");
            }
            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                IsActive = request.ActivateUser,
                EmailConfirmed = request.AutoConfirmEmail,
                CreatedOn = DateTime.UtcNow
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleConstant.BasicRole.ToString());
                    if (!request.AutoConfirmEmail)
                    {
                        var verificationUri = await SendVerificationEmail(user, origin);
                        //TODO: Attach Email Service here and configure it via appsettings
                        BackgroundJob.Enqueue(() => _mailService.SendAsync(new MailRequest() { From = "mail@codewithmukesh.com", To = user.Email, Body = $"Please confirm your account by <a href='{verificationUri}'>clicking here</a>.", Subject = "Confirm Registration" }));
                        return await Result<string>.SuccessAsync(user.Id, message: $"User Registered Mailbox");
                    }
                    return await Result<string>.SuccessAsync(user.Id, message: $"User Registered");
                }
                else
                {
                    return Result.Fail(result.Errors.Select(a => "UnSucceeded error : " +  a.Description).ToList());
                }
            }
            else
            {
                return await Result.FailAsync($"Email {request.Email } is already registered.");
            }
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/identity/user/confirm-email/";
            var endpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(endpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);
            return verificationUri;
        }

        public async Task<IResult<UserResponse>> GetAsync(string userId)
        {
            var user = await _userManager.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
            var result = _mapper.Map<UserResponse>(user);
            return await Result<UserResponse>.SuccessAsync(result);
        }

        public async Task<IResult> ToggleUserStatusAsync(ToggleUserStatusRequest request)
        {
            var user = await _userManager.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
            var IsAdmin = await _userManager.IsInRoleAsync(user, RoleConstant.AdministratorRole);
            if (IsAdmin)
            {
                return Result.Fail("Administrators Profile's Status cannot be toggled");
            }
            if (user != null)
            {
                user.IsActive = request.ActivateUser;
                var identityResult = await _userManager.UpdateAsync(user);
            }
            return Result.Success();
        }

        public async Task<IResult<UserRolesResponse>> GetRolesAsync(string userId)
        {
            var viewModel = new List<UserRoleModel>();
            var user = await _userManager.FindByIdAsync(userId);
            foreach (var role in _roleManager.Roles.ToList())
            {
                var userRolesViewModel = new UserRoleModel
                {
                    RoleName = role.Name
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }
                viewModel.Add(userRolesViewModel);
            }
            var result = new UserRolesResponse { UserRoles = viewModel };
            return Result<UserRolesResponse>.Success(result);
        }

        public async Task<IResult> UpdateRolesAsync(UpdateUserRolesRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user.Email == "mukesh@blazorhero.com") return Result.Fail("Not Allowed.");
            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, request.UserRoles.Where(x => x.Selected).Select(y => y.RoleName));
            return Result.Success("Roles Updated");
        }

        public async Task<IResult<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return Result<string>.Success(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/identity/token endpoint to generate JWT.");
            }
            else
            {
                throw new ValidationException($"An error occured while confirming {user.Email}.");
            }
        }

        public async Task<IResult> ForgotPasswordAsync(string emailId, string origin)
        {
            var user = await _userManager.FindByEmailAsync(emailId);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return Result.Fail("An Error has occured!");
            }
            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "account/reset-password";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var passwordResetURL = QueryHelpers.AddQueryString(_enpointUri.ToString(), "Token", code);
            var request = new MailRequest()
            {
                Body = $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(passwordResetURL)}'>clicking here</a>.",
                Subject = "Reset Password",
                To = emailId
            };
            BackgroundJob.Enqueue(() => _mailService.SendAsync(request));
            return Result.Success("Password Reset Mail has been sent to your authorized EmailId.");
        }

        public async Task<IResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return Result.Fail("An Error has occured!");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return Result.Success("Password Reset Successful!");
            }
            else
            {
                return Result.Fail("An Error has occured!");
            }
        }

        public async Task<IResult<List<LookupDto>>> GetAllAsync()
        {
            var allUsers = _userManager.Users.OrderBy(x => x.Email).Select(x => new LookupDto
            {
                Id = x.Id,
                Value = x.Email
            }).ToList();

            return await Result<List<LookupDto>>.SuccessAsync(allUsers);
        }

        public async Task<int> GetCountAsync()
        {
            var count = await _userManager.Users.CountAsync();
            return count;
        }
    }
}