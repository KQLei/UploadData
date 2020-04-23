using System;
using System.Collections.Generic;
using System.Text;

namespace EF.Core.Data
{
    /// <summary>
    /// 网址收藏
    /// </summary>
    public class URLCollection : BaseEntity
    {
        /// <summary>
        /// 网址
        /// </summary>
        public string URL { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 打开次数
        /// </summary>
        public int OpenCount { get; set; }

    }
}
