namespace ProjectOAuth2.Models;
public record GoogleTokenResponse(
     string access_token,
     int expires_in,
     string scope,
     string token_type,
     string id_token
    );

