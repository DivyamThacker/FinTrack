using FinTrack_Business.Repository.IRepository;
using FinTrack_Common;
using FinTrack_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
namespace FinTrack_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly IBudgetRepository _budgetRepository;
        public BudgetController(IBudgetRepository budgetRepository)
        {
            _budgetRepository = budgetRepository;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAll(string accountId)
        {
            return Ok(await _budgetRepository.GetAll(accountId));
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] BudgetDTO budgetDTO)
        {
            #region Period Validation
            if (budgetDTO.Period == SD.Period_Custom)
            {
                if (budgetDTO.StartTime == DateTime.MinValue || budgetDTO.EndTime == DateTime.MinValue || budgetDTO.StartTime > budgetDTO.EndTime)
                {
                    return BadRequest("Please enter valid start and end time for Custom budget");
                }
            }
            else if (budgetDTO.Period == SD.Period_Week)
            {
                DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                budgetDTO.StartTime = startOfWeek;
                budgetDTO.EndTime = endOfWeek;
            }
            else if (budgetDTO.Period == SD.Period_Week)
            {
                budgetDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                budgetDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (budgetDTO.Period == SD.Period_Month)
            {
                budgetDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                budgetDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (budgetDTO.Period == SD.Period_Year)
            {
                budgetDTO.StartTime = new DateTime(DateTime.Now.Year, 1, 1); ;
                budgetDTO.EndTime = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
            }
            #endregion
            if (budgetDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid budget amount");
            }
            if (budgetDTO.Category==null)
            {
                budgetDTO.Category = SD.Category_All;
            }
            budgetDTO.Status = SD.Status_Pending;
            var result = await _budgetRepository.Create(budgetDTO);
            return Ok(result);
        }
        
        [HttpDelete("{id:int}")]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _budgetRepository.Delete(id);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _budgetRepository.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPatch]
        [ActionName("Update")]
        public async Task<IActionResult> Update([FromBody] BudgetDTO budgetDTO)
        {
            #region Period Validation
            if (budgetDTO.Period == SD.Period_Custom)
            {
                if (budgetDTO.StartTime == DateTime.MinValue || budgetDTO.EndTime == DateTime.MinValue || budgetDTO.StartTime > budgetDTO.EndTime)
                {
                    return BadRequest("Please enter valid start and end time for Custom budget");
                }
            }
            else if (budgetDTO.Period == SD.Period_Week)
            {
                DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                budgetDTO.StartTime = startOfWeek;
                budgetDTO.EndTime = endOfWeek;
            }
            else if (budgetDTO.Period == SD.Period_Week)
            {
                budgetDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                budgetDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (budgetDTO.Period == SD.Period_Month)
            {
                budgetDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                budgetDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (budgetDTO.Period == SD.Period_Year)
            {
                budgetDTO.StartTime = new DateTime(DateTime.Now.Year, 1, 1); ;
                budgetDTO.EndTime = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
            }
            #endregion

            if (budgetDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid budget amount");
            }
            if (budgetDTO.Category == null)
            {
                budgetDTO.Category = SD.Category_All;
            }
            //Remember to add Status handling Logic here, also make sure if the above period validation is correct for the update method
            var result = await _budgetRepository.Update(budgetDTO);
            return Ok(result);  
        }


    }
}
