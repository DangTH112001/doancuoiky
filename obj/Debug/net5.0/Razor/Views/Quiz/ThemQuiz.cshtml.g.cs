#pragma checksum "C:\Users\DangT\Desktop\Code\doancuoiky\Views\Quiz\ThemQuiz.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "30f683c58cb2ad0372d7751136bf17736721806c"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Quiz_ThemQuiz), @"mvc.1.0.view", @"/Views/Quiz/ThemQuiz.cshtml")]
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
#line 1 "C:\Users\DangT\Desktop\Code\doancuoiky\Views\_ViewImports.cshtml"
using doancuoiky;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\DangT\Desktop\Code\doancuoiky\Views\_ViewImports.cshtml"
using doancuoiky.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"30f683c58cb2ad0372d7751136bf17736721806c", @"/Views/Quiz/ThemQuiz.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"291ef34b3b4f60fae8059f844ec38b81e1ea67ac", @"/Views/_ViewImports.cshtml")]
    public class Views_Quiz_ThemQuiz : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<");
#nullable restore
#line 1 "C:\Users\DangT\Desktop\Code\doancuoiky\Views\Quiz\ThemQuiz.cshtml"
   
    Layout = "~/Views/Shared/_LayoutHome.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<script>\r\n    alert(\"Lỗi không thêm được\");\r\n    window.location.href = \'");
#nullable restore
#line 7 "C:\Users\DangT\Desktop\Code\doancuoiky\Views\Quiz\ThemQuiz.cshtml"
                       Write(Url.Action("Index", "Question"));

#line default
#line hidden
#nullable disable
            WriteLiteral("\';\r\n</script>");
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