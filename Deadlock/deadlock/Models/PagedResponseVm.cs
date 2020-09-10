using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deadlock.Models
{
    public class PagedResponseVm<T> where T : class
    {
        public HashSet<T> Data { get; set; }
        public int Count { get; set; }
        public int Total { get; set; }
    }
}
