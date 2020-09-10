using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace deadlock.ModelDtos
{
    public class PositionDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
