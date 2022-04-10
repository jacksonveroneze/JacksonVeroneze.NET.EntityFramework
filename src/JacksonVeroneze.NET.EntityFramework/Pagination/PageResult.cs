using System.Collections.Generic;

namespace JacksonVeroneze.NET.EntityFramework.Pagination
{
    public class PageResult<T>
    {
        public ICollection<T> Data { get; set; }

        public int? TotalElements { get; set; }
        
        public int? TotalPages { get; set; }

        public int? CurrentPage { get; set; }
        
        public int? PageSize { get; set; }
    }
}
