using La_Galeria_del_Diez.Application.Interfaces;
using La_Galeria_del_Diez.Web.Models;
using La_Galeria_del_Diez.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace La_Galeria_del_Diez.Web.Controllers.Admin
{
    [Route("Admin/Reports")]
    public class ReportsController : Controller
    {
        private const int AdminUserId = 1;

        private readonly IReportService _reportService;
        private readonly ICurrentUserProvider _currentUserProvider;

        public ReportsController(IReportService reportService, ICurrentUserProvider currentUserProvider)
        {
            _reportService = reportService;
            _currentUserProvider = currentUserProvider;
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
        public async Task<IActionResult> BidHistory()
        {
            if (!IsAdminUser())
            {
                return Forbid();
            }

            try
            {
                var result = await _reportService.GetBidHistoryAsync();
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

        private bool IsAdminUser() => _currentUserProvider.CurrentUserId == AdminUserId;
    }
}
