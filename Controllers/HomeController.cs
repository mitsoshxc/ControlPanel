﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;


namespace VPCustInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly Models.VPCustomersContext CustomersContext;

        public HomeController(Models.VPCustomersContext _context)
        {
            CustomersContext = _context;
        }

        public IActionResult Index(string _failAction = "", string _exception = "")
        {
            if (_failAction == "SessionExpired")
            {
                ViewData["SessionExpired"] = "true";
            }
            else if (_failAction.Length > 0)
            {
                ViewData["LogFail"] = _failAction;
            }
            
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
        public async Task<IActionResult> Validate(string _name, string _pass)
        {
            var _uId = await (from n in CustomersContext.User
                              where n.Name == _name && n.Pass == _pass.Encrypt()
                              select n.id).FirstOrDefaultAsync();
            if (_uId > 0)
            {
                HttpContext.Session.SetSession<string>("User", _name);
                HttpContext.Session.SetSession<int>("UserId", _uId);

                return RedirectToAction("Customers");
            }
            else
            {
                return RedirectToAction("Index", new { _failAction = _name });
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
                return RedirectToAction("Index", new { _failAction = "SessionExpired" });
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