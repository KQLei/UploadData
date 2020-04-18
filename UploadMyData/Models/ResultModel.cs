using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UploadMyData.Models
{
    /// <summary>
    /// 请求响应视图模型
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 结果 true-正确 false-错误
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string Message { get; set; }
    }
}
