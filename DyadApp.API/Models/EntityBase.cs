using System;

namespace DyadApp.API.Models
{
    public class EntityBase
    {
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
    }
}