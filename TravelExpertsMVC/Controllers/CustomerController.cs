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
            "AB", // Alberta
            "BC", // British Columbia
            "MB", // Manitoba
            "NB", // New Brunswick
            "NL", // Newfoundland and Labrador
            "NS", // Nova Scotia
            "ON", // Ontario
            "PE", // Prince Edward Island
            "QC", // Quebec
            "SK"  // Saskatchewan
        };
                return Json(provinces);
            }
            else if (country == "USA")
            {
                var states = new List<string> {
            "AL", // Alabama
            "AK", // Alaska
            "AZ", // Arizona
            "AR", // Arkansas
            "CA", // California
            "CO", // Colorado
            "CT", // Connecticut
            "DE", // Delaware
            "FL", // Florida
            "GA", // Georgia
            "HI", // Hawaii
            "ID", // Idaho
            "IL", // Illinois
            "IN", // Indiana
            "IA", // Iowa
            "KS", // Kansas
            "KY", // Kentucky
            "LA", // Louisiana
            "ME", // Maine
            "MD", // Maryland
            "MA", // Massachusetts
            "MI", // Michigan
            "MN", // Minnesota
            "MS", // Mississippi
            "MO", // Missouri
            "MT", // Montana
            "NE", // Nebraska
            "NV", // Nevada
            "NH", // New Hampshire
            "NJ", // New Jersey
            "NM", // New Mexico
            "NY", // New York
            "NC", // North Carolina
            "ND", // North Dakota
            "OH", // Ohio
            "OK", // Oklahoma
            "OR", // Oregon
            "PA", // Pennsylvania
            "RI", // Rhode Island
            "SC", // South Carolina
            "SD", // South Dakota
            "TN", // Tennessee
            "TX", // Texas
            "UT", // Utah
            "VT", // Vermont
            "VA", // Virginia
            "WA", // Washington
            "WV", // West Virginia
            "WI", // Wisconsin
            "WY"  // Wyoming
        };
                return Json(states);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IActionResult ValidatePostalCode(string country, string postalCode)
        {
            bool isValidPostalCode = false;

            if (country == "USA")
            {
                var regex = new System.Text.RegularExpressions.Regex(@"^(\d{5})(-\d{4})?$");
                isValidPostalCode = regex.IsMatch(postalCode);
            }
            else if (country == "Canada")
            {
                var regex = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$");
                isValidPostalCode = regex.IsMatch(postalCode);
            }

            return Json(new { isValid = isValidPostalCode });
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
