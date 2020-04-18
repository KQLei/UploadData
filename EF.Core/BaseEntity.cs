using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Core
{
    public class BaseEntity
    {
        public long ID { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ModifiedTime { get; set; }

    }
}
