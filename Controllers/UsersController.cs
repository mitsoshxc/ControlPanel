using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ControlPanel.Controllers
{
    public class UsersController : Controller
    {
        private readonly Models.VPCustomersContext CustomersContext;

        public UsersController(Models.VPCustomersContext _context)
        {
            CustomersContext = _context;
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                return View(await CustomersContext.User.ToListAsync());
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                if (HttpContext.Session.GetSession<int>("Rank") < 2)
                {
                    return View(await (from _us in CustomersContext.User
                                       where _us.id == id
                                       select _us).FirstOrDefaultAsync()
                                );
                }
                else
                {
                    TempData["Unprivileged"] = HttpContext.Session.GetSession<int>("Rank");
                    return RedirectToAction("Customers", "Home");
                }
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int _id, string _name, string _pass, int _rank)
        {
            try
            {
                var _user = await (from us in CustomersContext.User
                                   where us.id == _id
                                   select us).FirstOrDefaultAsync();

                if (_pass != null)
                {
                    _user.Pass = _pass.Encrypt();
                }

                if (_rank != 0)
                {
                    _user.Rank = _rank;
                }

                await CustomersContext.SaveChangesAsync();

                var _Rank = await (from t0 in CustomersContext.User
                                   where t0.id == HttpContext.Session.GetSession<int>("UserId")
                                   select t0.Rank).FirstOrDefaultAsync();

                HttpContext.Session.SetSession<int>("Rank", _Rank);

                TempData["ActionSuccess"] = "Successfully edited user " + _user.Name.Decrypt();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error editing user " + _name + ". " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        public IActionResult Add()
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                if (HttpContext.Session.GetSession<int>("Rank") < 3)
                {
                    return View();
                }
                else
                {
                    TempData["Unprivileged"] = HttpContext.Session.GetSession<int>("Rank");
                    return RedirectToAction("Customers", "Home");
                }
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(string _name, string _pass, int _rank)
        {
            try
            {
                await CustomersContext.User.AddAsync(new Models.Users()
                {
                    Name = _name.Encrypt(),
                    Pass = _pass.Encrypt(),
                    Rank = _rank
                });

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "User " + _name + " created successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error creating user " + _name +
                                            Environment.NewLine + ex.Message;

                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                if (HttpContext.Session.GetSession<int>("Rank") < 2)
                {
                    return View(await (from t0 in CustomersContext.User
                                       where t0.id == id
                                       select t0).FirstAsync());
                }
                else
                {
                    TempData["Unprivileged"] = HttpContext.Session.GetSession<int>("Rank");
                    return RedirectToAction("Customers", "Home");
                }
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int _id, string _name)
        {
            try
            {
                CustomersContext.User.Remove(await (from t0 in CustomersContext.User
                                                    where t0.id == _id
                                                    select t0).FirstAsync());
                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "User " + _name + " removed successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error removing user " + _name +
                                            Environment.NewLine + ex.Message;

                return RedirectToAction("Index");
            }
        }
    }
}