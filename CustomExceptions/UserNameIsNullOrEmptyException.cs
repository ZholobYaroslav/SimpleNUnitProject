using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA_APP_Test_Project.InternalChatModule;

namespace QA_APP_Test_Project.CustomExceptions
{
    internal class UserNameIsNullOrEmptyException: ChatException
    {
        public UserNameIsNullOrEmptyException(string msg, InternalChatService.Chat chat): base(msg, chat)
        {

        }
    }
}
