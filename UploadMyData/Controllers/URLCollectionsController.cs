using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EF.Core.Data;
using EF.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using UploadMyData.Models;

namespace UploadMyData.Controllers
{
    public class URLCollectionsController : Controller
    {
        private readonly ILogger<URLCollectionsController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public URLCollectionsController(ILogger<URLCollectionsController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }


        // GET: URLCollections
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult URLLists()
        {
            var urlList = _unitOfWork.Repository<URLCollection>().Table;
            return Json(urlList);
        }


        [HttpPost]
        public IActionResult Create(URLCollection uRLCollection)
        {
            try
            {
                var urlRep = _unitOfWork.Repository<URLCollection>();

                urlRep.Insert(uRLCollection);
                _unitOfWork.Commit();
                return Json(new ResultModel
                {
                    IsSuccess = true,
                    Message = "添加成功"
                });
            }
            catch (Exception ex)
            {
                return Json(new ResultModel
                {
                    IsSuccess = false,
                    Message = $"添加失败，原因为：{ex.Message}"
                });
            }
        }


        [HttpPost]
        public IActionResult Delete(long id, string dCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dCode) || dCode != _configuration.GetValue<string>("DeleteCode"))
                {
                    throw new Exception("删除码错误");
                }
                var urlRep = _unitOfWork.Repository<URLCollection>();
                urlRep.Delete(urlRep.GetById(id));
                _unitOfWork.Commit();
                return Json(new ResultModel
                {
                    IsSuccess = true,
                    Message = "删除成功"
                });

            }
            catch (Exception ex)
            {
                return Json(new ResultModel
                {
                    IsSuccess = false,
                    Message = $"删除失败，原因为：{ex.Message}"
                });
            }
        }

    }
}
