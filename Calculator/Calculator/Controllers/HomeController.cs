using Calculator.Data;
using Calculator.Managers;
using Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ErrorHandler m_errorHandler;
        private readonly ICalculationManager m_calculationManager;
        private readonly ICalculationRepository m_calculationRep;

        public HomeController(AppDbContext context)
        {
            m_errorHandler = new ErrorHandler();
            m_calculationRep = new CalculationRepository(context);
            m_calculationManager = new CalculationManager(m_calculationRep, m_errorHandler.SendError);
        }

        public IActionResult Index()
        {
            ViewBag.History = m_calculationManager.HistoryOfLastN(10);
            return View();
        }

        [HttpPost]
        public JsonResult Calculate([FromBody] Calculation calc)
        {
            if (ModelState.IsValid) {
                try {
                    m_calculationManager.ProcessCalculation(calc);
                    m_calculationManager.InsertToDb(calc);
                    var history = m_calculationManager.HistoryOfLastN(10);
                    return Json(new { 
                        success = true, 
                        result = calc.Result,
                        history
                    });
                }
                catch (Exception ex) {
                    ModelState.AddModelError("Expression", ex.Message);
                }
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
