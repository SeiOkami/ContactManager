using Contacts.WebClient.Models;
using Contacts.Shared.Models;
using Contacts.WebClient.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Contacts.Shared.Services;
using Contacts.WebClient.Extensions;

namespace Contacts.WebClient.Controllers
{
    public class ContactsController : BaseController
    {

        private readonly IWebAPIService _webAPI;
        private readonly ILogger<HomeController> _logger;

        public ContactsController(ILogger<HomeController> logger, IWebAPIService webAPI)
        {
            _logger = logger;
            _webAPI = webAPI;
        }

        //[HttpGet("{userId?}")]
        [HttpGet()]
        [Authorize]
        public async Task<ActionResult> Index([FromQuery]Guid? userId)
        {
            try
            {
                if (userId == null || !IsAdmin)
                    userId = UserId;

                var token = await HttpContext.GetTokenAsync();

                var contacts = await _webAPI.ListContactsAsync(token, userId);

                if (contacts != null)
                {
                    contacts.IsThisUser = (userId == UserId);
                    contacts.User = await _webAPI.GetUserAsync(token, (Guid)userId);
                }

                return View(contacts);
            }
            catch
            {
                return RedirectToAction("Logout", "Account");
            }
        }

        [Authorize]
        public async Task<FileStreamResult> Export()
        {
            var token = await HttpContext.GetTokenAsync();
            var fileStream = await _webAPI.ExportContactsAsync(token);
            return File(fileStream, "application/json", "contacts.json");
        }
        
        [Authorize]
        public ActionResult Import()
        {
            var model = new ImportContactsModel();
            return View(model);
        }

        // POST: ContactsController/Import
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Import([FromForm] ImportContactsModel model)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync();
                await _webAPI.ImportContactsAsync(token, model.FileContacts);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: ContactsController/Details/5
        public async Task<ActionResult> Details(Guid id)
        {
            var token = await HttpContext.GetTokenAsync();
            var contact = await _webAPI.GetContactAsync(token, id);
            if (contact == null)
                return RedirectToAction(nameof(Index));
            else
                return View(contact);
        }

        // GET: ContactsController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(ContactModel contact)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync();
                await _webAPI.CreateContactAsync(token, contact);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: ContactsController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(Guid id)
        {
            var token = await HttpContext.GetTokenAsync();
            var contact = await _webAPI.GetContactAsync(token, id);
            if (contact == null)
                return RedirectToAction(nameof(Index));
            else
                return View(contact);
        }

        // POST: ContactsController/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Guid id, ContactModel contact)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync();
                await _webAPI.UpdateContactAsync(token, contact);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: ContactsController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(Guid id)
        {
            var token = await HttpContext.GetTokenAsync();
            var contact = await _webAPI.GetContactAsync(token, id);
            if (contact == null)
                return RedirectToAction(nameof(Index));
            else
                return View(contact);
        }

        // POST: ContactsController/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id, IFormCollection collection)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync();
                await _webAPI.DeleteContactAsync(token, id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: ContactsController/Clear
        [Authorize]
        public ActionResult Clear()
        {
            return View();
        }

        // POST: ContactsController/Clear
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Clear(IFormCollection collection)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync();
                await _webAPI.ClearContactsAsync(token);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // GET: ContactsController/Generate
        [Authorize]
        public ActionResult Generate()
        {
            return View();
        }

        // POST: ContactsController/Generate
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Generate(GenerateContactsModel model)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync();
                await _webAPI.GenerateContactsAsync(token, model.Clear);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

    }
}
