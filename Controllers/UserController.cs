using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace VPCustInfo.Controllers
{
    public class UserController : Controller
    {
        private readonly Models.VPCustomersContext CustomersContext;

        public UserController(Models.VPCustomersContext _context)
        {
            CustomersContext = _context;
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var _user = HttpContext.Session.GetSession<string>("User");

            if (_user != null)
            {
                return View(await (from _us in CustomersContext.User
                                   where _us.Name == _user
                                   select _us).FirstOrDefaultAsync()
                            );
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int _id, string _pass)
        {
            var _user = await (from us in CustomersContext.User
                               where us.id == _id
                               select us).FirstOrDefaultAsync();

            _user.Pass = _pass.Encrypt();

            await CustomersContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Add()
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                return View();
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(string _name, string _pass)
        {
            try
            {
                await CustomersContext.User.AddAsync(new Models.Users()
                {
                    Name = _name,
                    Pass = _pass.Encrypt()
                });

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "User " + _name + " created successfully";

                return RedirectToAction("Customers", "Home");
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error creating user " + _name +
                                            Environment.NewLine + ex.Message;
                
                return RedirectToAction("Customers", "Home");
            }
        }
    }
}