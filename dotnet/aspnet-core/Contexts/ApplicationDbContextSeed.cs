using Microsoft.AspNetCore.Identity;

public class ApplicationDbContextSeed
{
    public static async Task SeedEssensialDataAsync(UserManager<ApplicationUser> userManger, RoleManager<IdentityRole> roleManager)
    {
        // Seed roles
        await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Administrator.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.Moderator.ToString()));
        await roleManager.CreateAsync(new IdentityRole(Authorization.Roles.User.ToString()));

        // Seed default user
        var defaultUser = new ApplicationUser
        {
            UserName = Authorization.default_username,
            Email = Authorization.default_email,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            FirstName = "Trung",
            LastName = "Le"
        };

        if (userManger.Users.All(u => u.Id != defaultUser.Id))
        {
            await userManger.CreateAsync(defaultUser, Authorization.default_password);
            await userManger.AddToRoleAsync(defaultUser, Authorization.default_role.ToString());
        }
    }
}