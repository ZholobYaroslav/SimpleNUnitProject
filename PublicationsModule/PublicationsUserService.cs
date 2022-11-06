using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace QA_APP_Test_Project.PublicationsModule
{
    internal class PublicationsUserService
    {
        internal abstract class Publication
        {
            public enum CityOfWork
            {
                Lviv,
                Kyiv,
                Kharkiv,
                Lutsk,
                Ternopil,
                Odesa,
                AllCities
            }
            public enum EmploymentType
            {
                fullTime,
                partTime
            }
            public enum JobDirection
            {
                DataScience,
                FullStack,
                FrontEnd,
                BackEnd,
                GameDev,
                Mobile,
                Cloud,
                IoT,
                ML
            }

            public User? Author { get; }
            public string Name { get; }
            public decimal Salary { get; }
            public CityOfWork City { get; }
            public EmploymentType Employment { get; }
            public JobDirection Direction { get; }
            public byte ExperienceYears { get; }
            public DateTime CreationDate { get; }
            public string Description { get; }

            public bool IsOpen { get; set; } = true;
            public List<User>? UsersWhoResponded { get; set; }// why in ctor?
            public User? ApprovedUser { get; set; }

            internal Publication(string name, string description, decimal? salary, CityOfWork city, EmploymentType employment, JobDirection direction, byte? experienceYears, 
                User author, List<User>? users)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentException(nameof(name));
                }
                Name = name;
                if (string.IsNullOrEmpty(description))
                {
                    throw new ArgumentException(nameof(description));
                }
                Description = description;
                if (salary == null || salary <= 0 || salary >= decimal.MaxValue)
                {
                    throw new ArgumentException(nameof(salary));
                }
                Salary = (decimal)salary;
                City = city;
                Employment = employment;
                Direction = direction;
                if (experienceYears == null || experienceYears < 0 || experienceYears > 40)
                {
                    throw new ArgumentException(nameof(experienceYears));
                }
                ExperienceYears = (byte)experienceYears;
                CreationDate = DateTime.Now;
                if (users == null || users.Count() == 0)
                {
                    UsersWhoResponded = new List<User>();
                }
                else
                {
                    UsersWhoResponded = new List<User>(users);
                }
                Author = author ?? throw new ArgumentNullException(nameof(author));

            }
            internal Publication()
            {
                Author = null;
                Name = "JohnDoe";
                Salary = 300.00m;
                City = CityOfWork.Lviv;
                Employment = EmploymentType.fullTime;
                Direction = JobDirection.FullStack;
                ExperienceYears = 0;
                Description = "";
                UsersWhoResponded = null;
            }
        }
        public User PublicationsAuthor { get; set; }
        public List<Publication> Publications { get; set; }

        public PublicationsUserService(User publicationsAuthor, List<Publication> publications)
        {
            PublicationsAuthor = publicationsAuthor ?? throw new ArgumentNullException(nameof(publicationsAuthor));
            Publications = publications ?? throw new ArgumentNullException(nameof(publications));
        }
        public PublicationsUserService()
        {
            PublicationsAuthor = new User();
            Publications = new List<Publication>();
        }

        public void CreatePublication(Publication publication)
        {
            if (publication == null)
            {
                throw new ArgumentNullException(nameof(publication));
            }
            if (Publications.Contains(publication))
            {
                throw new ArgumentException(nameof(publication));
            }
            this.Publications.Add(publication);
        }

        public void ClosePublication(Publication publication)
        {
            if (publication == null)
            {
                throw new ArgumentNullException(nameof(publication));
            }
            if (!Publications.Contains(publication))
            {
                throw new ArgumentException(nameof(publication));
            }
            if (!publication.IsOpen)
            {
                throw new InvalidOperationException(nameof(publication));
            }
            publication.IsOpen = false;
        }

        public void DeletePublication(Publication publication)
        {
            if (publication == null)
            {
                throw new ArgumentNullException(nameof(publication));
            }
            if (!Publications.Contains(publication) || publication.IsOpen)
            {
                throw new ArgumentException(nameof(publication));
            }
            Publications.Remove(publication);
        }

        public Publication EditPublication(Publication publicationToChange, Publication publicationEdited)
        {
            Publication undoChanges = publicationToChange;
            if (!Publications.Remove(publicationToChange))
            {
                throw new InvalidOperationException(nameof(publicationToChange));
            }
            this.CreatePublication(publicationEdited);
            return undoChanges;
        }
        // Not finished yet
        public bool ChooseAndChat(Publication publication, User user)
        {
            if (!Publications.Contains(publication) || !publication.UsersWhoResponded.Contains(user))
            {
                throw new ArgumentException();
            }
            //return user.StartChat();
            return true;
        }

        public void ApproveChosenUser(Publication publication, User user)
        {
            if (!Publications.Contains(publication) || !publication.UsersWhoResponded.Contains(user))
            {
                throw new ArgumentException();
            }
            publication.ApprovedUser = user;
            this.ClosePublication(publication);
        }
    }
}