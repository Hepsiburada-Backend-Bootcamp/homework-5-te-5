using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Parameters
{
    public class ProductParameters : QuerystringParameters
    {
        public int MinPrice { get; set; } = 0;
        public int? MaxPrice { get; set; }

        public string Brand { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }

        public string SearchByBrand { get; set; }
        public string SearchByCategory { get; set; }
        public string SearchByName { get; set; }
    }
}
