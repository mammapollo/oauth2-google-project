using ProjectOAuth2.Models;
using System.Text.Json;

namespace ProjectOAuth2.Services
{
    public class GoogleAuthenticationService : IGoogleAuthenticationService
    {

        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string googleClientID;
        private readonly string googleClientSecret;

        public GoogleAuthenticationService(IHttpClientFactory clientFactory, IConfiguration config, IHttpContextAccessor httpContext)
        {
            _configuration = config;
            _httpContext = httpContext;
            _clientFactory = clientFactory;
            googleClientID = _configuration.GetValue<string>("googleClientID");
            googleClientSecret = _configuration.GetValue<string>("googleClientSecret");
        }

        public string GetAuthenticationURL()
        {

            // Specify the required scopes
            string[] scopes = new string[]
            {
                "profile",
                "email",
                "openid"
            };

            return $"https://accounts.google.com/o/oauth2/v2/auth?client_id={googleClientID}" +
                $"&response_type=code&scope={string.Join(" ", scopes)}&redirect_uri={Uri.EscapeDataString(ComposeAndGetBaseUrl())}";
        }

        public async Task<UserInformationOutputDto> GetUserInformation(string code)
        {
            HttpClient httpClient = _clientFactory.CreateClient("GoogleAuthApi");

            GoogleTokenResponse? tokenResponse = await GetToken(httpClient, code);

            if (tokenResponse == null)
            {
                throw new ArgumentNullException(nameof(tokenResponse));
            }

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.access_token);
            HttpResponseMessage userInfoResponse = await httpClient.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo");

            userInfoResponse.EnsureSuccessStatusCode();

            string userInfoJson = await userInfoResponse.Content.ReadAsStringAsync();
            UserInformationOutputDto? userInfo = JsonSerializer.Deserialize<UserInformationOutputDto>(userInfoJson);

            if (userInfo == null)
            {
                throw new ArgumentNullException(nameof(userInfo));
            }

            return userInfo;

        }

        private string ComposeAndGetBaseUrl()
        {
            if (_httpContext?.HttpContext?.Request != null)
            {
                var request = _httpContext.HttpContext.Request;
                var baseUrl = $"{request.Scheme}://{request.Host}/{request.PathBase.ToUriComponent()}";

                return $"{baseUrl}{_configuration.GetValue<string>("googleRedirectUri")}";
            }

            throw new ArgumentNullException("Http context is null");

        }

        private async Task<GoogleTokenResponse?> GetToken(HttpClient httpClient, string code)
        {
            FormUrlEncodedContent requestBody = new FormUrlEncodedContent(new[]
           {
            new KeyValuePair<string, string>("client_id", googleClientID),
            new KeyValuePair<string, string>("client_secret", googleClientSecret),
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("redirect_uri", ComposeAndGetBaseUrl()),
            });

            HttpResponseMessage response = await httpClient.PostAsync("https://oauth2.googleapis.com/token", requestBody);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GoogleTokenResponse?>(json);
        }



    }
}
