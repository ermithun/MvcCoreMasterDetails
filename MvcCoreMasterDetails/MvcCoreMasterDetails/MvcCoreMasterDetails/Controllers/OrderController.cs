using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MvcCoreMasterDetails.Models.OrderMgmt.Models;
using MvcCoreMasterDetails.Data;
using MvcCoreMasterDetails.Models;
using Microsoft.EntityFrameworkCore;
using MvcCoreMasterDetails.Models.OrderMgmt.ViewModels;

namespace MvcCoreMasterDetails.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            AppResult result = null;
            IEnumerable<OrderMast> lst = null;
            try
            {
                lst = await _context.OrderMast.ToListAsync();
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }
            return PartialView(lst);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            AppResult result = new AppResult();
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderMastViewModel data)
        {
            AppResult result = new AppResult();
            try
            {
                if (ModelState.IsValid)
                {
                    OrderMast model = new OrderMast
                    {
                        CustomerName = data.CustomerName,
                        OrderDate = data.OrderDate,
                        OrderDetl = data.OrderDetlViewModel.Select(a => new OrderDetl
                        {
                            ProductName = a.ProductName,
                            Qty = a.Qty,
                            Rate = a.Rate
                        }).ToList()
                    };
                    _context.Add(model);
                    await _context.SaveChangesAsync();

                    result = new AppResult { ResultType = ResultType.Success, Message = "Successfully Added!" };
                    return Json(result);

                }
                else
                {
                    result = new AppResult { ResultType = ResultType.Failed, Message = string.Join(";", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            AppResult result = new AppResult();
            try
            {
                OrderMast model = await _context.OrderMast.Where(a => a.Id == id).Include(a => a.OrderDetl).FirstOrDefaultAsync();
                OrderMastViewModel modelvm = new OrderMastViewModel
                {
                    Id = model.Id,
                    CustomerName = model.CustomerName,
                    OrderDate = model.OrderDate,
                    OrderDetlViewModel = model.OrderDetl.Select(a => new OrderDetlViewModel
                    {
                        Id = a.Id,
                        MastId = a.MastId,
                        ProductName = a.ProductName,
                        Qty = a.Qty,
                        Rate = a.Rate
                    }).ToList()
                };
                return PartialView(modelvm);
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(OrderMastViewModel data)
        {
            AppResult result = new AppResult();
            try
            {
                if (ModelState.IsValid)
                {
                    OrderMast model = new OrderMast
                    {
                        Id = data.Id,
                        CustomerName = data.CustomerName,
                        OrderDate = data.OrderDate,
                        OrderDetl = data.OrderDetlViewModel.Where(a => a.Flag == Flag.New).Select(a => new OrderDetl
                        {
                            Id = a.Id,
                            MastId = a.MastId,
                            ProductName = a.ProductName,
                            Qty = a.Qty,
                            Rate = a.Rate
                        }).ToList()
                    };
                    List<OrderDetl> detl = data.OrderDetlViewModel.Where(a => a.Flag == Flag.Deleted).Select(c => new OrderDetl
                    {
                        Id = c.Id,
                        MastId = c.MastId,
                        ProductName = c.ProductName,
                        Qty = c.Qty,
                        Rate = c.Rate
                    }).ToList();
                    _context.RemoveRange(detl);
                    await _context.SaveChangesAsync();
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    result = new AppResult { ResultType = ResultType.Success, Message = "Successfully Updated !!" };
                    return Json(result);
                }
                else
                {
                    result = new AppResult { ResultType = ResultType.Failed, Message = string.Join(";", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)) };
                    return Json(result);
                }
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            AppResult result = new AppResult();
            OrderMastViewModel modelvm = null;
            try
            {
                OrderMast model = await _context.OrderMast.Where(a => a.Id == id).Include(a => a.OrderDetl).FirstOrDefaultAsync();
                modelvm = new OrderMastViewModel
                {
                    Id = model.Id,
                    CustomerName = model.CustomerName,
                    OrderDate = model.OrderDate,
                    OrderDetlViewModel = model.OrderDetl.Select(a => new OrderDetlViewModel
                    {
                        Id = a.Id,
                        MastId = a.MastId,
                        ProductName = a.ProductName,
                        Qty = a.Qty,
                        Rate = a.Rate
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }
            return PartialView(modelvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int Id)
        {
            AppResult result = new AppResult();
            try
            {
                var model = await _context.OrderMast.Where(a => a.Id == Id).FirstOrDefaultAsync();
                _context.Remove(model);
                await _context.SaveChangesAsync();
                result = new AppResult { ResultType = ResultType.Success, Message = "Successfully Deleted." };
                return Json(result);
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            AppResult result = new AppResult();
            OrderMastViewModel modelvm = null;
            try
            {
                OrderMast model = await _context.OrderMast.Where(a => a.Id == id).Include(a => a.OrderDetl).FirstOrDefaultAsync();
                modelvm = new OrderMastViewModel
                {
                    Id = model.Id,
                    CustomerName = model.CustomerName,
                    OrderDate = model.OrderDate,
                    OrderDetlViewModel = model.OrderDetl.Select(a => new OrderDetlViewModel
                    {
                        Id = a.Id,
                        MastId = a.MastId,
                        ProductName = a.ProductName,
                        Qty = a.Qty,
                        Rate = a.Rate
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                result = new AppResult { ResultType = ResultType.Failed, Message = "Exception occur with the system. please contact to vendor." };
                return Json(result);
            }
            return PartialView("Details", modelvm);
        }


    }
}
