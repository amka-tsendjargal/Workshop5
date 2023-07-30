using TravelExpertsData;

namespace TravelExpertsMVC.Models
{
    public class ContactViewModel
    {
        public int AgencyId { get; set; }
        public string AgncyAddress { get; set; }
        public string AgncyCity { get; set; }
        public string AgncyProv { get; set; }
        public string AgncyPostal { get; set; }
        public string AgncyCountry { get; set; }
        public string AgncyPhone { get; set; }
        public string AgncyFax { get; set; }

        // Add a list of agents for each agency
        public List<Agent> Agents { get; set; }
    }
}
