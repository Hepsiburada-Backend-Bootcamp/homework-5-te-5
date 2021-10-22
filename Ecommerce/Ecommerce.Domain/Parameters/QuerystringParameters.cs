using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Parameters
{
    public abstract class QuerystringParameters
    {

        private int _maxPageSize = 25;
        private int _pageSize = 10;

        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return _pageSize; }

            set
            {
                _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
            }
        }

        //FIX: Offset swaggerda gözükmemeli. NSwag Paketi yüklendi.
        [OpenApiIgnore]
        public int? Offset 
        {
            get { return PageSize * (PageNumber - 1); }
        }

    }
}
