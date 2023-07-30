using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelExpertsData;
using TravelExpertsMVC.Models;

namespace TravelExpertsMVC.Controllers
{
    public class AgencyController : Controller
    {
        private readonly TravelExpertsContext _context;

        public AgencyController(TravelExpertsContext context)
        {
            _context = context;
        }
        // GET: ContactController
        public IActionResult Index()
        {
            var agencies = _context.Agencies.Include(a => a.Agents);

            // Convert the data to the ContactViewModel
            var contactViewModels = new List<ContactViewModel>();
            foreach (var agency in agencies)
            {
                var viewModel = new ContactViewModel
                {
                    AgencyId = agency.AgencyId,
                    AgncyAddress = agency.AgncyAddress,
                    AgncyCity = agency.AgncyCity,
                    AgncyProv = agency.AgncyProv,
                    AgncyPostal = agency.AgncyPostal,
                    AgncyCountry = agency.AgncyCountry,
                    AgncyPhone = agency.AgncyPhone,
                    AgncyFax = agency.AgncyFax,
                    Agents = agency.Agents.ToList() // Populate the list of agents for each agency
                };

                contactViewModels.Add(viewModel);
            }

            return View(contactViewModels);
        }
        // GET: ContactController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContactController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactController/Create
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

        // GET: ContactController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContactController/Edit/5
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

        // GET: ContactController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContactController/Delete/5
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
