using BowlingLeague.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingLeague.Controllers
{
    public class HomeController : Controller
    {
        private IBowlersRepository _repo { get; set; }
        public HomeController(IBowlersRepository temp)
        {
            _repo = temp;
        }

        //Default id = 0 to load all teams by default
        public IActionResult Index(int id = 0)
        {
            //If the id is 0, returns the full list
            if (id == 0)
            {
                var bowlers = _repo.Bowlers.OrderBy(x => x.BowlerLastName).ToList();
                ViewBag.Teams = _repo.Teams.ToList();
                ViewBag.CurrentTeamID = id;
                return View(bowlers);
            }
            //Returns bowlers from specific teams
            else
            {
                var bowlers = _repo.Bowlers.Include(x => x.Team).Where(x => x.TeamID == id).ToList();
                ViewBag.Teams = _repo.Teams.ToList();
                ViewBag.CurrentTeamID = id;
                return View(bowlers);
            }

        }

        [HttpGet]
        public IActionResult CreateBowler()
        {
            ViewBag.Teams = _repo.Teams.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult CreateBowler(Bowler b)
        {
            if (ModelState.IsValid)
            {
                _repo.CreateBowler(b);
                _repo.SaveBowler(b);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Teams = _repo.Teams.ToList();
                return View(b);
            }
        }

        [HttpGet]
        public IActionResult EditBowler()
        {
            //This will take the user back to the createbowler page and preload it with info about the bowler they selected
            int id = Convert.ToInt32(RouteData.Values["id"]);
            var bowler = _repo.Bowlers.Single(x => x.BowlerId == id);
            ViewBag.Teams = _repo.Teams.ToList();
            return View("CreateBowler", bowler);
        }

        [HttpPost]
        public IActionResult EditBowler(Bowler b)
        {
            //Data validation for editing a bowler
            if (ModelState.IsValid)
            {
                _repo.UpdateBowler(b);

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Teams = _repo.Teams.ToList();
                return View("CreateBowler", b);
            }

        }

        [HttpGet]
        public IActionResult DeleteBowler()
        {
            //This will take the user to the deletebowler page to confirm that they want to delete this record
            int id = Convert.ToInt32(RouteData.Values["id"]);
            var bowler = _repo.Bowlers.Single(x => x.BowlerId == id);

            return View(bowler);
        }

        [HttpPost]
        public IActionResult DeleteBowler(Bowler b)
        {
            _repo.DeleteBowler(b);
            _repo.SaveBowler(b);

            return RedirectToAction("Index");
        }
    }
}
