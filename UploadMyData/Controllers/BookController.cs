using EF.Core.Data;
using EF.Core.Helper;
using EF.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public ActionResult Index(BookType? bType = null)
        {
            ViewBag.BookType = EnumHelper.GetEnumOptions<BookType>();
            if (bType.HasValue)
            {
                ViewBag.BookTypeValue = (int)bType;
            }
            else
            {
                ViewBag.BookTypeValue = null;
            }
            return View();
        }

        public ActionResult BookLists(BookType? bType = null)
        {
            var bookRep = _unitOfWork.Repository<Book>();
            var bookList = bookRep.Table.Where(p => bType.HasValue ? p.BookType == bType : true).Select(p => new BookDTO()
            {
                ID = p.ID,
                Auther = p.Auther ?? "-",
                CreateTime = p.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                ModifiedTime = p.ModifiedTime,
                Title = p.Title ?? "-",
                URL = p.URL,
                DownloadNum = p.DownloadNum,
                FileSize = p.FileSize.ToString()
            });
            return Json(bookList);
        }

        [HttpPost]
        public ActionResult Create([FromForm] IFormCollection formData)
        {
            try
            {
                if (formData.Files.Count <= 0)
                {
                    return Json(new ResultModel
                    {
                        IsSuccess = false,
                        Message = "请选择要上传的书籍"
                    });
                }

                var bookRep = _unitOfWork.Repository<Book>();

                var newBook = new Book
                {
                    Auther = formData["Auther"].ToString(),
                    Title = formData["Title"].ToString(),
                    BookType = (BookType)int.Parse(formData["BookType"].ToString()),
                };

                HandleUploadFiles(formData.Files, newBook);

                bookRep.Insert(newBook);

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
        public ActionResult Delete(long bookId, string dCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dCode) || dCode != _configuration.GetValue<string>("DeleteCode"))
                {
                    throw new Exception("删除码错误");
                }
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
            var bookRep = _unitOfWork.Repository<Book>();


            try
            {
                HandleUploadFiles(Request.Form.Files, bookRep.GetById(bookId));
            }
            catch (Exception ex)
            {
                return Json(new ResultModel
                {
                    IsSuccess = false,
                    Message = $"上传失败，原因为：{ex.Message}"
                });
            }

            _unitOfWork.Commit();

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
            //文件路径
            string path = Path.Combine(_configuration.GetSection("BookUploadFile").Value, bookObj.URL);
            if (string.IsNullOrWhiteSpace(bookObj.URL) || !System.IO.File.Exists(path))
            {
                return RedirectToAction("Index", "Book");
            }
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[Path.GetExtension(bookObj.URL)];
            var result = File(new FileStream(path, FileMode.Open, FileAccess.Read), memi, bookObj.URL);
            //提交数据库
            bookObj.DownloadNum += 1;
            _unitOfWork.Commit();
            return result;
        }

        public ActionResult BookFilePath(long bookId)
        {
            var bookRep = _unitOfWork.Repository<Book>();
            var bookObj = bookRep.GetById(bookId);
            //文件路径
            string path = Path.Combine(_configuration.GetSection("BookUploadFile").Value, bookObj.URL);
            if (System.IO.File.Exists(path))
            {
                return Json(new ResultModel
                {
                    IsSuccess = true,
                    Message = Path.Combine("BookFile", bookObj.URL)
                });
            }
            return Json(new ResultModel
            {
                IsSuccess = false,
                Message = "当前书籍不存在"
            });
        }

        private void HandleUploadFiles(IFormFileCollection files, Book bookObj)
        {
            //文件夹地址
            string filePath = _configuration.GetSection("BookUploadFile").Value;
            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //文件名
                    string fileName = Path.GetFileName(file.FileName);

                    //文件扩展名
                    string fileExtension = Path.GetExtension(file.FileName);

                    var fileExtensionList = _configuration.GetSection("FileType").Get<List<string>>();

                    if (!fileExtensionList.Contains(fileExtension))
                    {
                        throw new Exception("当前上传的文件类型不满足要求");
                    }


                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    //文件路径
                    string path = Path.Combine(filePath, fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }                 

                    bookObj.URL = fileName;
                    bookObj.FileSize = file.Length;
                }
            }
        }
    }
}