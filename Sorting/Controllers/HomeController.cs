using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sorting.Models;
using Sorting.Classes;
using System.Diagnostics;

public class HomeController : Controller
{
    private static List<SortedFileModel> sortedFiles = new List<SortedFileModel>();

    public ActionResult Index()
    {
        return View(sortedFiles);
    }

    [HttpPost]
    public ActionResult UploadAndSort(UploadFileViewModel model)
    {
        if (model.File != null && model.File.Length > 0)
        {
            var fileName = model.File.FileName;
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var filePath = Path.Combine(uploadsFolder, fileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                    // Немає потреби в stream.Close(), так як using автоматично закриває файл після завершення блоку коду
                }

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                var numbers = ReadAndSortFile(filePath, model.SortingMethod);
                stopwatch.Stop();

                var uniqueFileName = GetUniqueFileName(Path.GetFileNameWithoutExtension(fileName));
                var newFilePath = Path.Combine(uploadsFolder, uniqueFileName + Path.GetExtension(fileName));

                SaveSortedNumbersToFile(numbers, newFilePath);

                var sortedFile = new SortedFileModel
                {
                    FileName = uniqueFileName,
                    FileSize = model.File.Length,
                    SortingMethod = model.SortingMethod,
                    SortingTime = stopwatch.Elapsed.TotalSeconds.ToString("F" + 2) + " s",
                };

                sortedFiles.Add(sortedFile);

                return View("Index", sortedFiles);
            }
            catch (IOException ex)
            {
                // Обробка винятку, наприклад, вивід повідомлення або журналування помилки
                Console.WriteLine($"Помилка при роботі з файлом: {ex.Message}");
                return View("Error"); // Повернення сторінки з повідомленням про помилку
            }
        }

        return RedirectToAction("Index");
    }


    [HttpGet]
    public IActionResult Download(string filename)
    {
        var sortedFile = sortedFiles.FirstOrDefault(f => f.FileName == filename);

        if (sortedFile != null)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads");
            var filePath = Path.Combine(folderPath, sortedFile.FileName + ".txt");

            if (System.IO.File.Exists(filePath))
            {
                byte[] fileContents = System.IO.File.ReadAllBytes(filePath);
                return File(fileContents, "application/octet-stream", sortedFile.FileName + ".txt");
            }
        }

        return RedirectToAction("Index");
    }

    private List<int> ReadAndSortFile(string filePath, string sortingMethod)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string content = reader.ReadToEnd();
            List<int> numbers = content.Split(',').Select(int.Parse).ToList();

            switch (sortingMethod)
            {
                case "BubbleSort":
                   SortingAlgs.BubbleSort(numbers);
                    break;
                case "SelectionSortArray":
                    SortingAlgs.SelectionSortArray(numbers);
                    break;
                case "Timsort":
                    SortingAlgs.Timsort(numbers);
                    break;
                 
            }

            return numbers;
        }
    }

    

    private string GetUniqueFileName(string fileName)
    {
        fileName = Path.GetFileNameWithoutExtension(fileName);
        fileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmssfff}";
        return fileName;
    }

    private void SaveSortedNumbersToFile(List<int> numbers, string filePath)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var number in numbers)
            {
                writer.Write(number + ",");
            }
        }
    }
}
