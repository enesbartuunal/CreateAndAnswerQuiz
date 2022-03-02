using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.Api.DataAccess.Entities
{
    public class Question
    {
        public int Id { get; set; }

        public string Name { get; set; }
        //Relations
        public int QuizId { get; set; }

        public virtual Quiz  Quiz { get; set; }

        public virtual List<Answer> Answers { get; set; }
    }
}
