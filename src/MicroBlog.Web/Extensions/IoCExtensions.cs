using Ganss.Xss;

namespace MicroBlog.Web.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection AddHtmlSanitizer(this IServiceCollection services)
        {
            var allowedIframeHosts = new HashSet<string>
            {
                "www.youtube.com",
                "youtube.com",
                "www.youtu.be",
                "youtu.be"
            };

            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Add("iframe");
            sanitizer.AllowedAttributes.Add("class");
            sanitizer.AllowedAttributes.Add("src");
            sanitizer.AllowedAttributes.Add("width");
            sanitizer.AllowedAttributes.Add("height");
            sanitizer.AllowedAttributes.Add("frameborder");
            sanitizer.AllowedAttributes.Add("allowfullscreen");

            sanitizer.RemovingAttribute += (s, e) =>
            {
                if (e.Tag.TagName == "iframe" && e.Attribute.Name == "src")
                {
                    var uri = new Uri(e.Attribute.Value);
                    if (allowedIframeHosts.Contains(uri.Host))
                    {
                        e.Cancel = true;
                    }
                }
            };

            services.AddSingleton<IHtmlSanitizer>(sanitizer);
            return services;
        }
    }
}
