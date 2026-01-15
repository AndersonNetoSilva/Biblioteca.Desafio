using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace Biblioteca.WebApp.TagHelpers
{

    namespace Biblioteca.WebApp.TagHelpers
    {
        [HtmlTargetElement("menu")]
        public class MenuTagHelper : TagHelper
        {
            [HtmlAttributeName("items")]
            public IEnumerable<MenuItem> Items { get; set; } = Enumerable.Empty<MenuItem>();

            [HtmlAttributeName("root-class")]
            public string RootClass { get; set; } = "navbar-nav flex-grow-1";

            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                output.TagName = "ul";
                output.Attributes.SetAttribute("class", RootClass);

                var sb = new StringBuilder();

                foreach (var item in Items)
                {
                    sb.AppendLine(RenderItem(item, isRoot: true));
                }

                output.Content.SetHtmlContent(sb.ToString());
            }

            private string RenderItem(MenuItem item, bool isRoot)
            {
                bool hasChildren = item.Children != null && item.Children.Count > 0;
                var id = "menu_" + Guid.NewGuid().ToString("N");

                var sb = new StringBuilder();
                var encodedTitle = HtmlEncode(item.Title);
                var href = !string.IsNullOrWhiteSpace(item.Url) ? HtmlEncodeAttr(item.Url!) : "#";
                var disabledClass = item.Disabled ? " disabled" : "";

                if (hasChildren)
                {
                    if (isRoot)
                    {
                        // top-level dropdown
                        sb.AppendLine($@"<li class=""nav-item dropdown"">");
                        sb.AppendLine($@"  <a class=""nav-link dropdown-toggle text-dark"" href=""#"" id=""{id}"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">");
                        if (!string.IsNullOrEmpty(item.IconClass))
                            sb.AppendLine($@"    <i class=""{HtmlEncodeAttr(item.IconClass)} me-1""></i>");
                        sb.AppendLine($"    {encodedTitle}");
                        sb.AppendLine("  </a>");
                        sb.AppendLine($@"  <ul class=""dropdown-menu"" aria-labelledby=""{id}"">");

                        foreach (var child in item.Children)
                        {
                            sb.AppendLine(RenderItem(child, isRoot: false));
                        }

                        sb.AppendLine("  </ul>");
                        sb.AppendLine("</li>");
                    }
                    else
                    {
                        // nested dropdown (submenu)
                        sb.AppendLine($@"<li class=""dropdown-submenu"">");
                        sb.AppendLine($@"  <a class=""dropdown-item dropdown-toggle"" href=""#"" id=""{id}"" role=""button"" data-bs-toggle=""dropdown"" aria-expanded=""false"">");
                        if (!string.IsNullOrEmpty(item.IconClass))
                            sb.AppendLine($@"    <i class=""{HtmlEncodeAttr(item.IconClass)} me-1""></i>");
                        sb.AppendLine($"    {encodedTitle}");
                        sb.AppendLine("  </a>");
                        sb.AppendLine($@"  <ul class=""dropdown-menu"" aria-labelledby=""{id}"">");

                        foreach (var child in item.Children)
                            sb.AppendLine(RenderItem(child, isRoot: false));

                        sb.AppendLine("  </ul>");
                        sb.AppendLine("</li>");
                    }
                }
                else
                {
                    if (isRoot)
                    {
                        // root simple item
                        sb.AppendLine("<li class=\"nav-item\">");
                        sb.AppendLine($@"  <a class=""nav-link text-dark{disabledClass}"" href=""{href}"">");
                        if (!string.IsNullOrEmpty(item.IconClass))
                            sb.AppendLine($@"    <i class=""{HtmlEncodeAttr(item.IconClass)} me-1""></i>");
                        sb.AppendLine($"    {encodedTitle}");
                        sb.AppendLine("  </a>");
                        sb.AppendLine("</li>");
                    }
                    else
                    {
                        // dropdown item
                        sb.AppendLine($@"<li><a class=""dropdown-item{disabledClass}"" href=""{href}"">");
                        if (!string.IsNullOrEmpty(item.IconClass))
                            sb.AppendLine($@"    <i class=""{HtmlEncodeAttr(item.IconClass)} me-1""></i>");
                        sb.AppendLine($"    {encodedTitle}");
                        sb.AppendLine("</a></li>");
                    }
                }

                return sb.ToString();
            }

            private static string HtmlEncode(string s) => System.Net.WebUtility.HtmlEncode(s ?? "");
            private static string HtmlEncodeAttr(string s) => System.Net.WebUtility.HtmlEncode(s ?? "");
        }
    }

    public class MenuItem
    {
        public string Title { get; set; } = "";
        public string? Url { get; set; }           // href
        public string? IconClass { get; set; }     // opcional: "bi bi-house"
        public bool Disabled { get; set; } = false;
        public List<MenuItem> Children { get; set; } = new();
    }

}
