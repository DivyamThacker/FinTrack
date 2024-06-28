using FinTrack_API.Helper;
using FinTrack_Business.Repository;
using FinTrack_Business.Repository.IRepository;
using FinTrack_Common;
using FinTrack_DataAccess;
using FinTrack_DataAccess.Data;
using FinTrack_Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrack_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly APISettings _aPISettings;
        //private readonly ApplicationDbContext _db;
        private readonly IAccountRepository _accountRepository;
        public AccountController(
            IAccountRepository accountRepository,
            //ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<APISettings> options)
        {
            _accountRepository = accountRepository;
            //_db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _aPISettings = options.Value;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestDTO signUpRequestDTO)
        {
            if (signUpRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = new ApplicationUser
            {
                UserName = signUpRequestDTO.Email,
                Email = signUpRequestDTO.Email,
                Name = signUpRequestDTO.Name,
                PhoneNumber = signUpRequestDTO.PhoneNumber,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, signUpRequestDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }
            string accountId =null;

            var accountRequest = new AccountCreationRequestDTO
            {
                AccountName = signUpRequestDTO.AccountName,
                AccountType = signUpRequestDTO.AccountType,
                UserId = user.Id
            };
            if (result.Succeeded)
            {
                if (accountRequest.AccountName == null || !ModelState.IsValid)
                {
                    return BadRequest(new SignUpResponseDTO()
                    {
                        IsRegisterationSuccessful = false,
                        Errors = new string[] { "Please Enter valid Account Name" }
                    });
                }
                //if (accountRequest.AccountType != SD.Account_Investment && accountRequest.AccountType != SD.Account_Savings)
                //{
                //    return BadRequest(new SignUpResponseDTO()
                //    {
                //        IsRegisterationSuccessful = false,
                //        Errors = new string[] { "Please Enter valid Account Type" }
                //    });
                //}
                var accountResponse = await _accountRepository.Create(accountRequest);
                if (accountResponse.Success)
                {
                    accountId = accountResponse.AccountId;
                }
            }

            if (accountId == null)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = new string[] { "Account Creation Failed" }
                });
            }

            var roleResult = await _userManager.AddToRoleAsync(user, SD.Role_User);
            if (!roleResult.Succeeded)
            {
                return BadRequest(new SignUpResponseDTO()
                {
                    IsRegisterationSuccessful = false,
                    Errors = result.Errors.Select(u => u.Description)
                });
            }
            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] SignInRequestDTO signInRequestDTO)
        {
            if (signInRequestDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signInManager.PasswordSignInAsync(signInRequestDTO.UserName, signInRequestDTO.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(signInRequestDTO.UserName);
                if (user == null)
                {
                    return Unauthorized(new SignInResponseDTO
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Invalid Authentication"
                    });
                }

                //everything is valid and we need to login 
                var signinCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);

                var tokenOptions = new JwtSecurityToken(
                    issuer: _aPISettings.ValidIssuer,
                    audience: _aPISettings.ValidAudience,
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: signinCredentials);

                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                return Ok(new SignInResponseDTO()
                {
                    IsAuthSuccessful = true,
                    Token = token,
                    UserDTO = new UserDTO()
                    {
                        Name = user.Name,
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    }
                });

            }
            else
            {
                return Unauthorized(new SignInResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });
            }

            return StatusCode(201);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] AccountCreationRequestDTO accountRequest)
        {
            if (accountRequest.AccountName == null || !ModelState.IsValid)
            {
                return BadRequest(new AccountCreationResponseDTO
                {
                    Success = false,
                    Message = "Please Enter valid Account Name"
                });
            }
            //if (accountRequest.AccountType != SD.Account_Investment && accountRequest.AccountType != SD.Account_Savings)
            //{
            //    return BadRequest(new AccountCreationResponseDTO
            //    {
            //        Success = false,
            //        Message = "Please Enter valid Account Type"
            //    });
            //}
            var result = await _accountRepository.Create(accountRequest);
            return Ok(result);
        }

        [HttpDelete("{accountId}")]
        [ActionName("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount(string accountId)
        {
            var result = await _accountRepository.DeleteAccount(accountId);
            return Ok(result);
        }

        [HttpGet("{userId}")]
        [ActionName("GetAccounts")]
         public async Task<IActionResult> GetAccounts(string userId)
        {
            var result = _accountRepository.GetAllAccounts(userId);
            return  Ok(result);
        }


        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_aPISettings.SecretKey));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Email),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim("Id",user.Id)
            };

            var roles = await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
