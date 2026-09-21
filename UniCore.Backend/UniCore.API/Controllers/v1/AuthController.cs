using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using UniCore.API.Controllers;
using UniCore.Application.Contract.Service.v1;
using UniCore.Application.DTO;
using UniCore.Application.Feature.v1.Auth.ForgotPassword;
using UniCore.Application.Feature.v1.Auth.Login;
using UniCore.Application.Feature.v1.Auth.Logout;
using UniCore.Application.Feature.v1.Auth.Me;
using UniCore.Application.Feature.v1.Auth.RefreshToken;
using UniCore.Application.Feature.v1.Auth.Register;
using UniCore.Application.Feature.v1.Auth.VerifyOtp;
using UniCore.Helper.Constant;
using UniCore.Helper.Localization;

namespace Project_Structure_UniCore.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService, IJsonStringLocalizer localizer) : base(localizer)
        {
            _authService = authService;
        }

        /// <summary>
        /// Login
        /// </summary>
        [EnableRateLimiting("login")]
        [HttpPost("login")]
        [ProducesResponseType(typeof(BaseAPIResponse<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            Response.Cookies.Append(
                "UniCore_RefreshToken",
                result.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddDays(result.RefreshTokenExpire),
                    Path = "/api/v1/auth",
                    IsEssential = true
                }
            );
            var message = _localizer.GetString(MessageConstants.Auth.LoginSuccess);
            return OkResponse<LoginResponseDTO>(result, message);
        }

        /// <summary>
        /// Refresh JWT Access Token using Refresh Token
        /// </summary>
        [HttpPost("refresh-token")]
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(BaseAPIResponse<RefreshTokenResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<RefreshTokenResponseDTO>>> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            var message = _localizer.GetString(MessageConstants.Auth.RefreshTokenSuccess);
            return OkResponse<RefreshTokenResponseDTO>(result, message);
        }

        /// <summary>
        /// Register new user account
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(BaseAPIResponse<RegisterResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<RegisterResponseDTO>>> Register([FromBody] RegisterRequestDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            var message = _localizer.GetString(MessageConstants.Auth.RegisterSuccess);
            return OkResponse<RegisterResponseDTO>(result, message);
        }

        /// <summary>
        /// Logout user account
        /// </summary>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(typeof(BaseAPIResponse<LogoutResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BaseAPIResponse<LogoutResponseDTO>>> Logout([FromBody] LogoutRequestDTO? request)
        {
            var req = request ?? new LogoutRequestDTO();
            var result = await _authService.LogoutAsync(req);
            Response.Cookies.Delete(
                "UniCore_RefreshToken",
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/api/v1/auth",
                }
            );
            var message = _localizer.GetString(MessageConstants.Auth.LogoutSuccess);
            return OkResponse<LogoutResponseDTO>(result, message);
        }

        /// <summary>
        /// Send OTP for email verification
        /// </summary>
        [HttpPost("send-otp")]
        [ProducesResponseType(typeof(BaseAPIResponse<SendOtpResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<SendOtpResponseDTO>>> SendOtp([FromBody] SendOtpRequestDTO request)
        {
            var result = await _authService.SendOtpAsync(request);
            var message = _localizer.GetString(MessageConstants.Auth.SendOtpSuccess);
            return OkResponse<SendOtpResponseDTO>(result, message);
        }

        /// <summary>
        /// Verify email with OTP (Valid-Email)
        /// </summary>
        [Authorize]
        [HttpPost("verify-otp")]
        [HttpPost("valid-email")]
        [ProducesResponseType(typeof(BaseAPIResponse<VerifyOtpResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<VerifyOtpResponseDTO>>> VerifyOtp([FromBody] VerifyOtpRequestDTO request)
        {
            var result = await _authService.VerifyOtpAsync(request);
            var message = _localizer.GetString(MessageConstants.Auth.VerifyOtpSuccess);
            return OkResponse<VerifyOtpResponseDTO>(result, message);
        }

        /// <summary>
        /// Request OTP for Forgot Password
        /// </summary>
        [Authorize]
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(BaseAPIResponse<ForgotPasswordResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<ForgotPasswordResponseDTO>>> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
        {
            var result = await _authService.ForgotPasswordAsync(request);
            var message = _localizer.GetString(MessageConstants.Auth.ForgotPasswordSuccess);
            return OkResponse<ForgotPasswordResponseDTO>(result, message);
        }

        /// <summary>
        /// Reset Password using OTP
        /// </summary>
        [Authorize]
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(BaseAPIResponse<ResetPasswordResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseAPIResponse<ResetPasswordResponseDTO>>> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            var message = _localizer.GetString(MessageConstants.Auth.ResetPasswordSuccess);
            return OkResponse<ResetPasswordResponseDTO>(result, message);
        }

        /// <summary>
        /// Get currently authenticated user profile
        /// </summary>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(BaseAPIResponse<GetMeResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BaseAPIResponse<GetMeResponseDTO>>> GetMe()
        {
            var userId = User.FindFirst(AuthConstants.Claims.UserId)?.Value
                         ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                         ?? User.FindFirst(ClaimTypes.Email)?.Value
                         ?? string.Empty;

            if (string.IsNullOrEmpty(userId))
            {
                var errMessage = _localizer.GetString(MessageConstants.Auth.IdentityNotFound);
                return UnauthorizedResponse<GetMeResponseDTO>(errMessage);
            }

            var result = await _authService.GetMeAsync(userId);
            if (result == null)
            {
                var notFoundMessage = _localizer.GetString(MessageConstants.Auth.UserNotFound);
                return NotFoundResponse<GetMeResponseDTO>(notFoundMessage);
            }

            var successMessage = _localizer.GetString(MessageConstants.Auth.GetMeSuccess);
            return OkResponse<GetMeResponseDTO>(result, successMessage);
        }
    }
}