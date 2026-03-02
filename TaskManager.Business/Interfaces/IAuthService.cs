using TaskManager.Business.DTOs.Auth;

namespace TaskManager.Business.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO);
    Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
}
