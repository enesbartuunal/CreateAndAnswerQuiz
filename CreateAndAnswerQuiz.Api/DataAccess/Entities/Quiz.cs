using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.Api.DataAccess.Entities
{
    public class Quiz
    {
        public int Id { get; set; }

        public string StoryName { get; set; }
        //Relations

        public virtual List<Question> Questions { get; set; }

    }
}
