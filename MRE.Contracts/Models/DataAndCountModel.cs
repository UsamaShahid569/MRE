using System;
using System.Collections.Generic;
using System.Text;

namespace MRE.Contracts.Models
{
    public class DataAndCountModel<T> where T : class
    {
        public List<T> Data { get; set; } = new List<T>();
        public int Count { get; set; }
    }
}
