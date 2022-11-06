using QA_APP_Test_Project.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QA_APP_Test_Project.EmploymentCenterModule.EmploymentCenterService;

namespace QA_APP_Test_Project.EmploymentCenterModule
{
    internal class EmploymentCenterService
    {
        internal class Center
        {
            public string Name { get; set; }
            public int Priority { get; set; }
            public string? ExternalLink { get; set; }
            public Center()
            {
                Name = "none";
                Priority = 0;
                ExternalLink = null;
            }
            public Center(string name, int priority, string? externalLink)
            {
                Name = name;
                Priority = priority;
                ExternalLink = externalLink;
            }
        }
        public List<Center>? centers { get; set; }
        public List<Center>? FollowingCenters { get; }
        public EmploymentCenterService(List<Center>? centersCollection)
        {
            if (centersCollection == null)
            {
                centers = new List<Center>();
                centers.Add(new Center() { Name = "LinkedIn", Priority = 10, ExternalLink = "https://www.linkedin.com/feed/" });
                centers.Add(new Center() { Name = "Work.ua", Priority = 5, ExternalLink = "https://www.work.ua/" });
                centers.Add(new Center() { Name = "Rabota.ua", Priority = 3, ExternalLink = "https://rabota.ua/ua" });
                centers.Add(new Center() { Name = "Jobs.Dou.ua", Priority = 9, ExternalLink = "https://jobs.dou.ua/" });
                centers.Add(new Center() { Name = "Recruitika", Priority = 7, ExternalLink = "https://recruitika.com/" });
                centers.Add(new Center() { Name = "Djinni", Priority = 8, ExternalLink = "https://djinni.co/" });
                centers.Add(new Center() { Name = "Hireforukraine", Priority = 6, ExternalLink = "https://hireforukraine.org/" });
                centers.Add(new Center() { Name = "Olx", Priority = 2, ExternalLink = "https://www.olx.ua/uk/rabota/" });
            }
            else
            {
                centers = new List<Center>(centersCollection);
            }
            FollowingCenters = new();
        }

        //Sort by ...
        public bool SortByName()
        {
            if (centers.Count != 0)
            {
                centers.Sort((centerA, centerB) => centerA.Name.CompareTo(centerB.Name));
                return true;
            }
            return false;
        }
        public bool SortByPriority()
        {
            if (centers.Count != 0)
            {
                centers.Sort(delegate (Center A, Center B)
                {
                    return A.Priority.CompareTo(B.Priority);
                });
                return true;
            }
            return false;
        }
        //Choose with EC to follow
        public void FollowEC(Center center)
        {
            if (FollowingCenters.Contains(center))
            {
                throw new CollectionsHasDuplicateException("Duplicate Employment Center could not be followed.", center);
            }
            else
            {
                FollowingCenters.Add(center);
            }
        }
        public void UnFollowEC(Center center)
        {
            if (!FollowingCenters.Remove(center))
            {
                throw new RemoveNonexistedCenterException("Could not be unfollowed EC which was not followed.", center);
            }
        }
        //Set Priority to EC which user follows
        public void SetPriorityToFollowedEC(Center center, int priority)
        {
            if (!FollowingCenters.Contains(center))
            {
                throw new SetPriorityToNotFollowedEcException("Could not set priority to EC which was not followed", center);
            }
            if(priority < 0 || priority > 10)
            {
                throw new SetPriorityOutOfRangeException("Priority value was less than 0 or greater than 10.", center);
            }
            center.Priority = priority;
        }

    }
    // Tests
    [TestFixture]
    internal class EcServiceTests
    {
        private EmploymentCenterService _employmentCenterService;
        [SetUp]
        public void SetUp()
        {
            //Arrange
            _employmentCenterService = new(null);
        }
        #region Sort
        [Test]
        public void EcServiceIsSortedByName()
        {
            //Act
            var res = _employmentCenterService.SortByName();
            //Assert
            Assert.AreEqual(res, true);
        }
        [Test]
        public void EcServiceIsSortedByNameEmptyCollection()
        {
            //Arrange
            _employmentCenterService = new EmploymentCenterService(new List<Center>());
            //Act
            var res = _employmentCenterService.SortByName();
            //Assert
            Assert.AreEqual(res, false);
        }
        [Test]
        public void EcServiceIsSortedByPriority()
        {
            //Act
            var res = _employmentCenterService.SortByPriority();
            //Assert
            Assert.AreEqual(res, true);
        }
        [Test]
        public void EcServiceIsSortedByPriorityEmptyCollection()
        {
            //Arrange
            _employmentCenterService = new(new List<Center>());
            //Act
            var res = _employmentCenterService.SortByPriority();
            //Assert
            Assert.AreEqual(res, false);
        }
        #endregion

