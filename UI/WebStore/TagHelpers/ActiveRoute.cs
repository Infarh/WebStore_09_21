using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebStore.TagHelpers
{
    [HtmlTargetElement(Attributes = AttributeName)]
    public class ActiveRoute : TagHelper
    {
        private const string AttributeName = "ws-is-active-route";

        private const string IgnoreAction = "ws-ignore-action";

        private const string IgnoreController = "ws-ignore-controller";

        [HtmlAttributeName("ws-is-active-route-active")]
        public string ActiveCssClass { get; set; } = "active";

        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }

        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }

        //[HtmlAttributeName("href")]
        //public string Href { get; set; }

        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public Dictionary<string, string> RouteValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var is_ignore_action = output.Attributes.RemoveAll(IgnoreAction);
            var is_ignore_controller = output.Attributes.RemoveAll(IgnoreController);

            if (IsActive(is_ignore_action, is_ignore_controller))
                MakeActive(output);

            output.Attributes.RemoveAll(AttributeName);
        }

        private bool IsActive(bool IgnoreAction, bool IgnoreController)
        {
            var route_values = ViewContext.RouteData.Values;

            var route_controller = route_values["controller"]?.ToString();
            var route_action = route_values["action"]?.ToString();

            //if (!string.IsNullOrEmpty(Action) && !string.Equals(Action, route_action))
            //    return false;
            if (!IgnoreAction && Action is { Length: > 0 } action && !string.Equals(action, route_action))
                return false;

            if (!IgnoreController && Controller is { Length: > 0 } controller && !string.Equals(controller, route_controller))
                return false;

            foreach (var (key, value) in RouteValues)
                if (!route_values.ContainsKey(key) || route_values[key]?.ToString() != value)
                    return false;

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var class_attribute = output.Attributes.FirstOrDefault(attr => attr.Name == "class");

            if(class_attribute is null)
                output.Attributes.Add("class", ActiveCssClass);
            else
            {
                if(class_attribute.Value?.ToString()?.Contains(ActiveCssClass) ?? false)
                    return;

                output.Attributes.SetAttribute("class", $"{class_attribute.Value} {ActiveCssClass}");
            }
        }
    }
}
