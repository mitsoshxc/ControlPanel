using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VPCustInfo.Controllers
{
    public class CustomerDetailsController : Controller
    {
        private readonly Models.VPCustomersContext CustomersContext;

        public CustomerDetailsController(Models.VPCustomersContext _context)
        {
            CustomersContext = _context;
        }

        public async Task<IActionResult> Customer(int id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                try
                {
                    var _customerDetails = await (from t0 in CustomersContext.CustomerDetails
                                                  where t0.CustomerId == id
                                                  select t0).ToListAsync();

                    var _customer = await (from t0 in CustomersContext.Customer
                                           where t0.id == id
                                           select t0).FirstAsync();

                    ViewData["Title"] = _customer.Name + "'s Details";
                    ViewData["Name"] = _customer.Name;
                    ViewData["CustId"] = _customer.id;
                    ViewData["Website"] = _customer.Website;

                    return View(_customerDetails);
                }
                catch (Exception ex)
                {
                    TempData["ActionError"] = ex.Message;

                    return RedirectToAction("Customers", "Home");
                }
            }
            else
            {
                return RedirectToAction("Customer", "Home", new { _failAction = "SessionExpired" });
            }
        }

        public async Task<IActionResult> Add(int id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                try
                {
                    await (from t0 in CustomersContext.Customer
                           where t0.id == id
                           select t0).FirstAsync();

                    return View(id);
                }
                catch (Exception ex)
                {
                    TempData["ActionError"] = ex.Message;

                    return RedirectToAction("Customer", new { id = id });
                }
            }
            else
            {
                return RedirectToAction("Customer", "Home", new { _failAction = "SessionExpired" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(int _custId, string _sqlUser = "", string _sqlPass = "",
            string _wpUser = "", string _wpPass = "")
        {
            try
            {
                await CustomersContext.CustomerDetails.AddAsync(new Models.CustomersDetails
                {
                    CustomerId = _custId,
                    SqlUser = _sqlUser.Encrypt(),
                    SqlPass = _sqlPass.Encrypt(),
                    WpUser = _wpUser.Encrypt(),
                    WpPass = _wpPass.Encrypt()
                });

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully added customer's details.";

                return RedirectToAction("Customer", new { id = _custId });
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error adding customer's details.   " + ex.InnerException;

                return RedirectToAction("Customer", new { id = _custId });
            }
        }

        public async Task<IActionResult> Edit(int id, int LineNo)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                try
                {
                    ViewData["Name"] = await (from t0 in CustomersContext.Customer
                                              where t0.id == id
                                              select t0.Name).FirstAsync();

                    return View(await (from t0 in CustomersContext.CustomerDetails
                                       where t0.id == LineNo
                                       select t0).FirstAsync());
                }
                catch (Exception ex)
                {
                    TempData["ActionError"] = ex.Message;

                    return RedirectToAction("Customer", new { id = id });
                }
            }
            else
            {
                return RedirectToAction("Customer", "Home", new { _failAction = "SessionExpired" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int _id, int _custId, string _sqlUser, string _sqlPass,
            string _wpUser, string _wpPass)
        {
            try
            {
                var _custDetailLine = await (from t0 in CustomersContext.CustomerDetails
                                             where t0.id == _id
                                             select t0).FirstAsync();

                _custDetailLine.SqlUser = _sqlUser.Encrypt();
                _custDetailLine.SqlPass = _sqlPass.Encrypt();
                _custDetailLine.WpUser = _wpUser.Encrypt();
                _custDetailLine.WpPass = _wpPass.Encrypt();

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully edited customer's details.";

                return RedirectToAction("Customer", new { id = _custId });
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error adding customer's details.   " + ex.InnerException;

                return RedirectToAction("Customer", new { id = _custId });
            }
        }

        public async Task<IActionResult> Delete(int id, int LineNo)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                try
                {
                    await (from t0 in CustomersContext.CustomerDetails
                           where t0.id == LineNo
                           select t0).FirstAsync();

                    ViewData["id"] = id;

                    return View(LineNo);
                }
                catch (Exception ex)
                {
                    TempData["ActionError"] = ex.Message;

                    return RedirectToAction("Customer", new { id = id });
                }
            }
            else
            {
                return RedirectToAction("Customer", "Home", new { _failAction = "SessionExpired" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? _id, int _custId)
        {
            try
            {
                CustomersContext.CustomerDetails.Remove(
                    await (from t0 in CustomersContext.CustomerDetails
                           where t0.id == _id
                           select t0).FirstAsync()
                );

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully removed customer's details.";

                return RedirectToAction("Customer", new { id = _custId });
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error removing customer's details.   " + ex.InnerException;

                return RedirectToAction("Customer", new { id = _custId });
            }
        }
    }
}