using Microsoft.AspNetCore.Mvc;
using ProjectOAuth2.Models;
using ProjectOAuth2.Services;
using System.Xml.Linq;

namespace ProjectOAuth2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {

        private readonly IGoogleAuthenticationService _googleAuthenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IGoogleAuthenticationService googleAuthenticationService, ILogger<AuthenticationController> logger)
        {
            _googleAuthenticationService = googleAuthenticationService;
            _logger = logger;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return Redirect(_googleAuthenticationService.GetAuthenticationURL());
        }

        [HttpGet("callback")]
        public async Task<IActionResult> Callback(string code)
        {
            try
            {
                UserInformationOutputDto userInfo = await _googleAuthenticationService.GetUserInformation(code);
                
                string html = System.IO.File.ReadAllText(@"./pages/returnpage.html");

                html = html.Replace("@username", userInfo.name)
                    .Replace("@id", userInfo.id)
                    .Replace("@email", userInfo.email)
                    .Replace("@name", userInfo.name)
                    .Replace("@verified_email", userInfo.verified_email.ToString())
                    .Replace("@given_name", userInfo.given_name)
                    .Replace("@family_name", userInfo.family_name)
                    .Replace("@img_src", userInfo.picture);

                return Content(html, "text/html");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest("Http request exception");
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest("No information provided");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                return BadRequest("Unmanaged exception");
            }
        }
    }

}
