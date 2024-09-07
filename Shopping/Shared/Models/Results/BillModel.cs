using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared.Models.Results
{
    public sealed class BillModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}