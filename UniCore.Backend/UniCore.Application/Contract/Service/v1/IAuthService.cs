using UniCore.Application.Feature.v1.Auth.ForgotPassword;
using UniCore.Application.Feature.v1.Auth.Login;
using UniCore.Application.Feature.v1.Auth.Logout;
using UniCore.Application.Feature.v1.Auth.Me;
using UniCore.Application.Feature.v1.Auth.RefreshToken;
using UniCore.Application.Feature.v1.Auth.Register;
using UniCore.Application.Feature.v1.Auth.VerifyOtp;

namespace UniCore.Application.Contract.Service.v1
{
    public interface IAuthService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request, CancellationToken cancellationToken = default);
        Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO request, CancellationToken cancellationToken = default);
        Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request, CancellationToken cancellationToken = default);
        Task<LogoutResponseDTO> LogoutAsync(LogoutRequestDTO request, CancellationToken cancellationToken = default);
        Task<SendOtpResponseDTO> SendOtpAsync(SendOtpRequestDTO request, CancellationToken cancellationToken = default);
        Task<VerifyOtpResponseDTO> VerifyOtpAsync(VerifyOtpRequestDTO request, CancellationToken cancellationToken = default);
        Task<ForgotPasswordResponseDTO> ForgotPasswordAsync(ForgotPasswordRequestDTO request, CancellationToken cancellationToken = default);
        Task<ResetPasswordResponseDTO> ResetPasswordAsync(ResetPasswordRequestDTO request, CancellationToken cancellationToken = default);
        Task<GetMeResponseDTO?> GetMeAsync(string userId, CancellationToken cancellationToken = default);
    }
}
