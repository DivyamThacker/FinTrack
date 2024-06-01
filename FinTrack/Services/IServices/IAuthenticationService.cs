using FinTrack_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinTrack.Services.IServices
{
    public interface IAuthenticationService
    {
        Task<SignUpResponseDTO> RegisterUser(SignUpRequestDTO signUpRequestDTO);
        Task<SignInResponseDTO> Login(SignInRequestDTO signInRequestDTO);
        Task Logout();
    }
}