        #region (Un)Follow
        [Test]
        public void EcServiceFollowEC()
        {
            //Arrange
            var linkedIn = new Center()
                { Name = "LinkedIn", Priority = 10, ExternalLink = "https://www.linkedin.com/feed/" };
            //Act
            _employmentCenterService.FollowEC(linkedIn);
            //Assert
            Assert.Contains(linkedIn, _employmentCenterService.FollowingCenters);
        }
        [Test]
        public void EcServiceFollowECDuplicateShouldThrowException()
        {
            //Arrange
            var linkedIn = new Center()
                { Name = "LinkedIn", Priority = 10, ExternalLink = "https://www.linkedin.com/feed/" };
            //Act
            _employmentCenterService.FollowEC(linkedIn);
            //Assert
            var ex = Assert.Throws<CollectionsHasDuplicateException>(
                () => _employmentCenterService.FollowEC(linkedIn)
                );
            StringAssert.StartsWith("Duplicate Employment Center could not be followed.", ex.Message);
        }
        //UnFollow
        [Test]
        public void EcServiceUnFollowEC()
        {
            //Arrange
            var linkedIn = new Center()
                { Name = "LinkedIn", Priority = 10, ExternalLink = "https://www.linkedin.com/feed/" };
            //Act
            _employmentCenterService.FollowEC(linkedIn);
            _employmentCenterService.UnFollowEC(linkedIn);
            //Assert
            Assert.IsEmpty(_employmentCenterService.FollowingCenters);
        }
        [Test]
        public void EcServiceUnFollowEcShouldThrowExceptionDueToInvalidRemove()
        {
            //Arrange
            var linkedIn = new Center()
                { Name = "LinkedIn", Priority = 10, ExternalLink = "https://www.linkedin.com/feed/" };
            var anon = new Center()
                { Name = "Anon", Priority = 1, ExternalLink = "https://www.anon.com/" };
            //Act
            _employmentCenterService.FollowEC(linkedIn);
            //Assert
            var ex = Assert.Throws<RemoveNonexistedCenterException>(
                () => _employmentCenterService.UnFollowEC(anon)
            );
            StringAssert.StartsWith("Could not be unfollowed EC which was not followed.", ex.Message);
        }
        #endregion

        #region PriorityToFollowedEC
        [Test]
        public void EcServiceSetPriorityToFollowedEC()
        {
            //Arrange
            var priority = 10;
            var linkedIn = new Center()
                { Name = "LinkedIn", Priority = 10, ExternalLink = "https://www.linkedin.com/feed/" };
            //Act
            _employmentCenterService.FollowEC(linkedIn);
            _employmentCenterService.SetPriorityToFollowedEC(linkedIn, priority);
            //Assert
            Assert.AreEqual(priority, _employmentCenterService.FollowingCenters.First(c => c.Name.Equals(linkedIn.Name)).Priority);
        }
        [Test]
        public void EcServiceSetPriorityToFollowedECShouldThrown_SetPriorityToNotFollowedEcException()
        {
            //Arrange
            var priority = 10;
            var linkedIn = new Center()
                { Name = "LinkedIn", Priority = 10, ExternalLink = "https://www.linkedin.com/feed/" };
            //Act
            //Assert
            var ex = Assert.Throws<SetPriorityToNotFollowedEcException>(
                () => _employmentCenterService.SetPriorityToFollowedEC(linkedIn, priority));
            StringAssert.StartsWith("Could not set priority to EC which was not followed", ex.Message);
        }
        #endregion
    }
}