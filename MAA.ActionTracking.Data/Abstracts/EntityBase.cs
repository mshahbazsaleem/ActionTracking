using System;
using System.ComponentModel.DataAnnotations;

namespace MAA.ActionTracking.Data.Abstracts
{
    public abstract class EntityBase
    {
        [Key]
        public virtual int Id { get; set; }
      
        [DataType(DataType.DateTime)]
        public virtual DateTime CreatedDate { get; set; }

       
        [DataType(DataType.DateTime)]
        public virtual DateTime LastUpdated { get; set; }
    }
}
