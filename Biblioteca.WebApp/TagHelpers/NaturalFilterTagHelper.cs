using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Net;
using System.Text;

namespace Biblioteca.WebApp.TagHelpers
{
    [HtmlTargetElement("natural-filter")]
    public class NaturalFilterTagHelper : TagHelper
    {
        [HtmlAttributeName("fields")]
        public string? Fields { get; set; }

        public string Placeholder { get; set; } = "Pesquisar…";

        public string Example { get; set; } = "Campo:Valor; Campo:Valor";

        public string? Value { get; set; }

        public string Name { get; set; } = "search";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            static string E(string? s) => s is null ? string.Empty : WebUtility.HtmlEncode(s);

            var items = (Fields ?? string.Empty)
                        .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => x.Length > 0)
                        .ToArray();

            var containerId = "nf_" + Guid.NewGuid().ToString("N");
            var inputId = "nf_input_" + Guid.NewGuid().ToString("N");
            var clearBtnId = "nf_clear_" + Guid.NewGuid().ToString("N");

            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("method", "get");
            output.Attributes.SetAttribute("id", containerId);
            output.Attributes.SetAttribute("class", "natural-filter mb-3");

            var sb = new StringBuilder();

            // Chips
            if (items.Length > 0)
            {
                sb.AppendLine("<div class=\"nf-fields mb-2 d-flex flex-wrap gap-2\">");
                foreach (var field in items)
                {
                    sb.AppendLine($"<button type=\"button\" class=\"btn btn-outline-secondary btn-sm nf-field-btn\" data-field=\"{E(field)}\">{E(field)}</button>");
                }
                sb.AppendLine("</div>");
            }

            sb.AppendLine("<div class=\"d-flex gap-2\">");
            sb.AppendLine($"<input id=\"{E(inputId)}\" name=\"{E(Name)}\" value=\"{E(Value)}\" placeholder=\"{E(Placeholder)}\" class=\"form-control\" />");

            sb.AppendLine("<div class=\"d-flex gap-2\">");

            // Botão Filtrar
            sb.AppendLine("<button type=\"submit\" class=\"btn btn-primary\">Filtrar</button>");

            // Botão Limpar só se houver valor
            if (!string.IsNullOrWhiteSpace(Value))
            {
                sb.AppendLine($"<button id=\"{E(clearBtnId)}\" type=\"button\" class=\"btn btn-outline-secondary\">Limpar</button>");
            }

            sb.AppendLine("</div>"); // fecha wrapper dos botões
            sb.AppendLine("</div>"); // fecha d-flex principal

            sb.AppendLine($"<small class=\"text-muted\">Ex.: <code>{Example}</code></small>");

            // JS (delegação)
            var js = $@"
<script>
(function() {{
    var container = document.getElementById('{containerId}');
    if (!container) return;

    // chips
    container.addEventListener('click', function (ev) {{
        var target = ev.target;
        if (!target) return;

        if (target.classList && target.classList.contains('nf-field-btn')) {{
            var field = target.getAttribute('data-field') || '';
            var input = container.querySelector('input[name=""{E(Name)}""]');
            if (!input) return;

            var token = field + ':';
            if (input.value.indexOf(token) === -1) {{
                if (input.value && !input.value.endsWith(' ')) {{
                    input.value += ' ';
                }}
                input.value += token;
            }}
            input.focus();
        }}
    }});

    // limpar
    var clearBtn = document.getElementById('{clearBtnId}');
    if (clearBtn) {{
        clearBtn.addEventListener('click', function () {{
            var input = container.querySelector('input[name=""{E(Name)}""]');
            if (input) input.value = '';

            // remover inputs hidden com mesmo nome
            var hiddenInputs = container.querySelectorAll('input[type=""hidden""][name=""{E(Name)}""]');
            hiddenInputs.forEach(function(h) {{ h.parentNode && h.parentNode.removeChild(h); }});

            container.submit();
        }});
    }}
}})();
</script>";

            sb.AppendLine(js);

            output.Content.SetHtmlContent(sb.ToString());
        }
    }
}
