using App.Services.Faqs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Components;

public class FaqViewComponent(IFaqService faqService) : ViewComponent
{
    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await faqService.GetByUrlAsync(Request.Path);
        return View(model);
    }
}
