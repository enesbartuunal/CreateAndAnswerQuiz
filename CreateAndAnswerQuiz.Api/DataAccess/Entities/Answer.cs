using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.Api.DataAccess.Entities
{
    public class Answer
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsTrue { get; set; }
        //Relations

        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
