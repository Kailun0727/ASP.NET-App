﻿using BulkyBookWeb.Data;
using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories;
            return View(objCategoryList);
        }

        //GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                // If the 'Name' matches the converted 'DisplayOrder', add a validation error to the model state with an appropriate message.
                ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
            }

            if (ModelState.IsValid) {
                //add category object to database
                _db.Categories.Add(obj);

                //save changes to database
                _db.SaveChanges();

				TempData["success"] = "Category created succesfully!";

                return RedirectToAction("Index");
            }
            // If the model state is not valid, return the 'Category' object back to the view with validation error messages.
            return View(obj);
         
        }


		//GET
		[HttpGet]
		public IActionResult Edit(int? id)
		{
            if(id == null || id == 0) {
                return NotFound();
            }

            var categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();
            }

			return View(categoryFromDb);
		}

		//POST
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(Category obj)
		{
			if (obj.Name == obj.DisplayOrder.ToString())
			{
				// If the 'Name' matches the converted 'DisplayOrder', add a validation error to the model state with an appropriate message.
				ModelState.AddModelError("Name", "The DisplayOrder cannot exactly match the Name.");
			}

			if (ModelState.IsValid)
			{
				//add category object to database
				_db.Categories.Update(obj);

				//save changes to database
				_db.SaveChanges();

				TempData["success"] = "Category updated succesfully!";

				return RedirectToAction("Index");
			}
			// If the model state is not valid, return the 'Category' object back to the view with validation error messages.
			return View(obj);

		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			var categoryFromDb = _db.Categories.Find(id);
			//var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
			//var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

			if (categoryFromDb == null)
			{
				return NotFound();
			}

			return View(categoryFromDb);
		}

		//POST
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePOST(int? id)
		{
			var obj = _db.Categories.Find(id);
			if (obj == null)
			{
				return NotFound();
			}

			_db.Categories.Remove(obj);
			_db.SaveChanges();

			TempData["success"] = "Category deleted successfully";

			return RedirectToAction("Index");

		}
	}
}
