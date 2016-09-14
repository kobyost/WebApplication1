using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class OrdersController : Controller
    {
        private StoreDatabase db = new StoreDatabase();

        // GET: Orders
        public ActionResult Index(DateTime? OrderDate, string UserName, string GameName)
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.GameTitle);
            if (!(String.IsNullOrEmpty(OrderDate.ToString())))
            {
                orders = orders.Where(x => DbFunctions.TruncateTime(x.OrderDate) == OrderDate.Value);

            }
            if (!(String.IsNullOrEmpty(UserName)))
            {
                orders = orders.Where(k => k.Customer.UserName.Contains(UserName));

            }
            if ((!String.IsNullOrEmpty(GameName)))
            {
                orders = orders.Where(m => m.GameTitle.Name.Contains(GameName));

            }

            return View(orders);
        }
        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.GameTitle);
            var Order=new List<Order>();
            Order=orders.ToList();
            Order order = Order.Find(item => item.OrderID == id);
               
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName");
            ViewBag.GameTitleID = new SelectList(db.Games, "GameTitleID", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,OrderDate,OrderPrice,GameTitleID,CustomerID")] Order order)
        {
            bool flag = true;
            if (order.CustomerID == 0)
            {
                ViewBag.CustomerIdError = "You cant make an order without specifying a customer id";
                flag = false;
               }
            if (order.GameTitleID == 0)
            {
                flag = false;
                ViewBag.TitleError = "You cant make an order without specifying a title";
            }

            if (ModelState.IsValid && flag)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.GameTitleID = new SelectList(db.Games, "GameTitleID", "Name", order.GameTitleID);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.GameTitleID = new SelectList(db.Games, "GameTitleID", "Name", order.GameTitleID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDate,OrderPrice,GameTitleID,CustomerID")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "UserName", order.CustomerID);
            ViewBag.GameTitleID = new SelectList(db.Games, "GameTitleID", "Name", order.GameTitleID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateOrderAndRecipt(int id)
        {
            var game = db.Games.Find(id);
            Order order = new Order();
            order.GameTitleID = game.GameTitleID;
            order.OrderPrice = game.Price;
            order.OrderDate = DateTime.Now;
            foreach (var customer in db.Customers)
                if (customer.UserName.Equals(User.Identity.Name))
                    order.CustomerID = customer.CustomerID;

            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
            }
            ViewBag.NameOfGame = game.Name;
            return View(order);
        }

        public ActionResult ShippingInfo(int id)
        {
            if (User.Identity.Name.Equals(""))
                return RedirectToAction("Login", "Account");
            Customer customerr = new Customer();
            foreach (var customer in db.Customers)
                if (customer.UserName.Equals(User.Identity.Name))
                {
                    customerr = customer;
                    ViewBag.GameTitleId = id;
                    return View(customerr);
                }
            
            return RedirectToAction("Create","Customers");
           
        }
    }
}
