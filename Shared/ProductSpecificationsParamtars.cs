using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecificationsParamtars
    {
        //int? brandId , int? typeId, string? sort, int pageIndex = 1, int pageSize = 5
        public int? brandId {  get; set; }
        public int? typeId { get; set; }
        public string? sort {  get; set; }
        public string? Search { get; set; }

        public int _pageIndex = 1;
        public int _pageSize = 5;
        
        public int pageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

    }
}
