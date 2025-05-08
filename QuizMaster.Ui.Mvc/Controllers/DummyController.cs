using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

public class DummyController : Controller
{
    [HttpGet]
    public IActionResult CreateQuestionsPageOne()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateQuestionsPageOne(IFormFile image)
    {
        string title = Request.Form["title"];
        string description = Request.Form["description"];

        // Opslaan in TempData (simpele sessie-opslag)
        TempData["title"] = title;
        TempData["description"] = description;
        TempData["imageName"] = image?.FileName ?? "";

        return RedirectToAction("CreateQuestionsPageTwo");
    }

    [HttpGet]
    public IActionResult CreateQuestionsPageTwo()
    {
        // Houd TempData in leven bij Peek
        ViewBag.Title = TempData.Peek("title");
        ViewBag.Description = TempData.Peek("description");
        ViewBag.ImageName = TempData.Peek("imageName");

        return View();
    }

    [HttpPost]
    public IActionResult Save()
    {
        string title = Request.Form["title"];
        string description = Request.Form["description"];
        string imageName = Request.Form["imageName"];

        Console.WriteLine("Titel: " + title);
        Console.WriteLine("Beschrijving: " + description);
        Console.WriteLine("Afbeelding: " + imageName);

        // Alle vragen ophalen
        var form = Request.Form;
        int questionIndex = 0;

        while (form.ContainsKey($"questions[{questionIndex}][text]"))
        {
            string vraag = form[$"questions[{questionIndex}][text]"];
            Console.WriteLine($"Vraag {questionIndex + 1}: {vraag}");

            for (int i = 0; i < 4; i++)
            {
                string antwoord = form[$"questions[{questionIndex}][answers][{i}]"];
                Console.WriteLine($" - Antwoord {i + 1}: {antwoord}");
            }

            string correct = form[$"questions[{questionIndex}][correct]"];
            Console.WriteLine($" --> Correct antwoord index: {correct}");

            questionIndex++;
        }

        // Hier zou je kunnen opslaan of een bevestigingspagina tonen
        TempData["message"] = "Quiz succesvol aangemaakt!";
        return RedirectToAction("CreateQuestionsPageOne");
    }

    public IActionResult Leaderboard()
    {
        return View();
    }

    public IActionResult AddBadges()
    {
        return View();
    }

    public IActionResult EditBadges()
    {
        return View();
    }
    public IActionResult CreateUser()
    {
        return View();
    }

    public IActionResult AddUser()
    {
        return View();
    }

}
