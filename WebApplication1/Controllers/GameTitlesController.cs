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
    public class GameTitlesController : Controller
    {
        private StoreDatabase db = new StoreDatabase();

        // GET: GameTitles
        public ActionResult Index(string GameGenre, string title, string GamePrice,string platform)
        {
            var GenreList = new List<string>();
            var GenreQry = from d in db.Games orderby d.Genre select d.Genre;
            GenreList.AddRange(GenreQry.Distinct());

            var PriceList = new List<string>();
            PriceList.Add("0-100₪");
            PriceList.Add("100₪-200₪");
            PriceList.Add("200₪-300₪");

            var PlatformList = new List<string>();
            var PlatformQry = from d in db.Games orderby d.Platform select d.Platform;
            PlatformList.AddRange(PlatformQry.Distinct());

            ViewBag.GamePrice = new SelectList(PriceList);
            ViewBag.GameGenre = new SelectList(GenreList);
            ViewBag.platform = new SelectList(PlatformList);

            var games = from m in db.Games select m;

            if (!String.IsNullOrEmpty(title))
            {
                games = games.Where(s => s.Name.Contains(title));
            }
            if (!String.IsNullOrEmpty(GamePrice))
            {
                if (GamePrice.Equals("0-100₪"))
                {
                    games = games.Where(k => k.Price <= 100);
                }
                if (GamePrice.Equals("100₪-200₪"))
                {
                    games = games.Where(k => k.Price >= 100 && k.Price <= 200);
                }
                if (GamePrice.Equals("200₪-300₪"))
                {
                    games = games.Where(k => k.Price >= 200 && k.Price <= 300);
                }
            }

            if (!String.IsNullOrEmpty(GameGenre))
            {
                games = games.Where(r => r.Genre == GameGenre);
            }

            if (!String.IsNullOrEmpty(platform))
            {
                games = games.Where(r => r.Platform == platform);
            }
            return View(games);
        }

        // GET: GameTitles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameTitle gameTitle = db.Games.Find(id);
            if (gameTitle == null)
            {
                return HttpNotFound();
            }
            return View(gameTitle);
        }

        // GET: GameTitles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GameTitles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GameTitleID,Name,DevelopedBy,Genre,Rating,ReleaseDate,Price,Description,Platform,ImageUrl,VideoUrl")] GameTitle gameTitle)
        {
            if (ModelState.IsValid)
            {
                db.Games.Add(gameTitle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gameTitle);
        }

        // GET: GameTitles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameTitle gameTitle = db.Games.Find(id);
            if (gameTitle == null)
            {
                return HttpNotFound();
            }
            return View(gameTitle);
        }

        // POST: GameTitles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameTitleID,Name,DevelopedBy,Genre,Rating,ReleaseDate,Price,Description,Platform,ImageUrl,VideoUrl")] GameTitle gameTitle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gameTitle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gameTitle);
        }

        // GET: GameTitles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GameTitle gameTitle = db.Games.Find(id);
            if (gameTitle == null)
            {
                return HttpNotFound();
            }
            return View(gameTitle);
        }

        // POST: GameTitles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GameTitle gameTitle = db.Games.Find(id);
            db.Games.Remove(gameTitle);
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
