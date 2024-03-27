using BLL.Services.Interfaces;
using BLL.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MvcWebUI.Models;
using System.Diagnostics;

namespace MvcWebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfigService _configService;
        private readonly IValidator<IFormFile> _formFileValidator;
        public HomeController(ILogger<HomeController> logger, IConfigService configService, IValidator<IFormFile> validator)
        {
            _logger = logger;
            _configService = configService;
            _formFileValidator = validator;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file.Length > 0 && file != null)
            {
                var validationResult = _formFileValidator.Validate(file);

                if (validationResult.IsValid)
                {
                    using var reader = new StreamReader(file.OpenReadStream());
                    var jsonString = await reader.ReadToEndAsync();

                    int configId = await _configService.AddConfigAsync(jsonString);

                    return RedirectToAction("DisplayTree", new { id = configId });
                }

                else
                {
                    foreach (var error in validationResult.Errors)
                    {
                        ModelState.AddModelError("File", error.ErrorMessage);
                    }
                    return BadRequest(ModelState);
                }
            }
            return BadRequest();
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        public async Task<IActionResult> DisplayTree(int id)
        {
            try
            {
                var config = await _configService.GetConfigByIdAsync(id);
                return View(config);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Index()
        {
            return RedirectToAction("UploadFile");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
