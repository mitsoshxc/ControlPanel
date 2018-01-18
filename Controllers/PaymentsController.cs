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
                    ViewData["CustId"] = _customer.id;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int _custId, string _type, double _amount)
        {
            try
            {
                var _LastLine = await (from t0 in CustomersContext.CustomerDetails
                                       where t0.CustomerId == _custId
                                       select t0.LineNo).LastOrDefaultAsync();
                
                await CustomersContext.Payment.AddAsync(new Models.Payments
                {
                    CustomerId = _custId,
                    LineNo = _LastLine + 1,
                    Type = _type.Encrypt(),
                    Amount = _amount
                });

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully added customer's payments details.";

                return RedirectToAction("Customer", new { id = _custId });
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error adding customer's payments details.   " + ex.InnerException;

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

                    return View(await (from t0 in CustomersContext.Payment
                                       where t0.CustomerId == id && t0.LineNo == LineNo
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
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int _id, int _custId, string _type = "", double _amount = 0)
        {
            try
            {
                var _custDetailLine = await (from t0 in CustomersContext.Payment
                                             where t0.id == _id
                                             select t0).FirstAsync();

                _custDetailLine.Type = _type.Encrypt();
                _custDetailLine.Amount = _amount;

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully edited customer's payments details.";

                return RedirectToAction("Customer", new { id = _custId });
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error editing customer's payments details.   " + ex.InnerException;

                return RedirectToAction("Customer", new { id = _custId });
            }
        }

        public async Task<IActionResult> Delete(int id, int LineNo)
        {
            if (HttpContext.Session.GetSession<string>("User") != null)
            {
                try
                {
                    await (from t0 in CustomersContext.Payment
                           where t0.CustomerId == id && t0.LineNo == LineNo
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
                TempData["SessionExpired"] = true;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int _custId, int? _lineNo)
        {
            try
            {
                CustomersContext.Payment.Remove(
                    await (from t0 in CustomersContext.Payment
                           where t0.CustomerId == _custId && t0.LineNo == _lineNo
                           select t0).FirstAsync()
                );

                await CustomersContext.SaveChangesAsync();

                var _custPaymentsDetails = await (from t0 in CustomersContext.Payment
                                          where t0.CustomerId == _custId && t0.LineNo > _lineNo
                                          select t0).ToListAsync();

                foreach (var _element in _custPaymentsDetails)
                {
                    _element.LineNo -= 1;
                }

                await CustomersContext.SaveChangesAsync();

                TempData["ActionSuccess"] = "Successfully removed customer's payment details.";

                return RedirectToAction("Customer", new { id = _custId });
            }
            catch (Exception ex)
            {
                TempData["ActionError"] = "Error removing customer's payment details.   " + ex.InnerException;

                return RedirectToAction("Customer", new { id = _custId });
            }
        }
    }
}