using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Notifications;

namespace DTKH2024.SbinSolution.Web.Controllers;

public class BrowserCacheCleanerController : SbinSolutionControllerBase
{
    private readonly INotificationAppService _notificationAppService;

    public BrowserCacheCleanerController(INotificationAppService notificationAppService)
    {
        _notificationAppService = notificationAppService;
    }
    
    public async Task<IActionResult> Clear()
    {
        var result = await _notificationAppService.SetAllAvailableVersionNotificationAsRead();
        
        HttpContext.Response.Headers.Append("Clear-Site-Data", "\"cache\"");

        return Json(new {Result = result});
    }
}