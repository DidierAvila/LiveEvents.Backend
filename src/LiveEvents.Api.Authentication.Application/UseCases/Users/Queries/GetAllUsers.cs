using AutoMapper;
using LiveEvents.Api.Authentication.Application.UseCases.Users.Dtos;
using LiveEvents.Api.Domain.Entities.Authentication;
using LiveEvents.Api.Domain.Ports;

namespace LiveEvents.Api.Authentication.Application.UseCases.Users.Queries;

public class GetAllUsers
{
    private readonly IRepositoryBase<User> _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsers(IRepositoryBase<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> HandleAsync(CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAll(cancellationToken);

        // Map collection of Entities to DTOs using AutoMapper
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

        // Asignar UserTypeName manualmente
        foreach (var userDto in userDtos)
        {
            userDto.UserTypeName = "Admin";
        }

        return userDtos;
    }
}
