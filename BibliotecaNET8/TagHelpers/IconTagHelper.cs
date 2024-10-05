using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BibliotecaNET8.TagHelpers
{
    [HtmlTargetElement("icon")]
    public class IconTagHelper : TagHelper
    {
        // Define el atributo "href" para pasar el icono específico
        public string Href { get; set; }

        // Define el atributo "class" para las clases adicionales que quieras aplicar
        public string Class { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Cambia la etiqueta de <icon> a <svg>
            output.TagName = "svg";
            output.Attributes.SetAttribute("class", $"{Class}");

            // Genera el contenido del SVG usando la etiqueta <use>
            var useTag = $"<use href=\"../../icons/sprite.svg#{Href}\"></use>";

            output.Content.SetHtmlContent(useTag);
        }
    }
}
