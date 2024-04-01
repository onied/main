using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Users.Dtos;
using Users.Dtos.VkUserInfoResponseDtos;
using Users.Services.UserCreatedProducer;

namespace Users.Controllers;

[ApiController]
[Route("/api/v1/[action]")]
public class OauthController(IUserCreatedProducer userCreatedProducer)
{
    [HttpPost]
    public async Task<IResult> SigninVk([FromBody] OauthCodeDto oauthCodeDto,
        [FromServices] IHttpClientFactory clientFactory, [FromServices] IConfiguration configuration,
        [FromServices] SignInManager<AppUser> signInManager, [FromServices] UserManager<AppUser> userManager,
        [FromServices] IUserStore<AppUser> userStore)
    {
        var client = clientFactory.CreateClient();
        var url = new UriBuilder(configuration.GetConnectionString("VkAccessTokenUri")!);
        var query = HttpUtility.ParseQueryString(url.Query);
        var vkConfig = configuration.GetSection("Authentication:VK");
        query.Add("client_id", vkConfig["ClientId"]!);
        query.Add("client_secret", vkConfig["ClientSecret"]!);
        query.Add("redirect_uri", oauthCodeDto.RedirectUri);
        query.Add("code", oauthCodeDto.Code);
        url.Query = query.ToString();
        var response = await client.GetAsync(url.ToString());
        if (!response.IsSuccessStatusCode)
            return Results.Unauthorized();
        var responseContent = await response.Content.ReadAsStreamAsync();
        var accessTokenResponse = await JsonSerializer.DeserializeAsync<VkAccessTokenResponse>(responseContent);
        if (accessTokenResponse?.Email == null)
            return Results.BadRequest(accessTokenResponse?.ErrorDescription);

        var user = await userManager.FindByEmailAsync(accessTokenResponse.Email);
        if (user == null)
        {
            url = new UriBuilder(configuration.GetConnectionString("VkGetProfileInfoMethodUri")!);
            query = HttpUtility.ParseQueryString(url.Query);
            query.Add("access_token", accessTokenResponse.AccessToken);
            query.Add("v", configuration["VkApiVersion"]);
            url.Query = query.ToString();
            response = await client.GetAsync(url.ToString());
            if (!response.IsSuccessStatusCode)
                return Results.Unauthorized();
            responseContent = await response.Content.ReadAsStreamAsync();
            var userInfoResponseWrapper =
                await JsonSerializer.DeserializeAsync<UserInfoResponseWrapper>(responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (userInfoResponseWrapper?.Response == null)
                return Results.Unauthorized();
            user = new AppUser();
            await userStore.SetUserNameAsync(user, accessTokenResponse.Email, CancellationToken.None);
            var emailStore = (IUserEmailStore<AppUser>)userStore;
            await emailStore.SetEmailAsync(user, accessTokenResponse.Email, CancellationToken.None);
            user.Gender = userInfoResponseWrapper.Response.Sex switch
            {
                1 => Gender.Female,
                2 => Gender.Male,
                _ => Gender.Other
            };
            user.FirstName = userInfoResponseWrapper.Response.FirstName;
            user.LastName = userInfoResponseWrapper.Response.LastName;
            user.Avatar = userInfoResponseWrapper.Response.Photo200;
            var result = await userManager.CreateAsync(user);

            if (!result.Succeeded) return Results.Unauthorized();
        }

        signInManager.AuthenticationScheme = IdentityConstants.BearerScheme;

        await signInManager.SignInAsync(user, false);
        await userCreatedProducer.PublishAsync(user);
        return Results.Empty;
    }
}
