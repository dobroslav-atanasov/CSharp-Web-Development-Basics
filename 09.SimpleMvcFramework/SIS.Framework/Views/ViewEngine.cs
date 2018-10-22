namespace SIS.Framework.Views
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class ViewEngine
    {
        private const string ViewPathPrefix = @"../../../";
        private const string DisplayTemplateSuffix = "DisplayTemplate";
        private const string LayoutViewName = "_Layout";
        private const string ErrorViewFile = "_Error";
        private const string ViewExtension = "html";
        private const string ModelCollectionViewParameterPattern = @"@Model\.Collection\.(\w+)\((.+)\)";

        private string ViewsFolderPath => $@"{ViewPathPrefix}\{MvcContext.Get.ViewsFolder}\";
        private string ViewsSharedFolderPath => $@"{this.ViewsFolderPath}Shared\";
        private string ViewsDisplayTemplateFolderPath => $@"{this.ViewsSharedFolderPath}\DisplayTemplates\";

        private string FormatLayoutViewPath() => $@"{this.ViewsSharedFolderPath}{LayoutViewName}.{ViewExtension}";
        private string FormatErrorViewPath() => $@"{this.ViewsSharedFolderPath}{ErrorViewFile}.{ViewExtension}";
        private string FormatViewPath(string controllerName, string actionName) => $@"{this.ViewsFolderPath}\{controllerName}\{actionName}.{ViewExtension}";
        private string FormatDisplayTemplatePath(string objectName) => $@"{this.ViewsDisplayTemplateFolderPath}{objectName}{DisplayTemplateSuffix}.{ViewExtension}";

        private string ReadLayoutHtml(string layoutViewPath)
        {
            if (!File.Exists(layoutViewPath))
            {
                throw new FileNotFoundException("Layout View does not exist!");
            }

            return File.ReadAllText(layoutViewPath);
        }

        private string ReadErrorHtml(string errorViewPath)
        {
            if (!File.Exists(errorViewPath))
            {
                throw new FileNotFoundException("Error View does not exist!");
            }

            return File.ReadAllText(errorViewPath);
        }

        private string ReadViewHtml(string viewPath)
        {
            if (!File.Exists(viewPath))
            {
                throw new FileNotFoundException($"View does not exist at {viewPath}!");
            }

            return File.ReadAllText(viewPath);
        }

        private string RenderObject(object viewObject, string displayTemplate)
        {
            foreach (var property in viewObject.GetType().GetProperties())
            {
                displayTemplate = this.RenderViewData(displayTemplate, property.GetValue(viewObject), property.Name);
            }

            return displayTemplate;
        }

        private string RenderViewData(string template, object viewObject, string viewObjectName = null)
        {
            if (viewObject != null 
                && viewObject.GetType() != typeof(string) 
                && viewObject is IEnumerable enumerable 
                && Regex.IsMatch(template, ModelCollectionViewParameterPattern))
            {
                var matchCollection = Regex.Matches(template, ModelCollectionViewParameterPattern)
                    .First(mc => mc.Groups[1].Value == viewObjectName);

                var fullMatch = matchCollection.Groups[0].Value;
                var itemPattern = matchCollection.Groups[2].Value;
                var result = string.Empty;

                foreach (var subObject in enumerable)
                {
                    result += itemPattern.Replace("@Item", this.RenderViewData(template, subObject));
                }

                return template.Replace(fullMatch, result);
            }

            if (viewObject != null
                && !viewObject.GetType().IsPrimitive
                && viewObject.GetType() != typeof(string))
            {
                if (File.Exists(this.FormatDisplayTemplatePath(viewObject.GetType().Name)))
                {
                    var renderedObject = this.RenderObject(viewObject, File.ReadAllText(this.FormatDisplayTemplatePath(viewObject.GetType().Name)));

                    return viewObjectName != null ? template.Replace($"@Model.{viewObjectName}", renderedObject) : renderedObject;
                }
            }

            return viewObjectName != null
                ? template.Replace($"@Model.{viewObject}", viewObject?.ToString())
                : viewObject?.ToString();
        }

        public string GetErrorContent()
        {
            return this.ReadLayoutHtml(this.FormatLayoutViewPath())
                .Replace("@RenderBody()", this.ReadErrorHtml(this.FormatErrorViewPath()));
        }

        public string GetViewContent(string controllerName, string actionName)
        {
            return this.ReadLayoutHtml(this.FormatLayoutViewPath()).Replace("@RenderBody()",
                this.ReadViewHtml(this.FormatViewPath(controllerName, actionName)));
        }

        public string RenderHtml(string fullHtmlContent, IDictionary<string, object> viewData)
        {
            var renderHtml = fullHtmlContent;

            if (viewData.Count > 0)
            {
                foreach (var parameter in viewData)
                {
                    renderHtml = this.RenderViewData(renderHtml, parameter.Value, parameter.Key);
                }
            }

            if (viewData.ContainsKey("Error"))
            {
                renderHtml = renderHtml.Replace("@Error", viewData["Error"].ToString());
            }

            return renderHtml;
        }
    }
}