#pragma checksum "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\_GridActions.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f363bba8288cee8c87c1a152b9f955c7d037b868"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Common__GridActions), @"mvc.1.0.view", @"/Views/Shared/Common/_GridActions.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\_ViewImports.cshtml"
using MAA.ActionTracking.Web;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\_ViewImports.cshtml"
using MAA.ActionTracking.Web.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f363bba8288cee8c87c1a152b9f955c7d037b868", @"/Views/Shared/Common/_GridActions.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0a29bcade02dfecb8976a2fde2df787e8ef7aa77", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Common__GridActions : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<GridActionModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n    <div class=\"table-filters-bar action-header\"");
            BeginWriteAttribute("id", " id=\"", 74, "\"", 103, 2);
            WriteAttributeValue("", 79, "grid-actions-", 79, 13, true);
#nullable restore
#line 3 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\_GridActions.cshtml"
WriteAttributeValue("", 92, Model.Name, 92, 11, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">\r\n        <h2 class=\"mr-3 color-iron\">");
#nullable restore
#line 4 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\_GridActions.cshtml"
                               Write(Model.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral(@"</h2>
        <div class=""actions"">
            <a href=""javascript:(0)"" class=""btn btn-light btn-sm col mr-1 column-options"" title=""Configure column order, sort and visibility"" data-toggle=""tooltip""><i class=""mr-1 fas fa-wrench color-primary""></i>Columns""]</a>
            <a href=""javascript:(0)"" class=""btn btn-light btn-sm col mr-1 current-hidden-columns"" data-title=""Hidden Columns"" data-toggle=""popover""><i class=""mr-1 mdi mdi-eye-off color-primary""></i></a>
            <a href=""javascript:(0)"" class=""btn btn-light btn-sm col mr-1 current-sort-columns"" data-title=""Current Sort Columns"" data-toggle=""popover""><i class=""mr-1 mdi mdi-sort color-primary""></i></a>
            <a href=""javascript:(0)"" class=""btn btn-light btn-sm col mr-1 current-filter-columns"" data-title=""Applied Filters"" data-toggle=""popover""><i class=""mr-1 mdi mdi-filter color-primary""></i></a>
            <div class=""custom-control custom-switch toggle-personalization ml-3 pt-2"">
                <input type=""checkbox"" class=""custom-cont");
            WriteLiteral(@"rol-input"" checked=""checked"" id=""toggle-personalization"" data-text-standard=""Show Standard Settings"" data-text-personalized=""Show Personalized Settings"">
                <label class=""custom-control-label"" for=""toggle-personalization"">Show Standard Settings""]</label>
            </div>
        </div>
        <div class=""actions search-wrapper"">
            <i class=""mr-1 fas fa-filter""></i> <input type=""text"" autocomplete=""off"" role=""search"" placeholder=""Dynamic placeholder here"" />
            <i class=""mr-1 mdi mdi-close fa-2x""></i>
        </div>
    </div>
");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<GridActionModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
