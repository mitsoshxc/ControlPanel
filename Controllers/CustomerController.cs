using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ControlPanel.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Models.VPCustomersContext CustomersContext;

        public CustomerController(Models.VPCustomersContext _context)
        {
            CustomersContext = _context;
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                if (HttpContext.Session.GetSession<int>("Rank") < 2)
                {
                    try
                    {
                        return View(await (from us in CustomersContext.Customer
                                           where us.id == id
                                           select us).FirstAsync());
                    }
                    catch (Exception ex)
                    {
                        TempData["ActionError"] = ex.Message;

                        return RedirectToAction("Customers", "Home");
                    }
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
        public async Task<IActionResult> Edit(int _id, string _name, string _website, string _phone1,
            string _phone2, string _address, string _email)
        {
            string _oldname = "";
            try
            {
                var _customer = await (from cust in CustomersContext.Customer
                                       where cust.id == _id
                                       select cust).FirstAsync();

                _oldname = _customer.Name;

                _customer.Name = _name;
                _customer.Website = _website;
                _customer.Phone1 = _phone1;
                _customer.Phone2 = _phone2;
                _customer.Address = _address;
                _customer.Email = _email;

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully edited customer " + _name;

                return RedirectToAction("Customers", "Home");
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error editing customer " + _oldname + " .   " + ex.Message;

                return RedirectToAction("Customers", "Home");
            }

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                if (HttpContext.Session.GetSession<int>("Rank") < 2)
                {
                    try
                    {
                        return View(await (from cust in CustomersContext.Customer
                                           where cust.id == id
                                           select cust).FirstAsync());
                    }
                    catch (Exception ex)
                    {
                        TempData["ActionError"] = ex.Message;

                        return RedirectToAction("Customers", "Home");
                    }
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
                //
                // Remove customer's details first
                //
                var _custDetails = from t0 in CustomersContext.CustomerDetails
                                   where t0.CustomerId == _id
                                   select t0;

                foreach (var _element in _custDetails)
                {
                    CustomersContext.CustomerDetails.Remove(_element);
                }

                await CustomersContext.SaveChangesAsync();
                //
                // Remove customer's payments details now
                //
                var _custPayment = from t0 in CustomersContext.Payment
                                   where t0.CustomerId == _id
                                   select t0;

                foreach (var _element in _custPayment)
                {
                    CustomersContext.Payment.Remove(_element);
                }

                await CustomersContext.SaveChangesAsync();
                //
                // Remove customer last
                //
                CustomersContext.Customer.Remove(await (from cust in CustomersContext.Customer
                                                        where cust.id == _id
                                                        select cust).FirstAsync()
                                                );

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully removed customer " + _name;

                return RedirectToAction("Customers", "Home");
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error removing customer " + _name + " .   " + ex.Message;

                return RedirectToAction("Customers", "Home");
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
        public async Task<IActionResult> Add(string _name, string _website, string _phone1,
            string _phone2, string _address, string _email)
        {
            try
            {
                await CustomersContext.Customer.AddAsync(new Models.Customers()
                {
                    Name = _name,
                    Website = _website,
                    Phone1 = _phone1,
                    Phone2 = _phone2,
                    Address = _address,
                    Email = _email
                });

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully added customer " + _name;

                return RedirectToAction("Customers", "Home");
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error adding customer " + _name + " .   " + ex.Message;

                return RedirectToAction("Customers", "Home");
            }
        }
    }
}