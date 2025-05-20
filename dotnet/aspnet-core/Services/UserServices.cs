using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

public class UserService : IUserService
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JWT _jwt;

    public UserService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _jwt = jwt.Value;
    }

    public async Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model)
    {
        var authenModel = new AuthenticationModel();
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            authenModel.IsAuthenticated = false;
            authenModel.Message = $"No Accounts Registered with{model.Email}";
            return authenModel;

        }

        if (await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authenModel.IsAuthenticated = true;
            JwtSecurityToken jwtToken = await CreateJwtToken(user);
            authenModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authenModel.Email = user.Email;
            authenModel.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            authenModel.Roles = rolesList.ToList();
            return authenModel;
        }

        authenModel.IsAuthenticated = false;
        authenModel.Message = $"Incorrect credentials for user {user.Email}";
        return authenModel;
    }

    public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        if (Encoding.UTF8.GetByteCount(_jwt.Key) < 32)
        {
            throw new ArgumentException("JWT key must be at least 32 bytes (256 bits) for HS256.");
        }

        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();
        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }
        var claims = new[]{
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id),
        }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        var jwtSecurityToken = new JwtSecurityToken(
            _jwt.Issuer,
            _jwt.Audience,
            claims,
            notBefore: null,
            DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
            signingCredentials);

        return jwtSecurityToken;
    }

    public async Task<string> RegisterAsync(RegisterModel model)
    {
        var userModel = new ApplicationUser
        {
            UserName = model.UserName,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
        };

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            var result = await _userManager.CreateAsync(userModel, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userModel, Authorization.default_role.ToString());
            }
            return $"User registered with username {userModel.UserName}";
        }
        else
        {
            return $"Email {user.Email} is already registered";
        }
    }
}

public interface IUserService
{
    Task<string> RegisterAsync(RegisterModel model);
    Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
}