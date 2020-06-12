using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EF.Core.Data
{
    /// <summary>
    /// 随笔
    /// </summary>
    public class Essay : BaseEntity
    {
        [StringLength(50)]
        public string Title { get; set; }

        public string Content { get; set; }

    }
}
