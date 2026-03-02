using TaskManagerAPI.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Business.DTOs.Auth;
using TaskManager.Business.Interfaces;

namespace TaskManager.Business.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public AuthService(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
    {
        // 1. Verifica se o email já está em uso
        if (await _userRepository.EmailExistsAsync(registerDTO.Email))
        {
            throw new Exception("Email already in use.");
        }

        // 2. Cria o hash da senha
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);

        // 3. Cria o usuário
        var user = new User
        {
            Name = registerDTO.Name,
            Email = registerDTO.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };

        // 4. Salva o usuário no banco de dados
        var createdUser = await _userRepository.CreateAsync(user);

        // 5. Gera o token JWT
        var token = _tokenService.GenerateToken(createdUser);

        // 6. Retorna os dados do usuário e o token
        return new AuthResponseDTO
        {
            Id = createdUser.Id,
            Name = createdUser.Name,
            Email = createdUser.Email,
            Token = token
        };
    }

    public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
    {
        // 1. Busca o usuário pelo email
        var user = await _userRepository.GetByEmailAsync(loginDTO.Email);

        if (user == null)
        {
            throw new Exception("Invalid email or password.");
        }

        // 2. Verifica a senha
        if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password.");
        }

        // 3. Gera o token JWT
        var token = _tokenService.GenerateToken(user);

        // 4. Retorna os dados do usuário e o token
        return new AuthResponseDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Token = token
        };

    }
}
