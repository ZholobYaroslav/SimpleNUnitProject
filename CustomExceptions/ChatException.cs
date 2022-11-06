using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QA_APP_Test_Project.EmploymentCenterModule.EmploymentCenterService;
using static QA_APP_Test_Project.InternalChatModule.InternalChatService;

namespace QA_APP_Test_Project.CustomExceptions
{
    internal class ChatException : Exception
    {
        public Chat _chat { get; }
        public ChatException(string msg, Chat chat) : base(msg)
        {
            this._chat = chat;
        }
    }
}
