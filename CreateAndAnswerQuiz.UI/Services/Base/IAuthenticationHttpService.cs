
using CreateAndAnswerQuiz.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateAndAnswerQuiz.UI.Services.Base
{
    public interface IAuthenticationHttpService
    {
        Task<IEnumerable<string>> RegisterUser(SignUpModel model);
        Task<string> Login(SignInModel model);
        Task Logout();
        Task<string> RefreshToken();
        Task<string> GetUserIdbyName(string userName);
    }
}
