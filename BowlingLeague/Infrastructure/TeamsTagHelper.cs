using BowlingLeague.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "team-blah")]
    public class TeamsTagHelper : TagHelper
    {

        private IUrlHelperFactory uhf;

        public TeamsTagHelper(IUrlHelperFactory temp)
        {
            uhf = temp;
        }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext vc { get; set; }

        public string PageAction { get; set; }
        public List<Team> Teams { get; set; }
        public int CurrentID { get; set; }
        public string PageClass { get; set; }
        public bool PageClassesEnabled { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }



        public override void Process(TagHelperContext thc, TagHelperOutput tho)
        {
            IUrlHelper uh = uhf.GetUrlHelper(vc);

            TagBuilder final = new TagBuilder("div");

            TagBuilder tb1 = new TagBuilder("a");
            tb1.Attributes["href"] = uh.Action(PageAction, new { id = 0 });

            if (PageClassesEnabled)
            {
                tb1.AddCssClass(PageClass);
                tb1.AddCssClass(0 == CurrentID ? PageClassSelected : PageClassNormal);
            }

            tb1.InnerHtml.Append("All Teams");

            final.InnerHtml.AppendHtml(tb1);

            for (int i = 0; i < Teams.Count(); i++)
            {
                TagBuilder tb = new TagBuilder("a");
                tb.Attributes["href"] = uh.Action(PageAction, new { id = Teams[i].TeamID });

                if (PageClassesEnabled)
                {
                    tb.AddCssClass(PageClass);
                    tb.AddCssClass(Teams[i].TeamID == CurrentID ? PageClassSelected : PageClassNormal);
                }

                tb.InnerHtml.Append(Teams[i].TeamName);

                final.InnerHtml.AppendHtml(tb);
            }
            tho.Content.AppendHtml(final.InnerHtml);
        }
    }
}