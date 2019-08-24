using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperDemo.Tag_Helper
{
    [HtmlTargetElement("email", TagStructure = TagStructure.WithoutEndTag)]
    public class EmailTagHelper:TagHelper
    {
        public string Address { get; set; }

       
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("href", $"mailto:{Address}");
            output.Content.SetContent($"Send Mail to {Address}");


            //<image url="image.jpg" fallback-url="noimage.jpg"
        }
    }


    public class ImageTagHelper : TagHelper
    {
        public string Url { get; set; }
        public string FallBackUrl { get; set; }
        public string ImgName { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("src", $"{Url}");
            output.Attributes.SetAttribute("fallback-url", $"{FallBackUrl}");
            output.Content.SetContent($"{ImgName}");

            //<image url="image.jpg" fallback-url="noimage.jpg"
        }
    }
}
