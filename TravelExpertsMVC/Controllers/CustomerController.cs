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
        public ActionResult Register()
        {
            return View(new Customer());
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                
                return View(customer);
            }          
            return RedirectToAction("RegistrationSuccessful");
        }

        public IActionResult RegistrationSuccessful()
        {
            return View();
        }

        public IActionResult MyAccount()
        {
            int sessionCustId = (int)HttpContext.Session.GetInt32("CurrentCustomer");
            Customer currentCustomer = CustomerModel.GetCustomerById(_context, sessionCustId);
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
                    CustomerModel.UpdateCustomer(_context, sessionCustId, updatedCustomer);
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
            Customer cust = CustomerModel.Authenticate(_context, customer.UserId, customer.UserPwd);
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

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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
