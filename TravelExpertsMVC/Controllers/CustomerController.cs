using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.RegularExpressions;
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

        [Authorize]
        public IActionResult MyAccount()
        {
            int sessionCustId = (int)HttpContext.Session.GetInt32("CurrentCustomer");
            Customer currentCustomer = CustomerViewModel.GetCustomerById(_context, sessionCustId);
            ViewBag.UserFName = currentCustomer.CustFirstName;
            ViewBag.UserLName = currentCustomer.CustLastName;
            ViewBag.Phone = currentCustomer.CustHomePhone;
            ViewBag.UserProv = currentCustomer.CustProv;
            ViewBag.UserCountry = currentCustomer.CustCountry;
            return View(currentCustomer);
        }
        [Authorize]
        [HttpPost]
        public IActionResult MyAccount(Customer updatedCustomer)
        {
            int sessionCustId = (int)HttpContext.Session.GetInt32("CurrentCustomer");
            Customer currentCustomer = CustomerViewModel.GetCustomerById(_context, sessionCustId);
            if (updatedCustomer.CustCountry == "USA")
            {
                var patternRegexU = @"^\d{5}(?:[-\s]\d{4})?$";
                Regex regexU = new Regex(patternRegexU);

                if (!regexU.IsMatch(updatedCustomer.CustPostal))
                {
                    ModelState.AddModelError("CustPostal", "Invalid postal code format for USA.");
                    return View("MyAccount", currentCustomer);
                }
            }
            else if (updatedCustomer.CustCountry == "Canada")
            {
                var patternRegexC = @"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$";
                Regex regexC = new Regex(patternRegexC);

                if (!regexC.IsMatch(updatedCustomer.CustPostal))
                {
                    ModelState.AddModelError("CustPostal", "Invalid postal code format for Canada.");
                    return View("MyAccount", currentCustomer);
                }
            }
            if (ModelState.IsValid)
            {
                try
                {
                    CustomerViewModel.UpdateCustomer(_context, sessionCustId, updatedCustomer);
                    TempData["Message"] = "Personal information successfully updated.";
                    return RedirectToAction("MyAccount", "Customer");
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
                TempData["IsError"] = true;
                TempData["ErrorMessage"] = "Login failed. Please try again.";
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
                // Check if the postal code format is valid based on the selected country
                if (customer.CustCountry == "USA")
                {
                    var patternRegexU = @"^\d{5}(?:[-\s]\d{4})?$";
                    Regex regexU = new Regex(patternRegexU);

                    if (!regexU.IsMatch(customer.CustPostal))
                    {
                        ModelState.AddModelError("CustPostal", "Invalid postal code format for USA.");
                        return View("Register", customer);
                    }
                }
                else if (customer.CustCountry == "Canada")
                {
                    var patternRegexC = @"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$";
                    Regex regexC = new Regex(patternRegexC);

                    if (!regexC.IsMatch(customer.CustPostal))
                    {
                        ModelState.AddModelError("CustPostal", "Invalid postal code format for Canada.");
                        return View("Register", customer);
                    }
                }

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
