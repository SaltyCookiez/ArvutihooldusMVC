using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArvutihooldusMVC.Data;
using ArvutihooldusMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArvutihooldusMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize]
        public async Task<IActionResult> Index()
        {
              return View(await _context.Orders.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ComputerType,Service,Price,Client,Complete,Paid")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ComputerType,Service,Price,Client,Complete,Paid")] Order order)
        {
            if (id != order.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.ID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        private bool OrderExists(int id)
        {
          return _context.Orders.Any(e => e.ID == id);
        }

        public IActionResult Order()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Order([Bind("ID,ComputerType,Service,Client")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(new Order() 
                {
                    ComputerType = order.ComputerType,
                    Service = order.Service,
                    Price = Math.Round(Models.Order.GetPrice(order.ComputerType, order.Service), 2),
                    Client = order.Client
                });
                await _context.SaveChangesAsync();
                return View("SubmittedOrder");
            }
            return View(order);
        }

        [Authorize]
        public async Task<IActionResult> UncompletedOrders()
        {
            return View(await _context.Orders.Where(x => x.Complete == false).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> CompleteOrderConfirmation(int id)
        {
            return View(await _context.Orders.FindAsync(id));
        }

        [Authorize]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Complete = true;
                _context.Orders.Update(order);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("UncompletedOrders");
        }

        [Authorize]
        public async Task<IActionResult> UnpaidOrders()
        {
            return View(await _context.Orders.Where(x => x.Paid == false).ToListAsync());
        }

        [Authorize]
        public async Task<IActionResult> MarkOrderPaidConfirmation(int id)
        {
            return View(await _context.Orders.FindAsync(id));
        }

        [Authorize]
        public async Task<IActionResult> MarkOrderPaid(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                order.Paid = true;
                _context.Orders.Update(order);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("UnpaidOrders");
        }

        [Authorize]
        public async Task<IActionResult> Clients()
        {
            List<ClientViewModel> clients = new();
            var orders = await _context.Orders.ToListAsync();
            foreach (var order in orders)
            {
                clients.Add(new ClientViewModel { Name = order.Client });
            }
            return View(clients.DistinctBy(x => x.Name).ToList());
        }

        [Authorize]
        public async Task<IActionResult> ClientOrders(string client)
        {
            return View(await _context.Orders.Where(x => x.Client == client).ToListAsync());
        }

        public double GetOrderPrice(ComputerType computerType, Service service)
        {
            return Math.Round(Models.Order.GetPrice(computerType, service), 2);
        }
    }
}
