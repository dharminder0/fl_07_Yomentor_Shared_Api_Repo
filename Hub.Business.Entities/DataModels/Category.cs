using Core.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    [Alias(Name = "Category")]
    public class Category {
        public Category() { }
        [Key(AutoNumber = true)]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Icon { get; set; }
        public int IsDeleted { get; set; }
    }
}
