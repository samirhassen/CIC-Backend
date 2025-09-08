using CIC.API.DTO.RequestDTO;
using CIC.API.DTO.ResponseDTO;
using CIC.API.Repository;

namespace CIC.API.Service
{
    public class LoginService: ILoginService
    {
        private readonly ILoginRepository _iLoginRepository;
        public LoginService(ILoginRepository iLoginRepository)
        {
            _iLoginRepository = iLoginRepository;
        }
        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login)
        {
           return await _iLoginRepository.LoginAsync(login);
        }
    }
}
