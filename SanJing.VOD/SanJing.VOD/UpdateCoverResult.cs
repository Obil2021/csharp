using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SanJing.VOD.TencentResult
{
    public class UpdateCoverResultResponse
    {
        public string CoverUrl { get; set; }
        public string RequestId { get; set; }
    }

    public class UpdateCoverResult
    {
        public UpdateCoverResultResponse Response { get; set; }
    }
}
