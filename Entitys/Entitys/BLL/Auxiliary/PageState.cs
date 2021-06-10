using System;
using System.Collections.Generic;
using System.Text;

namespace Entitys.BLL.Auxiliary
{
    public class PageState
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 40;
    }
}
