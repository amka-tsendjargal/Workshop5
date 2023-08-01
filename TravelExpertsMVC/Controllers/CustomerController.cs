using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelExpertsData;
using TravelExpertsMVC.Models;

namespace TravelExpertsMVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TravelExpertsContext _context;

        public CustomerController(TravelExpertsContext context)
        {
            _context = context;
        }
        // GET: CustomerController
        

        public IActionResult MyAccount()
        {
            int sessionCustId = (int)HttpContext.Session.GetInt32("CurrentCustomer");
            Customer currentCustomer = CustomerViewModel.GetCustomerById(_context, sessionCustId);
            return View(currentCustomer);
        }

        [HttpPost]
        public IActionResult MyAccount(Customer updatedCustomer)
        {
            int sessionCustId = (int)HttpContext.Session.GetInt32("CurrentCustomer");
            if (ModelState.IsValid)
            {
                try
                {
                    CustomerViewModel.UpdateCustomer(_context, sessionCustId, updatedCustomer);
                    TempData["Message"] = "Personal information successfully updated.";
                    return View();
                }
                catch
                {
                    TempData["Message"] = "Something went wrong while updating information. Please try again.";
                    TempData["IsError"] = true;
                    return View(updatedCustomer);
                }
            }
            else
            {
                return View(updatedCustomer);
            }
        }

        public IActionResult Login(string returnUrl)
        {
            if (returnUrl != null)
            {
                TempData["ReturnUrl"] = returnUrl;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(Customer customer)
        {
            Customer cust = CustomerViewModel.Authenticate(_context, customer.UserId, customer.UserPwd);
            if (cust == null) // if authentication fails
            {
                return View(); // stay on login page
            }
            HttpContext.Session.SetInt32("CurrentCustomer", cust.CustomerId); // create session for logged in customer
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, cust.UserId)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme); // cookies authentication
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            if (TempData["ReturnUrl"] == null || String.IsNullOrEmpty(TempData["ReturnUrl"].ToString()))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(TempData["ReturnUrl"].ToString());
            }
        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("CurrentCustomer");
            return RedirectToAction("Index", "Home");
        }

        //// GET: CustomerController/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        // GET: CustomerController/Register
        public ActionResult Register()
        {
            return View("Register", new Customer());
        }

        // POST: CustomerController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Customer customer, string confirmPassword)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check if the passwords match
                    if (customer.UserPwd != confirmPassword)
                    {
                        ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                        return View("Register", customer);
                    }

                    // Call the Add method from the CustomerViewModel class to add the customer to the database
                    CustomerViewModel.Add(_context, customer);

                    // Redirect to a success page after successful registration
                    return RedirectToAction("RegistrationSuccessful");
                }
                else
                {
                    // Use the "Register" view to display validation errors and re-render the form
                    return View("Register", customer);
                }
            }
            catch
            {
                // Handle any exceptions that occurred during registration
                TempData["Message"] = "Something went wrong while registering. Please try again.";
                TempData["IsError"] = true;
                return View("Register", customer);
            }
        }        
        
        [HttpGet]
        public IActionResult GetStatesProvinces(string country)
        {
            if (country == "Canada")
            {
                var provinces = new List<string> {  
                    "Alberta",
                    "British Columbia",
                    "Manitoba",
                    "New Brunswick",
                    "Newfoundland and Labrador",
                    "Nova Scotia",
                    "Ontario",
                    "Prince Edward Island",
                    "Quebec",
                    "Saskatchewan" };
                return Json(provinces);
            }
            else if (country == "USA")
            {
                var states = new List<string> {
                    "Alabama",
                    "Alaska",
                    "Arizona",
                    "Arkansas",
                    "California",
                    "Colorado",
                    "Connecticut",
                    "Delaware",
                    "Florida",
                    "Georgia",
                    "Hawaii",
                    "Idaho",
                    "Illinois",
                    "Indiana",
                    "Iowa",
                    "Kansas",
                    "Kentucky",
                    "Louisiana",
                    "Maine",
                    "Maryland",
                    "Massachusetts",
                    "Michigan",
                    "Minnesota",
                    "Mississippi",
                    "Missouri",
                    "Montana",
                    "Nebraska",
                    "Nevada",
                    "New Hampshire",
                    "New Jersey",
                    "New Mexico",
                    "New York",
                    "North Carolina",
                    "North Dakota",
                    "Ohio",
                    "Oklahoma",
                    "Oregon",
                    "Pennsylvania",
                    "Rhode Island",
                    "South Carolina",
                    "South Dakota",
                    "Tennessee",
                    "Texas",
                    "Utah",
                    "Vermont",
                    "Virginia",
                    "Washington",
                    "West Virginia",
                    "Wisconsin",
                    "Wyoming" };
                return Json(states);
            }
            else
            {
                return BadRequest();
            }
        }
        

        // GET: /CutomerController/RegistrationSuccessful
        public IActionResult RegistrationSuccessful()
        {
            return View();
        }

        // GET: CustomerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
