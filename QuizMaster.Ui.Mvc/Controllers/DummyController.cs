using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using QuizMaster.Models;
using QuizMaster.Models.ViewModels;

public class DummyController : Controller
{
   
    //    [HttpGet]
    //    public IActionResult CreateQuestionsPageOne()
    //    {
    //        return View(new Quiz());
    //    }

    //    [HttpGet]
    //    public IActionResult CreateQuestionsPageTwo()
    //    {
    //        var viewModel = new QuizWithQuestionsViewModel
    //        {
    //            Quiz = new Quiz(),
    //            Questions = new List<QuestionInputModel>()
    //        };

    //        return View(viewModel);
    //    }

    //    [HttpPost]
    //    public IActionResult CreateQuestionsPageTwo(QuizWithQuestionsViewModel model)
    //    {
    //        // Voorlopig geen verwerking
    //        TempData["message"] = "Quiz ingediend (zonder verwerking).";
    //        return RedirectToAction("CreateQuestionsPageOne");
    //    }


    //    public IActionResult Leaderboard()
    //{
    //    return View();
    //}

    //public IActionResult AddBadges()
    //{
    //    return View();
    //}

    //public IActionResult EditBadges()
    //{
    //    return View();
    //}
    //public IActionResult CreateUser()
    //{
    //    return View();
    //}

    //public IActionResult AddUser()
    //{
    //    return View();
    //}

    //public IActionResult EditUser()
    //{
    //    return View();
    //}

    //public IActionResult CreateBadge()
    //{
    //    return View();
    //}

}
