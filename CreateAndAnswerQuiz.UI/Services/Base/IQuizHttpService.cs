
using CreateAndAnswerQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.UI.Services.Base
{
    public interface IQuizHttpService
    {
        Task<List<string>> GetStorys();
        Task<QuizDto> GetStoryBodys(string head);
        Task<QuizResponseDto> CreateQuiz(QuizResponseDto quizResponseDto);
        Task<List<QuestionDto>> CreateQuestion(List<QuestionDto> questionDtos);
        Task<List<AnswerDto>> CreateAnswer(List<AnswerDto> answerDtos);
        Task<List<QuizResponseDto>> GetQuizess();

    }
}
