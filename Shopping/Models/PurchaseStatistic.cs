using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Models
{
    public sealed class PurchaseStatistic
    {
        public Dictionary<string, decimal> Statistics { get; set; } = new Dictionary<string,decimal>();
    }
}
