using AutoMapper;
using CreateAndAnswerQuiz.Api.DataAccess.Context;
using CreateAndAnswerQuiz.Api.DataAccess.Entities;
using CreateAndAnswerQuiz.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly ApiDbContext _db;
        private readonly IMapper _mapper;

        public StoryController(ApiDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var httpClient = HttpClientFactory.Create();
            httpClient.DefaultRequestHeaders
     .Accept
     .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            var url = "https://www.wired.com/";

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {

                //StoryHead
                //-------------------------------------------------------------------------------
                var content = httpResponseMessage.Content;
                var data = await content.ReadAsStringAsync();
                string[] dataMember = new string[6];
                string[] linkMember = new string[5];
                List<string> result = new List<string>();

                dataMember[0] = data;

                var startWith = "SummaryItemHedLink-cgPsOZ icgRnM summary-item-tracking__hed-link summary-item__hed-link";

                for (int i = 1; i <= 5; i++)
                {
                    int findIndex = dataMember[(i - 1)].IndexOf(startWith) + 172;
                    dataMember[i] = dataMember[(i - 1)].Substring(findIndex);
                    var lastIndex = dataMember[i].IndexOf("SummaryItemHedBase-dZmlME fEkmfC summary");
                    var resultKey = dataMember[i].Substring(0, lastIndex - 13);
                    var linkIndex = dataMember[(i - 1)].IndexOf(startWith) + 165;
                    linkMember[i - 1] = dataMember[(i - 1)].Substring(linkIndex);
                    //var firstLinkIndex = linkMember[i - 1].IndexOf("href") + 5;
                    //var linkresultMember = linkMember[i - 1].Substring(firstLinkIndex);
                    var lastLinkIndex = linkMember[i - 1].IndexOf("><h2");
                    var resultValue = linkMember[i - 1].Substring(0, lastLinkIndex - 1);


                    //Story Body
                    //---------------------------------------------------------------------
                    var httpClientforStoryBody = HttpClientFactory.Create();
                    httpClientforStoryBody.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    var urlForStoryBody = "https://www.wired.com" + resultValue;

                    HttpResponseMessage httpResponseMessageForStoryBody = await httpClientforStoryBody.GetAsync(urlForStoryBody);

                    if (httpResponseMessageForStoryBody.StatusCode == HttpStatusCode.OK)
                    {
                        var contentStoryBody = httpResponseMessageForStoryBody.Content;
                        var dataForStoryBody = await contentStoryBody.ReadAsStringAsync();
                        var startIndexStory = dataForStoryBody.IndexOf("BaseWrap-sc-TURhJ BodyWrapper-ctnerm eTiIvU eFtlys") + 148;
                        var dataMemberForStory = dataForStoryBody.Substring(startIndexStory);
                        //var lastIndexStory = dataMemberForStory.IndexOf("GridItem-buSdEM gKfJBS") - 12;
                        //var storyBody = dataMemberForStory.Substring(0, lastIndexStory);

                        result.Add(resultKey);

                    }
                }
                return Ok(result);
            }
            return BadRequest();
        }


        [HttpPost("quiz")]
        public async Task<IActionResult> CreateQuiz(QuizDto head)
        {

            char[] letters = head.Head.ToCharArray();
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i] == ' ')
                {
                    letters[i] = '-';
                }

            }
            var newHead = new String(letters);
            var newHead2 = newHead.ToLower();
            var lastHead = Path.Combine("/story/", newHead2);
            var httpClient = HttpClientFactory.Create();
            httpClient.DefaultRequestHeaders
     .Accept
     .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            var url = "https://www.wired.com/";

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);

            if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
            {

                //StoryHead
                //-------------------------------------------------------------------------------
                var content = httpResponseMessage.Content;
                var data = await content.ReadAsStringAsync();
                string[] dataMember = new string[6];
                string[] linkMember = new string[5];
                List<string> result = new List<string>();

                dataMember[0] = data;

                var startWith = "SummaryItemHedLink-cgPsOZ icgRnM summary-item-tracking__hed-link summary-item__hed-link";

                for (int i = 1; i <= 5; i++)
                {
                    int findIndex = dataMember[(i - 1)].IndexOf(startWith) + 172;
                    dataMember[i] = dataMember[(i - 1)].Substring(findIndex);
                    var lastIndex = dataMember[i].IndexOf("SummaryItemHedBase-dZmlME fEkmfC summary");
                    var resultKey = dataMember[i].Substring(0, lastIndex - 13);
                    var linkIndex = dataMember[(i - 1)].IndexOf(startWith) + 165;
                    linkMember[i - 1] = dataMember[(i - 1)].Substring(linkIndex);
                    //var firstLinkIndex = linkMember[i - 1].IndexOf("href") + 5;
                    //var linkresultMember = linkMember[i - 1].Substring(firstLinkIndex);
                    var lastLinkIndex = linkMember[i - 1].IndexOf("><h2");
                    var resultValue = linkMember[i - 1].Substring(0, lastLinkIndex - 1);



                    //Story Body
                    //---------------------------------------------------------------------
                    var httpClientforStoryBody = HttpClientFactory.Create();
                    httpClientforStoryBody.DefaultRequestHeaders
             .Accept
             .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    if (resultValue == lastHead)
                    {
                        var urlForStoryBody = "https://www.wired.com" + resultValue;

                        HttpResponseMessage httpResponseMessageForStoryBody = await httpClientforStoryBody.GetAsync(urlForStoryBody);

                        if (httpResponseMessageForStoryBody.StatusCode == HttpStatusCode.OK)
                        {
                            var contentStoryBody = httpResponseMessageForStoryBody.Content;
                            var dataForStoryBody = await contentStoryBody.ReadAsStringAsync();
                            var startIndexStory = dataForStoryBody.IndexOf("BaseWrap-sc-TURhJ BodyWrapper-ctnerm eTiIvU eFtlys") + 148;
                            var dataMemberForStory = dataForStoryBody.Substring(startIndexStory);
                            //var lastIndexStory = dataMemberForStory.IndexOf("GridItem-buSdEM gKfJBS") - 12;
                            //var storyBody = dataMemberForStory.Substring(0, lastIndexStory);
                            var dto = new QuizDto();
                            dto.Head = dataForStoryBody;
                            return Ok(dto);

                        }
                    }
                }
                return BadRequest();
            }
            return BadRequest();
        }

        [HttpPost("createquiz")]
        public async Task<IActionResult> CreateQuizName(QuizResponseDto quizResponseDto)
        {
            var quiz = new Quiz();
            quiz.StoryName = quizResponseDto.StoryName;
            await _db.Set<Quiz>().AddAsync(quiz);
            await _db.SaveChangesAsync();
            quizResponseDto.Id = quiz.Id;
            return Ok(quizResponseDto);
        }
        [HttpGet("quiz")]
        public async Task<IActionResult> GetQuizess()
        {
            var list = _db.Set<Quiz>().ToList();
            var modellist = _mapper.Map<List<Quiz>, List<QuizResponseDto>>(list);

            return Ok(modellist);
        }


        [HttpPost("createquestion")]
        public async Task<IActionResult> CreateQuizName(List<QuestionDto> questionDtoList)
        {
            foreach (var questionDto in questionDtoList)
            {
                var question = new Question();
                question.QuizId = questionDto.QuizId;
                question.Name = questionDto.Name;
                await _db.Set<Question>().AddAsync(question);
                await _db.SaveChangesAsync();
                questionDto.Id = question.Id;
            }

            return Ok(questionDtoList);
        }

        [HttpPost("createanswer")]
        public async Task<IActionResult> CreateAnswer(List<AnswerDto> answerDtoList)
        {
            foreach (var answerDto in answerDtoList)
            {
                var answer = new Answer();
                answer.Name = answerDto.Name;
                answer.QuestionId = answerDto.QuestionId;
                await _db.Set<Answer>().AddAsync(answer);
                await _db.SaveChangesAsync();
            }
            return Ok(answerDtoList);
        }
    }
}
