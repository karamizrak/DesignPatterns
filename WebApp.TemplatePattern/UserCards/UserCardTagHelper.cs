using Microsoft.AspNetCore.Razor.TagHelpers;
using WebApp.TemplatePattern.Models;

namespace WebApp.TemplatePattern.UserCards
{
    public class UserCardTagHelper:TagHelper
    {

        public AppUser AppUser { get; set; }
        private readonly IHttpContextAccessor _contextAccessor;
        public UserCardTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            UserCardTemplate userCardTemplate;
            if (_contextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                userCardTemplate = new PrimeUserCardTemplate();
            }
            else
            {
                userCardTemplate = new DefaultUserCardTemplate();
            }
            userCardTemplate.SetUser(AppUser);
            output.Content.SetHtmlContent(userCardTemplate.Build());
        }
    }
}
