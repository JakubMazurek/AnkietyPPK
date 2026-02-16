using Microsoft.AspNetCore.Identity;

namespace AnkietyPPK.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            //tworzenie ról
            string[] roles = { "Ankieter", "Respondent" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //tworzenie przykładowego ankietera
            var ankieterEmail = "ankieter@example.com";
            if (await userManager.FindByEmailAsync(ankieterEmail) == null)
            {
                var ankieter = new IdentityUser
                {
                    UserName = ankieterEmail,
                    Email = ankieterEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(ankieter, "Ankieter123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(ankieter, "Ankieter");
                }
            }

            //tworzenie przykładowego respondenta
            var respondentEmail = "respondent@example.com";
            if (await userManager.FindByEmailAsync(respondentEmail) == null)
            {
                var respondent = new IdentityUser
                {
                    UserName = respondentEmail,
                    Email = respondentEmail,
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(respondent, "Respondent123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(respondent, "Respondent");
                }
            }
        }
    }
}