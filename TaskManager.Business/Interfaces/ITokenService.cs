using TaskManager.Domain.Entities;

namespace TaskManager.Business.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}
