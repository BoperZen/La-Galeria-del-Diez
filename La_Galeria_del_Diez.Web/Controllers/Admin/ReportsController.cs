using La_Galeria_del_Diez.Application.Interfaces;
using La_Galeria_del_Diez.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Diagnostics;

namespace La_Galeria_del_Diez.Web.Controllers.Admin
{
    [Route("Admin/Reports")]
    [Authorize]
    public class ReportsController : Controller
    {
        private const int AdminUserId = 1;

        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            if (!IsAdminUser())
            {
                return Forbid();
            }

            return View("~/Views/Admin/Reports/Index.cshtml");
        }

        [HttpGet("AuctionSummary")]
        public async Task<IActionResult> AuctionSummary()
        {
            if (!IsAdminUser())
            {
                return Forbid();
            }

            try
            {
                var result = await _reportService.GetAuctionSummaryAsync();
                return View("~/Views/Admin/Reports/AuctionSummary.cshtml", result);
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        [HttpGet("MostActiveAuctions")]
        public async Task<IActionResult> MostActiveAuctions()
        {
            if (!IsAdminUser())
            {
                return Forbid();
            }

            try
            {
                var result = await _reportService.GetMostActiveAuctionsAsync();
                return View("~/Views/Admin/Reports/MostActiveAuctions.cshtml", result);
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        [HttpGet("BidHistory")]
        public async Task<IActionResult> BidHistory(DateTime? startDate, DateTime? endDate)
        {
            if (!IsAdminUser())
            {
                return Forbid();
            }

            try
            {
                var endDateExclusive = endDate?.Date.AddDays(1);
                var result = await _reportService.GetBidHistoryAsync(startDate?.Date, endDateExclusive);
                return View("~/Views/Admin/Reports/BidHistory.cshtml", result);
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        [HttpGet("TopValuedItems")]
        public async Task<IActionResult> TopValuedItems()
        {
            if (!IsAdminUser())
            {
                return Forbid();
            }

            try
            {
                var result = await _reportService.GetTopValuedItemsAsync();
                return View("~/Views/Admin/Reports/TopValuedItems.cshtml", result);
            }
            catch
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
        }

        private bool IsAdminUser()
        {
            var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(idClaim, out var currentUserId) && currentUserId == AdminUserId;
        }
    }
}
