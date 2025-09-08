using CIC.API.DTO.RequestDTO;
using CIC.API.DTO.ResponseDTO;

namespace CIC.API.Repository
{
    public interface ILoginRepository
    {
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login);
    }
}
