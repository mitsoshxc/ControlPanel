using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace VPCustInfo.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly Models.VPCustomersContext CustomersContext;

        public PaymentsController(Models.VPCustomersContext _context)
        {
            CustomersContext = _context;
        }

        public async Task<IActionResult> Customer(int id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                try
                {
                    var _customerPay = await (from t0 in CustomersContext.Payment
                                              where t0.CustomerId == id
                                              select t0).ToListAsync();

                    var _customer = await (from t0 in CustomersContext.Customer
                                           where t0.id == id
                                           select t0).FirstAsync();

                    ViewData["Title"] = _customer.Name + "'s Payment Details";
                    ViewData["Name"] = _customer.Name;
                    
                    if (_customer.Website.Length > 0)
                    {
                        ViewData["Website"] = _customer.Website;
                    }

                    return View(_customerPay);
                }
                catch (Exception ex)
                {
                    TempData["ActionError"] = ex.Message;

                    return RedirectToAction("Customers", "Home");
                }
            }
            else
            {
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Add(int id)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                try
                {
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
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}