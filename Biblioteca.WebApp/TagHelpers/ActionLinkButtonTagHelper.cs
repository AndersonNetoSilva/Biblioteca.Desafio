using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Biblioteca.WebApp.TagHelpers
{
    [HtmlTargetElement("action-link-button", Attributes = "action")]
    public class ActionLinkButtonTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public ActionLinkButtonTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [HtmlAttributeName("page")]
        public string Page { get; set; } = "#";

        [HtmlAttributeName("action")]
        public ButtonActionType? Action { get; set; }

        [HtmlAttributeName("route-id")]
        public object? RouteId { get; set; } = null;

        [HtmlAttributeName("icon")]
        public string Icon { get; set; } = "bi-plus-circle";

        [HtmlAttributeName("tooltip")]
        public string Tooltip { get; set; } = "Ação";

        [HtmlAttributeName("color")]
        public string Color { get; set; } = "primary";

        [HtmlAttributeName("th")]
        public bool RenderAsTh { get; set; } = true;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } = default!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Action.HasValue)
            {
                switch (Action.Value)
                {
                    case ButtonActionType.Create:
                        Page = "./Create";
                        Icon = "bi-plus-circle";
                        Tooltip = "Inserir";
                        Color = "primary";
                        RenderAsTh = true;
                        break;
                    case ButtonActionType.Edit:
                        Page = "./Edit";
                        Icon = "bi-pencil";
                        Tooltip = "Alterar";
                        Color = "secondary";
                        RenderAsTh = false;
                        break;
                    case ButtonActionType.Delete:
                        Page = "./Delete";
                        Icon = "bi-trash";
                        Tooltip = "Excluir";
                        Color = "danger";
                        RenderAsTh = false;
                        break;
                    case ButtonActionType.Details:
                        Page = "./Details";
                        Icon = "bi-card-text";
                        Tooltip = "Detalhes";
                        Color = "info";
                        RenderAsTh = false;
                        break;
                    default:
                        break;
                }
            }

            // Define th ou td
            output.TagName = RenderAsTh ? "th" : "td";
            output.Attributes.SetAttribute("class", "text-center text-nowrap");
            output.Attributes.SetAttribute("style", "width:1%");

            // Cria URL
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var href = RouteId != null ? urlHelper.Page(Page, new { id = RouteId }) : urlHelper.Page(Page);

            // Cria <a>
            output.Content.SetHtmlContent($@"
<a href=""{href}"" 
   class=""btn btn-outline-{Color} btn-sm p-1"" 
   data-bs-toggle=""tooltip"" 
   title=""{Tooltip}"">
    <i class=""bi {Icon}""></i>
</a>");
        }
    }

    public enum ButtonActionType
    {
        Custom,
        Create,
        Edit,
        Delete,
        Details
    }
}
