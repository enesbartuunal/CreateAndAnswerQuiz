using System;
using System.Collections.Generic;
using System.Text;

namespace CreateAndAnswerQuiz.Models.Models
{
    public class AnswerDto
    {
        public int QuestionId { get; set; }

        public string Name { get; set; }

        public bool IsTrue { get; set; } = false;
    }
}
