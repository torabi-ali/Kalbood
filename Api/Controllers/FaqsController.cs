using App.Services.Faqs;
using App.ViewModels.Faqs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class FaqsController : BaseApiController
{
    private readonly IFaqService _faqService;

    public FaqsController(IFaqService faqService)
    {
        _faqService = faqService;
    }

    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn300s")]
    public Task<List<FaqDetailDto>> Get(string url)
    {
        return _faqService.GetByUrlAsync(url);
    }
}
