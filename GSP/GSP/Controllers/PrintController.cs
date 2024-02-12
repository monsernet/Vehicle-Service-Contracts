using GSP.Repositories;
using GSP.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace GSP.Controllers
{
    public class PrintController : Controller
    {
        private readonly PdfService _pdfService;
        private readonly IViewRenderService _viewRenderService;

        public PrintController(
            PdfService pdfService,
            IViewRenderService viewRenderService )
        {
            _pdfService = pdfService;
            _viewRenderService = viewRenderService;
        }

        public async Task<IActionResult> Print()
        {
            // Render the view to HTML string
            string htmlContent = await _viewRenderService.RenderToStringAsync("ServiceContract/PrintContract", null);

            // Generate PDF
            byte[] pdfBytes = _pdfService.GeneratePdf(htmlContent);

            // Return PDF as a file
            return File(pdfBytes, "application/pdf", "output.pdf");
        }

        //    private string RenderViewToString(string viewName)
        //    {
        //        var viewResult = View(viewName);
        //        var viewContext = new ViewContext(
        //            ControllerContext,
        //            viewResult.View,
        //            ViewData,
        //            TempData,
        //            new StringWriter(),
        //            new HtmlHelperOptions()
        //        );

        //        var t = viewResult.View.RenderAsync(viewContext);
        //        t.Wait();

        //        return t.Result.ToString();
        //    }
    }
}
