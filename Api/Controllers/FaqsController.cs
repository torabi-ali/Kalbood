using App.Services.Faqs;
using App.ViewModels.Faqs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace Api.Controllers;

public class FaqsController(IFaqService faqService) : BaseApiController
{
    [HttpGet]
    [OutputCache(PolicyName = "ExpireIn300s")]
    public Task<List<FaqDetailDto>> Get(string url)
    {
        return faqService.GetByUrlAsync(url);
    }
}
