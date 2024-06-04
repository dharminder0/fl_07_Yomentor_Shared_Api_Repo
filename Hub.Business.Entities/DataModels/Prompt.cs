using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business.Entities.DataModels {
    public class Prompt {
        public int Prompt_Id { get; set; }
        public string Prompt_Text { get; set; }
        public string Prompt_Type { get; set; }
        public float Temperature { get; set; }
        public int Max_tokens { get; set; }
        public float Top_p { get; set; }
        public string Sop_Sequence { get; set; }
        public string Model { get; set; }
        public string System_Role { get; set; }
    }
}
