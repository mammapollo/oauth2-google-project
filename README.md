# Google OAuth2 Authentication With .Net Core Web Api

The project uses Asp.Net Core Web Api (.Net Core 6) to manage the authentication flow through OAuth2 with Google.

# Prerequisites

To be able to authenticate via this application you need to create an application via the Google portal by following this guide https://developers.google.com/identity/protocols/oauth2?hl=it

Once the process has been completed and permissions have been provided to the application created, we will obtain our Google Client ID and Google Client Secret.

Once obtained, to correctly run the application it is necessary to change the values ​​in the appsetting.json.

![immagine](https://github.com/mammapollo/oauth2-google-project/assets/110206243/0b71cb26-23fe-496b-be43-7879e33ba67d)

:gun: Attention! It is very important to define the landing URL for our google application. In this case, <endpoint>/api/authentication/callback was used.
This is to ensure that once the authentication process has passed the first phase we can be taken to another endpoint.
This endpoint will take the incoming secret token and use it to conclude the authentication operation.

# How to run

After creating the resources and configuring everything correctly, you need to navigate via browser to the endpoint <endpoint>/api/Authentication/login.

A Google authentication screen will appear, you will be redirected to that specific page.

![immagine](https://github.com/mammapollo/oauth2-google-project/assets/110206243/d91f2cc9-2bf2-4387-b5bb-3325ad3171f4)

You will then need to authenticate with your Google account. 

Once authenticated, a secret token will be retrieved from OAuth2 and you will be redirected to the endpoint we previously configured as a return.

The routine will take care of recovering the data associated with the account using the token that was sent by the previous request.

If no problem occurred, an html page will be populated and served in response to the user. The page will contain the main information for each Google user, such as name, surname, email etc..

(Sorry for the nasty deletions of sensitive information :telescope::telescope::telescope::telescope:)

![immagine](https://github.com/mammapollo/oauth2-google-project/assets/110206243/c26351a5-ab2e-403a-9cb7-427338d6f27d)

