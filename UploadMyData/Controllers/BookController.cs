using EF.Core.Data;
using EF.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using UploadMyData.Models;

namespace UploadMyData.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public BookController(ILogger<BookController> logger, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BookLists()
        {
            var bookRep = _unitOfWork.Repository<Book>();
            var bookList = bookRep.Table.Select(p => new BookDTO()
            {
                ID = p.ID,
                Auther = p.Auther,
                CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ModifiedTime = p.ModifiedTime,
                Title = p.Title,
                DownloadNum = p.DownloadNum
            });
            return Json(bookList);
        }

        [HttpPost]
        public ActionResult Create(BookDTO bookDTO)
        {
            try
            {
                var bookRep = _unitOfWork.Repository<Book>();

                bookRep.Insert(new Book
                {
                    Auther = bookDTO.Auther,
                    Title = bookDTO.Title,
                });
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
        public ActionResult Delete(long bookId)
        {
            try
            {
                var bookRep = _unitOfWork.Repository<Book>();
                bookRep.Delete(bookRep.GetById(bookId));
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

        [HttpPost]
        public ActionResult Upload(long bookId)
        {

            if (Request.Form.Files.Count <= 0)
            {
                return Json(new ResultModel
                {
                    IsSuccess = false,
                    Message = "请选择要上传的书籍"
                });
            }

            HandleUploadFiles(Request.Form.Files, bookId);

            return Json(new ResultModel
            {
                IsSuccess = true,
                Message = "上传成功"
            });
        }

        public ActionResult Download(long bookId)
        {
            var bookRep = _unitOfWork.Repository<Book>();
            var bookObj = bookRep.GetById(bookId);
            bookObj.DownloadNum += 1;
            if (string.IsNullOrWhiteSpace(bookObj.URL))
            {
                return RedirectToAction("Index");
            }

            var fileName = Path.GetFileName(bookObj.URL);
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[Path.GetExtension(bookObj.URL)];
            _unitOfWork.Commit();
            return File(new FileStream(bookObj.URL, FileMode.Open, FileAccess.Read), memi, fileName);
        }

        private void HandleUploadFiles(IFormFileCollection files, long bookId)
        {
            string webRootPath = _configuration.GetSection("BookUploadFile").Value;
            //数据总长度
            long size = files.Sum(f => f.Length);
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //大小 字节
                    long fileSize = file.Length;

                    //文件地址
                    string filePath = Path.Combine(webRootPath, "upload");

                    //文件名
                    string fileName = Path.GetFileName(file.FileName);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    string path = Path.Combine(filePath, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        var bookRep = _unitOfWork.Repository<Book>();
                        var bookObj = bookRep.GetById(bookId);
                        bookObj.URL = path;
                        _unitOfWork.Commit();
                    }
                }
            }
        }
    }
}