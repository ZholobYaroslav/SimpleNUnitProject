using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QA_APP_Test_Project.PublicationsModule
{
    internal class PublicationsRepository
    {
        public List<PublicationsUserService> AllPublications { get; set; }

        public PublicationsRepository(List<PublicationsUserService> allPublications)
        {
            AllPublications = allPublications ?? throw new ArgumentNullException(nameof(allPublications));
        }
        public PublicationsRepository()
        {
            AllPublications = new List<PublicationsUserService>();
        }
        //
        //Search
        private List<PublicationsUserService.Publication> FlattenAndGetPublications()
        {
            List<PublicationsUserService.Publication> res = new();
            this.AllPublications.ForEach(pus => res.AddRange(pus.Publications));
            return res;
        }
        //Search for...
        public List<PublicationsUserService.Publication> SearchForSalary(decimal startSalary, decimal salaryOffset)
        {
            return FlattenAndGetPublications().FindAll(p =>
                p.Salary >= (startSalary - salaryOffset / 2) && p.Salary <= (startSalary + salaryOffset / 2));
        }
    }
}
