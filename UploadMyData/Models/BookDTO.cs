﻿using EF.Core.Data;
using Microsoft.AspNetCore.Http;
using System;

namespace UploadMyData.Models
{
    /// <summary>
    /// 书籍视图模型
    /// </summary>
    public class BookDTO
    {
        public long ID { get; set; }

        public string Title { get; set; }

        public string Auther { get; set; }

        public string URL { get; set; }

        public int DownloadNum { get; set; }

        public string CreateTime { get; set; }

        public BookType BookType { get; set; }

        public DateTime ModifiedTime { get; set; }

        public string FileSize { get; set; }

        public FormFile BookFile { get; set; }
    }
}
