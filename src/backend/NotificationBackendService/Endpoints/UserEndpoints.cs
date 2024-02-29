using Microsoft.AspNetCore.Authorization;
using NotificationBackendService.Extentions;
using NotificationBackendService.Models.Dto;
using NotificationBackendService.Services;

namespace NotificationBackendService.Endpoints;

public static class UserEndpoints
{
    /// <summary>
    /// User Endpoints - Login, Logout, Details Etc.
    /// </summary>
    /// <param name="app"></param>
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/details", GetUserDetails)
            .WithName("GetUserDetails").WithDisplayName("Get User Details")
            .Produces<IReadOnlyCollection<GetUserTokenDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("api/users", GetAllUsers)
            .WithName("GetAllUsers").WithDisplayName("Get All Users")
            .Produces<GetUserTokenDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("api/users", UserLoginAsync)
            .WithName("UserToken").WithDisplayName("Get User Token")
            .Produces<GetUserTokenDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);


        app.MapGet("api/users/events", GetAllUserEvents)
            .WithName("GetAllUserEvents").WithDisplayName("Get All User Events")
            .Produces<IReadOnlyCollection<EventItemDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPost("api/users/refreshToken", UserTokenUsingRefreshAsync)
            .WithName("UserTokenUsingRefreshToken").WithDisplayName("Get User Token using RefreshToken")
            .Produces<GetUserTokenDto>(StatusCodes.Status200OK)
            .Produces<ValidationResultDto>(StatusCodes.Status400BadRequest)
            .Produces<ValidationResultDto>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("api/users/logout", UserLogoutAsync)
            .WithName("UserLogout").WithDisplayName("User Logout")
            .Produces<GetUserTokenDto>(StatusCodes.Status200OK)
            .Produces<ValidationResultDto>(StatusCodes.Status400BadRequest)
            .Produces<ValidationResultDto>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Get All Users
    /// </summary>
    /// <param name="userService"></param>
    /// <returns></returns>
    [AllowAnonymous]
    private static async Task<IResult> GetAllUsers(IUserService userService)
    {
        var entities = await userService.GetAllUsersAsync();
        var items = entities.Select(x => x.AsDto());

        return Results.Ok(items);
    }

    /// <summary>
    /// Login Using Username and Password
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="newItem"></param>
    /// <returns></returns>
    [AllowAnonymous]
    private static async Task<IResult> UserLoginAsync(IUserService userService, LoginRequestDto newItem)
    {
        if (string.IsNullOrEmpty(newItem.Username))
            return Results.BadRequest();

        if (string.IsNullOrEmpty(newItem.Password))
            return Results.BadRequest();

        var user = await userService.GetUserAsync(newItem.Username, newItem.Password);

        if (user == null)
            return Results.NotFound();

        var usrDto = user.AsDto();
        usrDto.Token = await userService.LoginAsync(user);

        return Results.Ok(usrDto);
    }

    /// <summary>
    /// Get User Details
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    [Authorize]
    private static async Task<IResult> GetUserDetails(IUserService userService, ICurrentUserIdentity currentUser)
    {
        var user = await userService.GetUserAsync(currentUser.UserName);

        if (user == null)
            return Results.NotFound();

        var usrDto = user.AsDto();
        return Results.Ok(usrDto);
    }
    

    /// <summary>
    /// Get User Details
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    [Authorize]
    private static async Task<IResult> GetAllUserEvents(IUserEventService userService, ICurrentUserIdentity currentUser)
    {
        var userEvents = await userService.GetUserEvents(currentUser.UserId);

        if (userEvents == null)
            return Results.NotFound();

        var usrEvtDto = userEvents.Select(x => x?.Event.AsDto());
        return Results.Ok(usrEvtDto);
    }

    /// <summary>
    /// Generate Token Using Refresh Token
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="tokenService"></param>
    /// <param name="refreshTokenRequest"></param>
    /// <returns></returns>
    [AllowAnonymous]
    public static async Task<IResult> UserTokenUsingRefreshAsync(IUserService userService, ITokenService tokenService, RefreshTokenRequestDto refreshTokenRequest)
    {
        if (refreshTokenRequest == null || string.IsNullOrEmpty(refreshTokenRequest.RefreshToken) || string.IsNullOrEmpty(refreshTokenRequest.Username))
        {
            return Results.BadRequest(new
            {
                Message = "Missing refresh token details",
            });
        }

        var user = await userService.GetUserAsync(refreshTokenRequest.Username);

        if (user == null)
            return Results.NotFound();

        var validateRefreshTokenResponse = await tokenService.ValidateRefreshTokenAsync(refreshTokenRequest, user.Id);
        //if (!validateRefreshTokenResponse.IsSuccess)
        //{
        //    return Results.UnprocessableEntity(validateRefreshTokenResponse);
        //}

        var usrDto = user.AsDto();
        usrDto.Token = await userService.LoginAsync(user);

        return Results.Ok(usrDto);
    }

    /// <summary>
    /// Logout User
    /// </summary>
    /// <param name="userService"></param>
    /// <param name="currentUser"></param>
    /// <returns></returns>
    [Authorize]
    public static async Task<IResult> UserLogoutAsync(IUserService userService, ICurrentUserIdentity currentUser)
    {
        var validateLogoutResponse = await userService.LogoutAsync(currentUser.UserId);
        if (!validateLogoutResponse.IsSuccess)
        {
            return Results.UnprocessableEntity(validateLogoutResponse);
        }

        return Results.Ok(validateLogoutResponse);
    }
}
