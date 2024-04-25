namespace ProjectOAuth2.Models;
public class UserInformationOutputDto
{
    public string id { get; set; }
    public string email { get; set; }
    public bool verified_email { get; set; }
    public string name { get; set; }
    public string given_name { get; set; }
    public string family_name { get; set; }
    public string picture { get; set; }
}


