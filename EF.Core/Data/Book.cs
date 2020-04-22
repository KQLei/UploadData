
using System;

namespace EF.Core.Data
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }

        public string Auther { get; set; }

        public string ISBN { get; set; }

        public DateTime Published { get; set; }

        public string IP { get; set; }

        public string URL { get; set; }

        public int DownloadNum { get; set; }

        public BookType BookType { get; set; }

    }

    public enum BookType
    {
        计算机技术,
        文学,
        其他
    }
}
