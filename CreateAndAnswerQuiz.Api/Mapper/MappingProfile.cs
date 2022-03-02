using AutoMapper;
using CreateAndAnswerQuiz.Api.DataAccess.Entities;
using CreateAndAnswerQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.Api.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Quiz, QuizResponseDto>();
        }
    }
}
