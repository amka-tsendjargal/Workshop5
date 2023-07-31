using TravelExpertsData;

namespace TravelExpertsMVC.Models
{
    public class CustomerModel
    {
        private static List<Customer> GetCustomers(TravelExpertsContext db)
        {
            List<Customer> customers = db.Customers.ToList();
            return customers;
        }

        // Will use for registering
        public static void Add(TravelExpertsContext db, Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public static Customer Authenticate(TravelExpertsContext db, string userId, string userPwd)
        {
            var customer = GetCustomers(db).SingleOrDefault(cust => cust.UserId == userId &&
                                                                    cust.UserPwd == userPwd);
            return customer;
        }
    }
}
