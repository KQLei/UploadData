using EF.Core.Data;
using EF.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace UpdateBookDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EFDbContext>();
            optionsBuilder.UseSqlServer("Server=49.233.170.29;Database=BookDB;User ID=sa;Password=123qwe!@#");


            using (EFDbContext context = new EFDbContext(optionsBuilder.Options))
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                var bookRep = unitOfWork.Repository<Book>();

                foreach (var item in bookRep.Table)
                {
                    //查询文件：
                    string filePath = Path.Combine("C:\\BookUpload", item.URL);
                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine($"编号为{item.ID}的书籍路径为{filePath}，查询不到当前书籍");
                        continue;
                    }

                    //获取文件大小
                    if (File.Exists(filePath))
                    {
                        var file = new FileInfo(filePath);
                        item.FileSize = file.Length;
                        Console.WriteLine($"编号为{item.ID}的书籍路径为{filePath}，获取到的文件大小为{item.FileSize}");
                    }
                }
                unitOfWork.Commit();

            }

            Console.WriteLine("结束");
            Console.ReadKey();

        }
    }
}
