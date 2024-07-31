using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using QualiPro_Recruitment_Web_Api.Models;
using QualiPro_Recruitment_Web_Api.Repositories.AuthRepo;
using QualiPro_Recruitment_Web_Api.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QualiPro_Recruitment_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;


        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInModel signInModel)
        {
            try
            {
                if (signInModel != null && signInModel.Email != null && signInModel.Password != null)
                {

                    AuthModel result = new AuthModel();
                    result = await _authRepository.SignIn(signInModel);
                    if (result.Success == true && result.AccessToken != null && result.UserInfo != null)
                    {
                        return Ok(result);
                    }
                    else if (result.Success == false)
                    {
                        return BadRequest(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("sign-up")]

        public async Task<IActionResult> SignUp(UserAccountRoleModel userAccountRoleModel)
        {
            try
            {
                if (userAccountRoleModel != null && userAccountRoleModel.Email != null && userAccountRoleModel.Password != null)
                {
                    AuthModel result = await _authRepository.SignUp(userAccountRoleModel);

                    if (result.Success == true && result.Success.Value && result.AccessToken != null && result.UserInfo != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("condidat/sign-up")]

        public async Task<IActionResult> SignUpCondidat(CondidatAccountRoleModel condidatAccountRoleModel)
        {
            try
            {
                if (condidatAccountRoleModel != null && condidatAccountRoleModel.Email != null && condidatAccountRoleModel.Password != null)
                {
                    AuthModel result = await _authRepository.SignUpCondidat(condidatAccountRoleModel);

                    if (result.Success == true && result.Success.Value && result.AccessToken != null && result.CondidatInfo != null)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        return BadRequest(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //public async Task<IActionResult> SomeControllerAction(string token)
        //{
        //    var _configuration = new ConfigurationBinder()
        //    _configuration["Jwt:Key"]

        //    //string secretKey = "YourSuperSecretKey"; // Ensure this matches the key used to create the token
        //    var tokenService = new TokenService(secretKey);

        //    var claimsPrincipal = tokenService.DecryptToken(token);

        //    if (claimsPrincipal == null)
        //    {
        //        return Unauthorized("Invalid token");
        //    }

        //    // Use claimsPrincipal to access the token's claims
        //    var userIdClaim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);
        //    var emailClaim = claimsPrincipal.FindFirst(ClaimTypes.Email);

        //    // Process the claims as needed

        //    return Ok(new { UserId = userIdClaim?.Value, Email = emailClaim?.Value });
        //}

        [HttpPost("condidat/sign-in")]
        public async Task<IActionResult> SignInCondidat(SignInModel signInModel)
        {
            try
            {
                if (signInModel != null && signInModel.Email != null && signInModel.Password != null)
                {

                    AuthModel result = new AuthModel();
                    result = await _authRepository.SignInCondidat(signInModel);
                    if (result.Success == true && result.AccessToken != null && result.CondidatInfo != null)
                    {
                        return Ok(result);
                    }
                    else if (result.Success == false)
                    {
                        return BadRequest(result);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-user-from-token")]
        public async Task<IActionResult> GetUserByToken(TokenModel tokenModel)
        {
            try
            {
                var UserAccountRoleModel = new UserAccountRoleModel();
                UserAccountRoleModel = await _authRepository.GetUserByToken(tokenModel);

                return Ok(UserAccountRoleModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            };
        }

        //[HttpGet("condidat/{id}")]
        //public async Task<IActionResult> GetCondidatById(int id)
        //{
        //    try
        //    {
        //        var condidat = await _authRepository.GetCondidatById(id);
        //        if (condidat != null)
        //        {
        //            return Ok(condidat.Email);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        //[HttpGet("condidat/{id}")]
        //public async Task<IActionResult> GetCondidatEmailById(int id)
        //{
        //    try
        //    {
        //        var condidatAccount = await _authRepository.GetCondidatAccountById(id);
        //        if (condidatAccount != null)
        //        {
        //            return Ok(condidatAccount.Email);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        //[HttpGet("{condidatId}/email")]
        //public async Task<IActionResult> GetEmailByCondidatId(int condidatId)
        //{
        //    var email = await _authRepository.GetEmailByCondidatId(condidatId);
        //    if (email == null)
        //    {
        //        return NotFound("Email not found.");
        //    }
        //    return Ok(new { Email = email });
        //}
    }


}