using System;
using System.Collections.Generic;
using System.Text;

namespace CreateAndAnswerQuiz.Models.Models
{
    public  class QuestionDto
    {
        public int Id { get; set; }

        public int QuizId { get; set; }

        public string Name { get; set; }
    }
}
