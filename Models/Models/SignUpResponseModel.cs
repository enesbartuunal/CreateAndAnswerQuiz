﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CreateAndAnswerQuiz.Models.Models
{
    public class SignUpResponseModel
    {
        public bool IsSuccessfulRegistration { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
