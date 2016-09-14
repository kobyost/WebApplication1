using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Migrations;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private StoreDatabase db = new StoreDatabase();
        public ActionResult Index(string GameGenre, string title, string GamePrice, string Developer, string Platform)
        {
            var GenreList = new List<string>();
            var DeveloperList = new List<string>();
            var PlatformList = new List<string>();
            var DeveQry = from d in db.Games orderby d.DevelopedBy select d.DevelopedBy;
            var GenreQry = from d in db.Games orderby d.Genre select d.Genre;
            var PlatQry = from d in db.Games orderby d.Platform select d.Platform;
            GenreList.AddRange(GenreQry.Distinct());
            DeveloperList.AddRange(DeveQry.Distinct());
            PlatformList.AddRange(PlatQry.Distinct());
            ViewBag.GameGenre = new SelectList(GenreList);
            ViewBag.Developer = new SelectList(DeveloperList);
            ViewBag.Platform = new SelectList(PlatformList);
            var PriceList = new List<string>();
            PriceList.Add("0-100₪");
            PriceList.Add("100₪-200₪");
            PriceList.Add("200₪-300₪");
            ViewBag.GamePrice = new SelectList(PriceList);


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

            if (!String.IsNullOrEmpty(Developer))
            {
                games = games.Where(a => a.DevelopedBy == Developer);
            }
            if (!String.IsNullOrEmpty(Platform))
            {
                games = games.Where(b => b.Platform == Platform);
            }
            return View(games);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {

            var Branches = db.Branches.ToList();

            return View(Branches);
        }

        public ActionResult Events()
        {

            return View();
        }

         public ActionResult Chart()
        {

            return View();
        }
        public ActionResult ManagerChart()
        {

            return View();
        }

        public class result
        {
            public string State { get; set; }
            public int freq { get; set; }
        }
        //most popular genre
        public JsonResult PupularGenreGraph()
        {
            var QueryRes = from x in db.Games
                           group x by x.Genre into tab
                           select new
                                   {
                                       tab.Key,
                                       amount = tab.Count()
                                   };
            List<result> GenreAmont = new List<result>();

            foreach (var y in QueryRes)
            {
                result a = new result();
                a.State = y.Key;
                a.freq = y.amount;
                GenreAmont.Add(a);
            }

            return Json(GenreAmont, JsonRequestBehavior.AllowGet);
        }
        //amount of game for each platform
        public JsonResult GamePlatformGraph()
        {

            var QueryRes1 = from x in db.Games
                            group x by x.Platform into tab
                            select new
                                    {
                                        tab.Key,
                                        amount = tab.Count()
                                    };
            List<result> PlatAmont = new List<result>();

            foreach (var y in QueryRes1)
            {
                result a = new result();
                a.State = y.Key;
                a.freq = y.amount;
                PlatAmont.Add(a);
            }
            return Json(PlatAmont, JsonRequestBehavior.AllowGet);
        }
        // All the  customers that bought a game and  from each genre
        public JsonResult CustomersGameGenreGraph()
        {
            var QueryRes2 = from x in db.Customers
                            join y in db.Orders on x.CustomerID equals y.CustomerID
                            join d in db.Games on y.GameTitleID equals d.GameTitleID
                            group d by d.Genre into gen
                            select new
                                    {
                                        gen.Key,
                                        amount = gen.Count()
                                    };
            List<result> PlatAmont = new List<result>();

            foreach (var y in QueryRes2)
            {
                result a = new result();
                a.State = y.Key;
                a.freq = y.amount;
                PlatAmont.Add(a);
            }
            return Json(PlatAmont, JsonRequestBehavior.AllowGet);

        }
        //All the  customers that bought a game and  from each platform
        public JsonResult CustomersGamePlatformGraph()
        {
            var QueryRes2 = from x in db.Customers
                            join y in db.Orders on x.CustomerID equals y.CustomerID
                            join d in db.Games on y.GameTitleID equals d.GameTitleID
                            group y by d.Platform into gen
                            
                            select new
                            {
                                gen.Key,
                                amount = gen.Count()
                            };
            List<result> PlatAmont = new List<result>();

            foreach (var y in QueryRes2)
            {
                result a = new result();
                a.State = y.Key;
                a.freq = y.amount;
                PlatAmont.Add(a);
            }
            return Json(PlatAmont, JsonRequestBehavior.AllowGet);

        }
    }
}