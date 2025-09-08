using CIC.API.DTO.RequestDTO;
using CIC.API.DTO.ResponseDTO;

namespace CIC.API.Service
{
    public interface ILoginService
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login);
    }
}
