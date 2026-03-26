using AutoMapper;
using TaskManager.Business.DTOs.Boards;
using TaskManager.Business.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Business.Services;

public class BoardMemberService : IBoardMemberService
{
    private readonly IBoardMemberRepository _boardMemberRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public BoardMemberService(IBoardMemberRepository boardMemberRepository, IMapper mapper, IBoardRepository boardRepository, IUserRepository userRepository)
    {
        _boardMemberRepository = boardMemberRepository;
        _boardRepository = boardRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<BoardMemberDTO> GetByBoardAndUserAsync(Guid boardId, Guid userId)
    {
        var member = await _boardMemberRepository.GetByBoardAndUserAsync(boardId, userId);

        if (member == null)
            throw new Exception("Member not found.");

        return _mapper.Map<BoardMemberDTO>(member);
    }

    public async Task<IEnumerable<BoardMemberDTO>> GetAllByBoardIdAsync(Guid boardId)
    {
        var members = await _boardMemberRepository.GetAllByBoardIdAsync(boardId);

        return _mapper.Map<IEnumerable<BoardMemberDTO>>(members);
    }

    public async Task<BoardMemberDTO> CreateAsync(Guid boardId, AddMemberDTO addMemberDTO, Guid userId)
    {
        var board = await _boardRepository.GetByIdAsync(boardId);

        if (board == null)
            throw new Exception("Board not found");

        if (board.UserOwnerId != userId)
            throw new Exception("Only board owner can add members");

        var existingMember = await _boardMemberRepository.VerifyUserBoardLinked(userId);

        if (existingMember)
            throw new Exception("User already a member of this board");

        var member = new BoardMember
        {
            Id = Guid.NewGuid(),
            BoardId = boardId,
            UserId = userId,
            Role = addMemberDTO.Role,
            AddedAt = DateTime.UtcNow
        };

        var createdMember = await _boardMemberRepository.CreateAsync(member);

        return new BoardMemberDTO
        {
            Id = createdMember.Id,
            UserId = createdMember.UserId,
            UserName = user.Name,
            UserEmail = user.Email,
            Role = createdMember.Role,
            AddedAt = createdMember.AddedAt
        };
    }

    public async Task<bool> DeleteAsync(Guid boardId, Guid memberId, Guid userId)
    {
        if (!await _boardMemberRepository.IsOwnerAsync(boardId, userId))
            throw new Exception("Only board owner can remove members");

        var member = await _boardMemberRepository.GetByBoardAndUserAsync(boardId, memberId);
        if (member == null)
            return false;

        if (member.Role == BoardRole.Owner)
            throw new Exception("Cannot remove board owner");

        return await _boardMemberRepository.DeleteAsync(member.Id);
    }

    public async Task<bool> IsOwnerAsync(Guid boardId, Guid userId)
    {
        return await _boardMemberRepository.IsOwnerAsync(boardId, userId);
    }
}
