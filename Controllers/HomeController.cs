using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;


namespace ControlPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly Models.VPCustomersContext CustomersContext;

        public HomeController(Models.VPCustomersContext _context)
        {
            CustomersContext = _context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetSession<string>("UserId") != null)
            {
                return RedirectToAction("Customers");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Validate(string _name, string _pass)
        {
            try
            {
                if (_name == null)
                {
                    TempData["NoUserName"] = "true";
                    return RedirectToAction("Index");
                }
                var _User = await (from n in CustomersContext.User
                                  where n.Name.Decrypt() == _name && n.Pass == _pass.Encrypt()
                                  select n).FirstOrDefaultAsync();
                                  
                if (_User != null)
                {
                    HttpContext.Session.SetSession<string>("User", _name);
                    HttpContext.Session.SetSession<int>("UserId", _User.id);
                    HttpContext.Session.SetSession<int>("Rank", _User.Rank);

                    return RedirectToAction("Customers");
                }
                else
                {
                    TempData["LogFail"] = _name;
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ValidateException"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Customers()
        {
            var _user = HttpContext.Session.GetSession<string>("User");

            if (_user != null)
            {
                ViewData["User"] = _user;

                return View(await CustomersContext.Customer.ToListAsync());
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index");
            }
        }

        public IActionResult ActionError(string _message)
        {
            ViewData["ActionError"] = _message;

            return RedirectToAction("Index");
        }

        public IActionResult ActionSuccess(string _message)
        {
            ViewData["ActionSuccess"] = _message;

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new Models.ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}