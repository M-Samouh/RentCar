using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models;

namespace RentCar.Controllers
{
    [Area("Proprietaire")]
    public class ProfileController : Controller
    {
        private ApplicationDbContext _db;
        public ProfileController(ApplicationDbContext db) 
        {
            _db = db;
        }

        public IActionResult listVoiture2(string id)
        {
            var prod = _db.ProductUsers.Include(c => c.SpecialTag).Include(c => c.ProductTypes).Where(c => c.UserId == id).ToList();
            return View(prod);
        }

        /*
                public IActionResult Index(int id)
                {
                    var pro = _db.Proprietaires.Find(id);
                    return View(pro);
                }

                public IActionResult listVoiture(int id)
                {
                    ViewBag.nameProp = _db.Proprietaires.Find(id).Name;
                    var prod = _db.Products.Include(c => c.SpecialTag).Include(c => c.ProductTypes).Include(c => c.Proprietaire).Where(c=>c.ProprietaireId==id).ToList();
                    return View(prod);
                }

            */

    }
}