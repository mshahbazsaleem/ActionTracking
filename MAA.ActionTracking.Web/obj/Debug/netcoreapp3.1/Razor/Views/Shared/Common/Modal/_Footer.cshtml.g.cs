#pragma checksum "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\Modal\_Footer.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d0d66c5b17bad291254bf8f67c66bb6b85a3caec"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared_Common_Modal__Footer), @"mvc.1.0.view", @"/Views/Shared/Common/Modal/_Footer.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d0d66c5b17bad291254bf8f67c66bb6b85a3caec", @"/Views/Shared/Common/Modal/_Footer.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0a29bcade02dfecb8976a2fde2df787e8ef7aa77", @"/Views/_ViewImports.cshtml")]
    public class Views_Shared_Common_Modal__Footer : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral(@"        </div><!--modal-body ends-->
<div class=""modal-footer custom-footer"">
    <div class=""pager""></div>
    <div class=""modal-footer-actions"">
        <button type=""button"" class=""btn btn-link btn-cancel btn-sm"" data-icon=""mdi mdi-close-box"" data-processing-text=""Cleaning up..."">
            Cancel
        </button>
        <button type=""button"" class=""btn btn-primary btn-save btn-sm"" data-icon=""mdi mdi-");
#nullable restore
#line 8 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\Modal\_Footer.cshtml"
                                                                                     Write(string.IsNullOrEmpty(Model.ActionButtonIcon) ? "content-save" : Model.ActionButtonIcon);

#line default
#line hidden
#nullable disable
            WriteLiteral("\" data-processing-text=\"");
#nullable restore
#line 8 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\Modal\_Footer.cshtml"
                                                                                                                                                                                                      Write(Model.ActionButtonProcessingText==null ?"Saving..." : Model.ActionButtonProcessingText);

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n            <i");
            BeginWriteAttribute("class", " class=\"", 638, "\"", 748, 4);
            WriteAttributeValue("", 646, "mdi", 646, 3, true);
            WriteAttributeValue(" ", 649, "mdi-", 650, 5, true);
#nullable restore
#line 9 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\Modal\_Footer.cshtml"
WriteAttributeValue("", 654, string.IsNullOrEmpty(Model.ActionButtonIcon) ? "content-save" : Model.ActionButtonIcon, 654, 89, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue(" ", 743, "mr-1", 744, 5, true);
            EndWriteAttribute();
            WriteLiteral("></i>");
#nullable restore
#line 9 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\Modal\_Footer.cshtml"
                                                                                                                              Write(Model.ActionButtonText == null ? "Save" : Model.ActionButtonText);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </button>\r\n");
#nullable restore
#line 11 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\Modal\_Footer.cshtml"
         if (Model?.ShowSaveAndNew ?? false)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <button type=\"button\" class=\"btn btn-success btn-save-new btn-sm\" data-icon=\"mdi mdi-library-plus\" data-processing-text=\"Saving...\">\r\n                <i class=\"mdi mdi-library-plus mr-1\"></i>Save & New\r\n            </button>\r\n");
#nullable restore
#line 16 "E:\Ali\DynamicForms\MAA.ActionTracking.Web\Views\Shared\Common\Modal\_Footer.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n</div>\r\n    </div><!--modal-cotnent ends-->\r\n    </div><!--modal-dialog ends-->\r\n</div><!--modal ends-->\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
