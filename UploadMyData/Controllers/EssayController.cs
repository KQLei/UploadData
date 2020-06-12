using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EF.Core.Data;
using EF.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UploadMyData.Models;

namespace UploadMyData.Controllers
{
    public class EssayController : Controller
    {
        private readonly ILogger<EssayController> _logger;

        private readonly IUnitOfWork _unitOfWork;

        private readonly IConfiguration _configuration;

        public EssayController(ILogger<EssayController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult EssayList()
        {
            var essayList = _unitOfWork.Repository<Essay>().Table.Select(p => new EssayDTO
            {
                Id = p.ID,
                Title = p.Title,
                Content = p.Content,
                AddDateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")
            }); ;

            return Json(essayList);
        }

        [HttpPost]
        public IActionResult AddEssay(EssayDTO essayDTO)
        {
            ResultModel resultModel = new ResultModel();
            try
            {
                _unitOfWork.Repository<Essay>().Insert(new Essay
                {
                    Content = essayDTO.Content,
                    CreateTime = DateTime.Now,
                    Title = essayDTO.Title
                });

                _unitOfWork.Commit();

                resultModel.IsSuccess = true;
                resultModel.Message = "添加成功";
            }
            catch (Exception ex)
            {
                resultModel.IsSuccess = false;
                resultModel.Message = $"添加失败，原因为：{ex.Message}";
            }
            return Json(resultModel);
        }

        [HttpPost]
        public IActionResult Delete(long id, string dCode)
        {
            ResultModel result = new ResultModel();
            try
            {
                if (string.IsNullOrWhiteSpace(dCode) || dCode != _configuration.GetValue<string>("DeleteCode"))
                {
                    throw new Exception("删除码错误");
                }
                var urlRep = _unitOfWork.Repository<Essay>();
                urlRep.Delete(urlRep.GetById(id));
                _unitOfWork.Commit();

                result.IsSuccess = true;
                result.Message = "删除成功";
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"删除失败，原因为：{ex.Message}";
            }
            return Json(result);
        }

    }
}
