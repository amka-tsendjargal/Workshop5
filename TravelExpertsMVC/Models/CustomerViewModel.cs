using TravelExpertsData;

namespace TravelExpertsMVC.Models
{
    public class CustomerViewModel
    {
        private static List<Customer> GetCustomers(TravelExpertsContext db)
        {
            List<Customer> customers = db.Customers.ToList();
            return customers;
        }

        // get customer by id
        public static Customer GetCustomerById(TravelExpertsContext db, int id)
        {
            Customer customer = db.Customers.Find(id);
            return customer;
        }

        // Will use for registering
        public static void Add(TravelExpertsContext db, Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        public static void UpdateCustomer(TravelExpertsContext db, int id, Customer newCustomer)
        {
            // Find the existing Customer record in the database based on the provided id.
            Customer customer = db.Customers.Find(id);
            // Check if the customer with the provided id exists in the database.
            if (customer != null)
            {
                customer.CustFirstName = newCustomer.CustFirstName;
                customer.CustLastName = newCustomer.CustLastName;
                customer.CustAddress = newCustomer.CustAddress;
                customer.CustCity = newCustomer.CustCity;
                customer.CustProv = newCustomer.CustProv;
                customer.CustPostal = newCustomer.CustPostal;
                customer.CustCountry = newCustomer.CustCountry;
                customer.CustHomePhone = newCustomer.CustHomePhone;
                customer.CustBusPhone = newCustomer.CustBusPhone;
                customer.CustEmail = newCustomer.CustEmail;
                customer.AgentId = newCustomer.AgentId;
                customer.UserId = newCustomer.UserId;
                customer.UserPwd = newCustomer.UserPwd;
                db.SaveChanges();
            }
        }

        public static Customer Authenticate(TravelExpertsContext db, string userId, string userPwd)
        {
            var customer = GetCustomers(db).SingleOrDefault(cust => cust.UserId == userId &&
                                                                    cust.UserPwd == userPwd);
            return customer;
        }
    }
}
