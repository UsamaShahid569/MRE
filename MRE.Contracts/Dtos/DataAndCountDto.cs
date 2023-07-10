using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Dtos
{
    public class DataAndCountDto<T> where T : class
    {
        public List<T> Data { get; set; }

        public int Count { get; set; }
    }
}
