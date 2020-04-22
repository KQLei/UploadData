using EF.Core.Data;
using EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UploadMyData.Models;

namespace UploadMyData.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            ViewBag.BookCount = _unitOfWork.Repository<Book>().Table.Count();
            ViewBag.Soft = new BookTypeInfo { BType = BookType.计算机技术.ToString(), BCount = _unitOfWork.Repository<Book>().Table.Where(p => p.BookType == BookType.计算机技术).Count() };
            ViewBag.Literature = new BookTypeInfo { BType = BookType.文学.ToString(), BCount = _unitOfWork.Repository<Book>().Table.Where(p => p.BookType == BookType.文学).Count() };
            ViewBag.Other = new BookTypeInfo { BType = BookType.其他.ToString(), BCount = _unitOfWork.Repository<Book>().Table.Where(p => p.BookType == BookType.其他).Count() };
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
