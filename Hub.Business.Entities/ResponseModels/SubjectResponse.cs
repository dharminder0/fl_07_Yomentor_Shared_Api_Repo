﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class SubjectResponse {
        public int Id { get; set; }
        public string  Name { get; set; }
        public int GradeId { get; set; }
        public bool IsDeleted { get; set; }
        public string Icon { get; set; }
    }
}
