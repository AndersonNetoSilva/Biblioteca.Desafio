using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Biblioteca.WebApp.TagHelpers
{
    [HtmlTargetElement("breadcrumb")]
    public class BreadcrumbTagHelper : TagHelper
    {
        /// <summary>
        /// Itens do breadcrumb separados por vírgula
        /// </summary>
        public string? Items { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "breadcrumb");
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Content.AppendHtml("<ol class=\"breadcrumb bg-light p-2 rounded fs-5 mb-3\">");

            var items = Items?.Split(',').Select(i => i.Trim()).ToArray() ?? Array.Empty<string>();

            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];

                if (i == items.Length - 1)
                {
                    // último item → ativo
                    output.Content.AppendHtml($"<li class=\"breadcrumb-item active\" aria-current=\"page\">{item}</li>");
                }
                else
                {
                    // itens anteriores → cor de link
                    output.Content.AppendHtml($"<li class=\"breadcrumb-item\" style=\"color:var(--cor-fonte-global);\">{item}</li>");
                }
            }

            output.Content.AppendHtml("</ol>");
        }
    }
}
