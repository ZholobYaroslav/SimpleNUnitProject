using QA_APP_Test_Project.CustomExceptions;
using QA_APP_Test_Project.EmploymentCenterModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QA_APP_Test_Project.EmploymentCenterModule.EmploymentCenterService;

namespace QA_APP_Test_Project.InternalChatModule
{
    internal class InternalChatService
    {
        internal class Chat
        {
            public string UserNickName { get; set; }
            public bool IsBlocked { get; set; }
            public const int maxFileSize = 2000;
            public Chat()
            {
                UserNickName = "";
                IsBlocked = false;
            }
            public Chat(string userNickName, bool isBlocked)
            {
                UserNickName = userNickName;
                IsBlocked = isBlocked;
            }
        }
        public List<Chat> activeChats { get; set; }
        public List<Chat> blockedChats { get; }
        public InternalChatService(List<Chat>? aChats)
        {
            if (aChats == null || aChats.Count == 0)
            {
                activeChats = new();
                StartChat("Billy");
                StartChat("Van Darkholme");
                StartChat("Monkey D. Luffy");
                StartChat("Ken Kaneki");
                StartChat("Jotaro Kujo");
                StartChat("Avatar Aang");
                StartChat("Dio Brando");
                StartChat("Jiorno Jowanna");
                StartChat("Bruno Buchelatti");
                StartChat("Guido Mista");
            }
            else
            {
                activeChats = new List<Chat>(aChats);
            }
            blockedChats = new();
        }
        //Chat Manipulations
        public void StartChat(string? whoToStartTheDialogWith)
        {
            if (whoToStartTheDialogWith == null || whoToStartTheDialogWith == "")
            {
                throw new UserNameIsNullOrEmptyException("Who to start the chat with? Name is null or empty.", null);
            }
            Chat chat = new InternalChatService.Chat(whoToStartTheDialogWith, false);
            activeChats.Add(chat);
        }
        public Chat DeleteChat(string userName)
        {
            Chat chat = activeChats.Find(c => c.UserNickName == userName);
            if (chat is null)
            {
                throw new ChatException("Could not find chat with provided UserNickName", chat);
            }
            activeChats.Remove(chat);
            return chat;

        }
        public void BlockChat(string userName)
        {
            blockedChats.Add(DeleteChat(userName));
        }
        //
        public void SendFile(Chat chat, int fileSize)
        {
            if (!(activeChats.Contains(chat) && fileSize < Chat.maxFileSize))
            {
                throw new ChatException("Cound not send file due to chat non-existence or file size limitations", chat);
            }
        }
    }
    // Tests
    [TestFixture]
    internal class IChatTests
    {
        private InternalChatService _internalChatService;
        [SetUp]
        public void SetUp()
        {
            _internalChatService = new(null);
        }
        #region ChatManipulations
        [Test]
        public void IChatServiceStartChat()
        {
            //Arrange
            var chatAA = new InternalChatService.Chat("Bob Marley", false);
            //Act
            _internalChatService.StartChat(chatAA.UserNickName);
            //Assert
            Assert.That(_internalChatService.activeChats.Any(c => c.UserNickName == "Bob Marley"));
        }
        [Test]
        public void IChatServiceStartChatShouldThrow_UserNameIsNullOrEmptyException()
        {
            //Arrange
            var chatAA = new InternalChatService.Chat();
            //Act
            //Assert
            var ex = Assert.Throws<UserNameIsNullOrEmptyException>(
                () => _internalChatService.StartChat(chatAA.UserNickName));
            StringAssert.StartsWith("Who to start the chat with? Name is null or empty.", ex.Message);
        }
        [Test]
        public void IChatServiceDeleteChat()
        {
            //Arrange
            string existingChat = "Jotaro Kujo";
            //Act
            _internalChatService.DeleteChat(existingChat);
            //Assert
            Assert.AreEqual(false, _internalChatService.activeChats.Any(c => c.UserNickName == existingChat));
        }
        [Test]
        public void IChatServiceBlockChat()
        {
            //Arrange
            var existingChat = "Bruno Buchelatti";
            //Act
            _internalChatService.BlockChat(existingChat);
            //Assert
            Assert.AreEqual(true, _internalChatService.blockedChats.Any(c => c.UserNickName == existingChat));
        }
        #endregion
        [Test]
        public void IChatServiceSendFileShouldThrowExceptionDueFileSizeLimits()
        {
            //Arrange
            var chatAA = new InternalChatService.Chat("Avatar Aang", false);
            //Act
            //Assert
            var ex = Assert.Throws<ChatException>(
                () => _internalChatService.SendFile(chatAA, 3000)
            );
            StringAssert.StartsWith("Cound not send file due to chat non-existence or file size limitations", ex.Message);
        }
    }
}
