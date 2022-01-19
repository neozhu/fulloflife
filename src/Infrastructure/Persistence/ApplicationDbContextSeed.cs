// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Razor.Application.Common.Extensions;
using CleanArchitecture.Razor.Infrastructure.Constants.Role;
using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace CleanArchitecture.Razor.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        var administratorRole = new ApplicationRole(RoleConstants.AdministratorRole) { Description = "Admin Group" };
        var userRole = new ApplicationRole(RoleConstants.BasicRole) { Description = "Basic Group" };

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
            await roleManager.CreateAsync(userRole);
            var Permissions = GetAllPermissions();
            foreach (var permission in Permissions)
            {
                await roleManager.AddClaimAsync(administratorRole, new System.Security.Claims.Claim(ApplicationClaimTypes.Permission, permission));
            }
        }

        var administrator = new ApplicationUser { UserName = "administrator", IsActive = true, Site = "Razor", DisplayName = "Administrator", Email = "new163@163.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://cn.gravatar.com/avatar/{"new163@163.com".ToMD5()}?s=120&d=retro" };
        var demo = new ApplicationUser { UserName = "Demo", IsActive = true, Site = "Razor", DisplayName = "Demo", Email = "neozhu@126.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://cn.gravatar.com/avatar/{"neozhu@126.com".ToMD5()}?s=120&d=retro" };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, RoleConstants.DefaultPassword);
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            await userManager.CreateAsync(demo, RoleConstants.DefaultPassword);
            await userManager.AddToRolesAsync(demo, new[] { userRole.Name });


        }

    }
    private static IEnumerable<string> GetAllPermissions()
    {
        var allPermissions = new List<string>();
        var modules = typeof(Permissions).GetNestedTypes();

        foreach (var module in modules)
        {
            var moduleName = string.Empty;
            var moduleDescription = string.Empty;

            var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var fi in fields)
            {
                var propertyValue = fi.GetValue(null);

                if (propertyValue is not null)
                {
                    allPermissions.Add(propertyValue.ToString());
                }
            }
        }

        return allPermissions;
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        //Seed, if necessary
        if (!context.DocumentTypes.Any())
        {
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Document", Description = "Document" });
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "PDF", Description = "PDF" });
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Image", Description = "Image" });
            context.DocumentTypes.Add(new Domain.Entities.DocumentType() { Name = "Other", Description = "Other" });
            await context.SaveChangesAsync();
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Initial add document types", Level = "Information", UserName = "System", TimeStamp = System.DateTime.Now });
            await context.SaveChangesAsync();
        }
        if (!context.KeyValues.Any())
        {
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "initialization", Text = "initialization", Description = "Status of workflow" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "processing", Text = "processing", Description = "Status of workflow" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "pending", Text = "pending", Description = "Status of workflow" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Status", Value = "finished", Text = "finished", Description = "Status of workflow" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "CNC", Text = "CNC", Description = "Region of Customer" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "CNN", Text = "CNN", Description = "Region of Customer" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "CNS", Text = "CNS", Description = "Region of Customer" });
            context.KeyValues.Add(new Domain.Entities.KeyValue() { Name = "Region", Value = "Oversea", Text = "Oversea", Description = "Region of Customer" });
            await context.SaveChangesAsync();
            context.Loggers.Add(new Domain.Entities.Log.Logger() { Message = "Initial add key values", Level = "Information", UserName = "System", TimeStamp = System.DateTime.Now });
            await context.SaveChangesAsync();
        }
        if (!context.Customers.Any())
        {
            context.Customers.Add(new Domain.Entities.Customer() { Name = "SmartAdmin", AddressOfEnglish = "https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0", GroupName = "SmartAdmin", Address = "https://wrapbootstrap.com/theme/smartadmin-responsive-webapp-WB0573SK0", Sales = "GotBootstrap", RegionSalesDirector = "GotBootstrap", Region = "CNC", NameOfEnglish = "SmartAdmin", PartnerType = Domain.Enums.PartnerType.TP, Contact = "GotBootstrap", Email = "drlantern@gotbootstrap.com" });
            await context.SaveChangesAsync();

         
        }
        if (!context.Categories.Any())
        {
            context.Categories.Add(new Domain.Entities.Category() { Name = "蔬菜", Description = "自产自销", Icon = "https://fulloflifeimg.dotnet6.cn/product_catalog_637781721342302652_vegetable.jpg", Sequence = 1 });
            context.Categories.Add(new Domain.Entities.Category() { Name = "蛋类", Description = "自产自销", Icon = "https://fulloflifeimg.dotnet6.cn/product_catalog_637781723056165385_egg.png", Sequence = 2 });
            context.Categories.Add(new Domain.Entities.Category() { Name = "腌菜", Description = "自产自销", Icon = "https://fulloflifeimg.dotnet6.cn/product_catalog_637781723241360428_pickles.jpg", Sequence = 2 });
            await context.SaveChangesAsync();
        }
    }
}
