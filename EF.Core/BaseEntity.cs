using System;

namespace EF.Core
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
        }

        public long ID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifiedTime { get; set; }

    }
}
