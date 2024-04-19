using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.ResponseModels {
    public class AttemptSummaryResponse {
        public int AttemptId { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalCorrectAnswers { get; set; }
        public double PercentageCorrect { get; set; }
    }
}
