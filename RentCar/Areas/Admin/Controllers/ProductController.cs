﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentCar.Data;
using RentCar.Models;

namespace RentCar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;
        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }

        public IActionResult Index2()
        {
            var d = _db.ProductUsers.Include(c => c.ProductTypes).Include(b => b.SpecialTag).ToList();

            return View(d);
        }

        [HttpPost]
        public IActionResult Index2(decimal? LowAmount, decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).Where(c => c.Price >= LowAmount && c.Price <= largeAmount).ToList();

            if (LowAmount == null || largeAmount == null)
            {
                products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            }

            return View(products);
        }

        public ActionResult Create2()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            ViewData["ProprietaireId"] = new SelectList(_db.applicationUsers.Where(c => c.droit == "Proprietaire").ToList(), "Id", "UserName");
            ViewData["UserId"] = new SelectList(_db.applicationUsers.Where(c => c.droit == "Proprietaire").ToList(), "Id", "UserName");

            return View();
        }
        ///create post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2(ProductUser product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.ProductUsers.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.messages = "This product  already exists";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                    ViewData["UserId"] = new SelectList(_db.applicationUsers.Where(c=>c.droit=="Proprietaire").ToList(), "Id", "UserName");

                    return View(product);
                }

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = Path.GetFileName(name);
                }

                if (image == null)
                {
                    product.Image = "NoImageFound.jpg";
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index2));
            }

            return View(product);
        }


        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            ViewData["UserId"] = new SelectList(_db.applicationUsers.Where(c => c.droit == "Proprietaire").ToList(), "Id", "UserName");
            if (id == null)
            {
                return NotFound();
            }


            var product = _db.ProductUsers.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductUser product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.ProductUsers.FirstOrDefault(c => c.Name == product.Name);
                /*   if (searchProduct != null)
                   {
                       ViewBag.message = "This product is already exist";
                       ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                       ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                       return View(product);
                   }*/
                
                                if (image != null)
                                {
                                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                                    product.Image = Path.GetFileName(name);
                                }

                                if (image == null)
                                {
                                    product.Image = "NoImageFound.jpg";
                                }
                                _db.Products.Update(product);
                                await _db.SaveChangesAsync();
                                TempData["save"] = "A car been update";
                                return RedirectToAction(nameof(Index2));
            }

             return View(product);

        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _db.ProductUsers.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // get product
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = _db.ProductUsers.Include(c => c.SpecialTag).Include(c => c.ProductTypes).Where(c => c.Id == id).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();

            }
            return View(product);
        }
        //Post Delete
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var Product = _db.ProductUsers.FirstOrDefault(c => c.Id == id);
            if (Product == null)
            {
                return NotFound();
            }
            _db.Products.Remove(Product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index2));
        }




        ////////////////////////////////////////////
        /*
        //Index 
        public IActionResult Index()
        {
            var d=_db.ProductUsers.Include(c=>c.ProductTypes).Include(b => b.SpecialTag).ToList();

            return View(_db.Products.Include(c => c.ProductTypes).Include(b => b.SpecialTag).ToList());
        }

        // post Index 

        [HttpPost]
        public IActionResult Index(decimal? LowAmount,decimal? largeAmount)
        {
            var products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).Where(c => c.Price >= LowAmount && c.Price <= largeAmount).ToList();

            if(LowAmount==null || largeAmount==null)
            {
               products = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            }

            return View(products);
        }



        ////    Create get action
        public ActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            ViewData["ProprietaireId"] = new SelectList(_db.applicationUsers.Where(c=>c.droit=="Proprietaire").ToList(), "Id", "UserName");
            return View();
        }
        ///create post action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                if (searchProduct != null)
                {
                    ViewBag.messages = "This product  already exists";
                    ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                    ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                    ViewData["ProprietaireId"] = new SelectList(_db.Proprietaires.ToList(), "Id", "Name");

                    return View(product);
                }

                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = Path.GetFileName(name);
                }

                if (image == null)
                {
                    product.Image = "NoImageFound.jpg";
                }
                _db.Products.Add(product);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        //Get Edit Action Method

        public ActionResult Edit(int? id)
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
            ViewData["ProprietaireId"] = new SelectList(_db.Proprietaires.ToList(), "Id", "Name");

            if (id == null)
            {
                return NotFound();
            }


            var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Post Edit action
        [HttpPost]
        public async Task<IActionResult> Edit(Products product, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                var searchProduct = _db.Products.FirstOrDefault(c => c.Name == product.Name);
                /*   if (searchProduct != null)
                   {
                       ViewBag.message = "This product is already exist";
                       ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
                       ViewData["TagId"] = new SelectList(_db.SpecialTags.ToList(), "Id", "Name");
                       return View(product);
                   }*/
        /*
                        if (image != null)
                        {
                            var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                            await image.CopyToAsync(new FileStream(name, FileMode.Create));
                            product.Image = Path.GetFileName(name);
                        }

                        if (image == null)
                        {
                            product.Image = "NoImageFound.jpg";
                        }
                        _db.Products.Update(product);
                        await _db.SaveChangesAsync();
                        TempData["save"] = "A car been update";
                        return RedirectToAction(nameof(Index));
                    }

                    return View(product);

                }

                //get 
                public ActionResult Details(int? id)
                {
                    if (id == null)
                    {
                        return NotFound();
                    }
                    var product = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);

                    if (product == null)
                    {
                        return NotFound();
                    }
                    return View(product);
                }

                // get product
                public ActionResult Delete(int? id)
                {

                    if (id == null)
                    {
                        return NotFound();
                    }

                    var product = _db.Products.Include(c => c.SpecialTag).Include(c => c.ProductTypes).Where(c => c.Id == id).FirstOrDefault(c => c.Id == id);
                    if (product == null)
                    {
                        return NotFound();

                    }
                    return View(product);
                }
                //Post Delete
                [HttpPost]
                [ActionName("Delete")]
                public async Task<IActionResult> DeleteConfirm(int? id)
                {

                    if (id == null)
                    {
                        return NotFound();
                    }
                    var Product = _db.Products.FirstOrDefault(c => c.Id == id);
                    if (Product == null)
                    {
                        return NotFound();
                    }
                    _db.Products.Remove(Product);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }*/
    }

        }