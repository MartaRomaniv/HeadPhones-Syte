#pragma checksum "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "c57fdfa5eed6509ba8aa8418efb1f6c66dbc1ccc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Roles_Edit), @"mvc.1.0.view", @"/Views/Roles/Edit.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Roles/Edit.cshtml", typeof(AspNetCore.Views_Roles_Edit))]
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
#line 1 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\_ViewImports.cshtml"
using WebShop;

#line default
#line hidden
#line 2 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\_ViewImports.cshtml"
using WebShop.Models;

#line default
#line hidden
#line 1 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
using Microsoft.AspNetCore.Identity;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"c57fdfa5eed6509ba8aa8418efb1f6c66dbc1ccc", @"/Views/Roles/Edit.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"bf6e511299f0cda60cf4de484793b6cb35bd75b1", @"/Views/_ViewImports.cshtml")]
    public class Views_Roles_Edit : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<WebShop.ViewModels.ChangeRoleViewModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Edit", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(85, 38, true);
            WriteLiteral("<h2> Change role for the User <strong>");
            EndContext();
            BeginContext(124, 14, false);
#line 3 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
                                 Write(Model.UserName);

#line default
#line hidden
            EndContext();
            BeginContext(138, 18, true);
            WriteLiteral("</strong></h2>\r\n\r\n");
            EndContext();
            BeginContext(156, 446, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "c57fdfa5eed6509ba8aa8418efb1f6c66dbc1ccc4509", async() => {
                BeginContext(194, 38, true);
                WriteLiteral("\r\n  <input type=\"hidden\" name=\"userId\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 232, "\"", 253, 1);
#line 6 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
WriteAttributeValue("", 240, Model.UserId, 240, 13, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(254, 33, true);
                WriteLiteral(" />\r\n  <div class=\"form-group\">\r\n");
                EndContext();
#line 8 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
     foreach (IdentityRole role in Model.AllRoles)
    {

#line default
#line hidden
                BeginContext(346, 41, true);
                WriteLiteral("      <input type=\"checkbox\" name=\"roles\"");
                EndContext();
                BeginWriteAttribute("value", " value=\"", 387, "\"", 405, 1);
#line 10 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
WriteAttributeValue("", 395, role.Name, 395, 10, false);

#line default
#line hidden
                EndWriteAttribute();
                BeginContext(406, 15, true);
                WriteLiteral("\r\n             ");
                EndContext();
                BeginContext(423, 64, false);
#line 11 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
         Write(Model.UserRoles.Contains(role.Name) ? "checked=\"checked\"" : "");

#line default
#line hidden
                EndContext();
                BeginContext(488, 3, true);
                WriteLiteral(" />");
                EndContext();
                BeginContext(492, 9, false);
#line 11 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
                                                                              Write(role.Name);

#line default
#line hidden
                EndContext();
                BeginContext(501, 8, true);
                WriteLiteral("<br />\r\n");
                EndContext();
#line 12 "C:\Users\HP\Desktop\MyWebProject2\MyWebProject2\WebShop\Views\Roles\Edit.cshtml"
          }

#line default
#line hidden
                BeginContext(522, 73, true);
                WriteLiteral("  </div>\r\n  <button type=\"submit\" class=\"btn btn-primary\">Save</button>\r\n");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(602, 2, true);
            WriteLiteral("\r\n");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<WebShop.ViewModels.ChangeRoleViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591