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
    public class CustomersController : Controller
    {
        private StoreDatabase db = new StoreDatabase();

        // GET: Customers
        public ActionResult Index(string FirstName, string LastName, string City)
        {
 
            var customers = from m in db.Customers select m;

            if (!String.IsNullOrEmpty(FirstName))
            {
                customers = customers.Where(s => s.FirstName == FirstName);
            }
            if (!String.IsNullOrEmpty(LastName))
            {
                customers = customers.Where(k => k.LastName == LastName);            
            }

            if (!String.IsNullOrEmpty(City))
            {
                customers = customers.Where(r => r.City == City);
            }

            return View(customers);


        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            var customers = from m in db.Customers select m;
            foreach(var customer in customers)
                if (customer.UserName.Equals(User.Identity.Name))
                    return RedirectToAction("Index", "Home");
            
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerID,UserName,FirstName,LastName,PhoneNumber,City,Address,ZipCode,BirthDate")] Customer customer)
        {          
            if (!User.Identity.Name.Equals("Admin@gmail.com")) 
                customer.UserName = User.Identity.Name;

            if(customer.UserName==null)
            {
                ViewBag.NoUserName="Hey admin you must enter a user name";
                return View(customer);
            }
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(customer);
        }


        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerID,UserName,FirstName,LastName,PhoneNumber,City,Address,ZipCode,BirthDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
     

    }
}
