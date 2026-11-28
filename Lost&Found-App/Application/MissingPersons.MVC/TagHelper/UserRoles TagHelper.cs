using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MissingPersons.MVC.TagHelpers;

[HtmlTargetElement("*", Attributes = RolesAttributeName)]
public class UserRolesTagHelper : TagHelper
{
    private const string RolesAttributeName = "user-roles";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserRolesTagHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    [HtmlAttributeName(RolesAttributeName)]
    public string Roles { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.Attributes.RemoveAll(RolesAttributeName);

        var user = _httpContextAccessor.HttpContext?.User;
        var allowedRoles = Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var visible = user?.Identity?.IsAuthenticated == true
                      && allowedRoles.Any(role => user.IsInRole(role));

        if (!visible)
        {
            output.SuppressOutput();
        }
    }
}