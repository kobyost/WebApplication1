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
using System.Xml.Linq;
using System.IO;
using System.Globalization;

namespace WebApplication1.Controllers
{
    public class BranchesController : Controller
    {
        private StoreDatabase db = new StoreDatabase();

        // GET: Branches
        public ActionResult Index(string City, string ZipCode, string Email,string PhoneNumber)
        {

            var branches = from m in db.Branches select m;

            if (!String.IsNullOrEmpty(City))
            {
                branches = branches.Where(s => s.City.Contains(City));
            }
            if (!String.IsNullOrEmpty(ZipCode))
            {
                branches = branches.Where(k => k.ZipCode == ZipCode);
            }

            if (!String.IsNullOrEmpty(Email))
            {
                branches = branches.Where(r => r.Email == Email);
            }
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                branches = branches.Where(r => r.PhoneNumber == PhoneNumber);
            }

            return View(branches);


        }

        // GET: Branches/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // GET: Branches/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Branches/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BranchID,ZipCode,City,Address,PhoneNumber,Email,Latitude,Longitude")] Branch branch)
        {
            string LatitudeLongitude= GeocoderLocation(branch.City+','+branch.Address+','+branch.ZipCode);
            var commaPos = LatitudeLongitude.IndexOf(',');
            branch.Latitude = Double.Parse(LatitudeLongitude.Substring(0, commaPos));
            branch.Longitude = Double.Parse(LatitudeLongitude.Substring(commaPos + 1));

            if (ModelState.IsValid)
             {
                db.Branches.Add(branch);
                db.SaveChanges();
                return RedirectToAction("Index");
              }

            return View(branch);
        }

        // GET: Branches/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BranchID,ZipCode,City,Address,PhoneNumber,Email,Latitude,Longitude")] Branch branch)
        {
            string LatitudeLongitude = GeocoderLocation(branch.City + ',' + branch.Address + ',' + branch.ZipCode);
            var commaPos = LatitudeLongitude.IndexOf(',');
            branch.Latitude = Double.Parse(LatitudeLongitude.Substring(0, commaPos));
            branch.Longitude = Double.Parse(LatitudeLongitude.Substring(commaPos + 1));


            if (ModelState.IsValid)
            {
                db.Entry(branch).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(branch);
        }

        // GET: Branches/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = db.Branches.Find(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            return View(branch);
        }

        // POST: Branches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Branch branch = db.Branches.Find(id);
            db.Branches.Remove(branch);
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

         public string GeocoderLocation(string query)
        {
            double[] array = new double[2];
            WebRequest request = WebRequest
               .Create("http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address="
                  + HttpUtility.UrlEncode(query));

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XDocument document = XDocument.Load(new StreamReader(stream));

                    XElement longitudeElement = document.Descendants("lng").FirstOrDefault();
                    XElement latitudeElement = document.Descendants("lat").FirstOrDefault();

                    if (longitudeElement != null && latitudeElement != null)
                    {
                        return String.Format("{0},{1}", Double.Parse(latitudeElement.Value, CultureInfo.InvariantCulture), Double.Parse(longitudeElement.Value, CultureInfo.InvariantCulture));
                    }
                }
            }
            return null;
        }

    }
   }

