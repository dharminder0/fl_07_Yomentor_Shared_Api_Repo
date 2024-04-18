﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class SkillTestResponse {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int GradeId { get; set; }
        public int SubjectId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string GradeName { get; set; }
        public string SubjectName { get; set; }
        public bool IsDeleted { get; set; }
        public int AverageMarks { get; set; }
        public int AteemptCount { get; set; }
        public List<AteemptHistory> AteemptHistory { get; set; }  
    }
    public class AteemptHistory {
        public int Score { get; set; }
        public DateTime AttemptDate { get; set; }

    }
}
