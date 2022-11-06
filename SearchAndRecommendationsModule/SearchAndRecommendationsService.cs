using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QA_APP_Test_Project.PublicationsModule;

namespace QA_APP_Test_Project.SearchAndRecommendationsModule
{
    internal class SearchAndRecommendationsService
    {
        //internal class Vacancy
        //internal class Candidacy
        public PublicationsRepository PublicationsRepository { get; set; }
        public List<PublicationsUserService.Publication>? SearchResults { get; set; }

        public SearchAndRecommendationsService(PublicationsRepository allPublications)
        {
            PublicationsRepository = allPublications ?? throw new ArgumentNullException(nameof(allPublications));
            SearchResults = new();
        }
        public SearchAndRecommendationsService()
        {
            PublicationsRepository = new();
            SearchResults = new();
        }

        //Search + filters(ZP(max, min), Dir, City)
        //Search(MaxSymbolsCount + #$%^&) if(0) => alert
        public void SearchForSalary(decimal startSalary, decimal salaryOffset)
        {
            SearchResults = new(PublicationsRepository.SearchForSalary(startSalary, salaryOffset));
            if (!SearchResults.Any())
            {
                throw new ApplicationException(nameof(SearchResults));
            }
        }

        //Sort res by Data, Job, City, ZP


        //Notifys + email

        //view vk + apply

        //make filters to get recommendations 
        //view recommendations + aply
    }
}
