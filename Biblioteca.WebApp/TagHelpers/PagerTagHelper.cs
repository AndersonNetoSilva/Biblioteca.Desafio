using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Biblioteca.WebApp.PageModels;

namespace Biblioteca.WebApp.TagHelpers
{
    [HtmlTargetElement("pager")]
    public class PagerTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PagerTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        public string Page { get; set; } = "./Index";
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Não renderiza nada se não for necessário
            if (TotalPages <= 1)
            {
                output.SuppressOutput();
                return;
            }

            output.TagName = "nav";
            output.Attributes.SetAttribute("aria-label", "Page navigation");

            var ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            // Previous
            ul.InnerHtml.AppendHtml(CreatePageItem("Anterior",
                PageNumber > 1 ? PageNumber - 1 : 1,
                PageNumber <= 1));

            // Page numbers
            for (int i = 1; i <= TotalPages; i++)
            {
                ul.InnerHtml.AppendHtml(CreatePageItem(i.ToString(), i, i == PageNumber, isActive: i == PageNumber));
            }

            // Next
            ul.InnerHtml.AppendHtml(CreatePageItem("Próximo",
                PageNumber < TotalPages ? PageNumber + 1 : TotalPages,
                PageNumber >= TotalPages));

            output.Content.AppendHtml(ul);
        }

        private TagBuilder CreatePageItem(string text, int page, bool disabled = false, bool isActive = false)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            var li = new TagBuilder("li");
            li.AddCssClass("page-item");

            if (disabled)
                li.AddCssClass("disabled");

            if (isActive)
                li.AddCssClass("active");

            var a = new TagBuilder("a");
            a.AddCssClass("page-link");

            if (ViewContext.ViewData.Model is PagerAndFilterPageModel model)
                a.Attributes["href"] = $"{Page}?PageNumber={page}{(string.IsNullOrWhiteSpace(model.Search) ? string.Empty : $"&Search={model.Search}")}";
            else
                a.Attributes["href"] = $"{Page}?PageNumber={page}";

            a.InnerHtml.Append(text);

            li.InnerHtml.AppendHtml(a);
            return li;
        }
    }
}
