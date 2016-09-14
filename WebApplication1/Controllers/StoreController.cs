using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class StoreController : Controller
    {
        private StoreDatabase db = new StoreDatabase();
        public List<GameTitle> Games=new List<GameTitle>();
        // GET: Store
        public ActionResult PC()
        {           
            foreach (var game in db.Games)
                if (game.Platform.Equals("PC"))
                    Games.Add(game);

            return View(Games);
        }

        public ActionResult PS4()
        {
            foreach (var game in db.Games)
                if (game.Platform.Equals("PS4"))
                    Games.Add(game);

            return View(Games);
        }

        public ActionResult XboxOne()
        {
            foreach (var game in db.Games)
                if (game.Platform.Equals("XboxOne"))
                    Games.Add(game);

            return View(Games);
        }

        public ActionResult WiiU()
        {
            foreach (var game in db.Games)
                if (game.Platform.Equals("WiiU"))
                    Games.Add(game);

            return View(Games);
        }

        public ActionResult Details(int id)
        {
            var game = db.Games.Find(id);

            return View(game);
        }

    }
}