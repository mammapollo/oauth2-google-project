using ProjectOAuth2.Models;

namespace ProjectOAuth2.Services
{
    public interface IGoogleAuthenticationService
    {
        public string GetAuthenticationURL();
        public Task<UserInformationOutputDto> GetUserInformation(string code);

    }
}
