using App.Services.Faqs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Web.Components;

public class FaqViewComponent : ViewComponent
{
    private readonly IFaqService _faqService;

    public FaqViewComponent(IFaqService faqService)
    {
        _faqService = faqService;
    }

    [OutputCache(PolicyName = "ExpireIn3000s")]
    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = await _faqService.GetByUrlAsync(Request.Path);
        return View(model);
    }
}
