using CreateAndAnswerQuiz.Models.Models;
using CreateAndAnswerQuiz.UI.Services.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.UI.Services.Implemantation
{
    public class QuizHttpService : IQuizHttpService
    {
        private readonly HttpClient _client;

        public QuizHttpService(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<AnswerDto>> CreateAnswer(List<AnswerDto> answerDtos)
        {
            var content = JsonConvert.SerializeObject(answerDtos);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("story/createanswer", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<List<AnswerDto>>(data);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                return null;
            }
        }

        public async Task<List<QuestionDto>> CreateQuestion(List<QuestionDto> questionDtos)
        {
            var content = JsonConvert.SerializeObject(questionDtos);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("story/createquestion", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<List<QuestionDto>>(data);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                return null;
            }
        }

        public async Task<QuizResponseDto> CreateQuiz(QuizResponseDto quizResponseDto)
        {
            var content = JsonConvert.SerializeObject(quizResponseDto);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("story/createquiz", bodyContent);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<QuizResponseDto>(data);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                return null;
            }
        }

        public async Task<List<QuizResponseDto>> GetQuizess()
        {
            var response = await _client.GetAsync("story/quiz");
            var content = await response.Content.ReadAsStringAsync();
            var quizess = JsonConvert.DeserializeObject<List<QuizResponseDto>>(content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            return quizess;
        }

        public async Task<QuizDto> GetStoryBodys(string head)
        {
            var sendinghead = new QuizDto();
            sendinghead.Head = head;
            var content = JsonConvert.SerializeObject(sendinghead);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("story/quiz",bodyContent);
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<QuizDto>(data);
                return result;
            }
            else
            {
                var contentTemp = await response.Content.ReadAsStringAsync();
                return null;
            }
        }

        public async Task<List<string>> GetStorys()
        {
            var response = await _client.GetAsync("story/");
            var content = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<string>>(content);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content);
            }
            return products;
        }
    }
}
