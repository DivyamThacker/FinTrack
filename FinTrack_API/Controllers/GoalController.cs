using FinTrack_Business.Repository;
using FinTrack_Business.Repository.IRepository;
using FinTrack_Common;
using FinTrack_Models;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private readonly IGoalRepository _goalRepository;
        public GoalController(IGoalRepository goalRepository)
        {
            _goalRepository = goalRepository;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAll(string accountId)
        {
            return Ok(await _goalRepository.GetAll(accountId));
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] GoalDTO goalDTO)
        {
            #region Period Validation
            if (goalDTO.Period == SD.Period_Custom)
            {
                if (goalDTO.StartTime == DateTime.MinValue || goalDTO.EndTime == DateTime.MinValue || goalDTO.StartTime > goalDTO.EndTime)
                {
                    return BadRequest("Please enter valid start and end time for Custom goal");
                }
            }
            else if (goalDTO.Period == SD.Period_Week)
            {
                DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                goalDTO.StartTime = startOfWeek;
                goalDTO.EndTime = endOfWeek;
            }
            else if (goalDTO.Period == SD.Period_Week)
            {
                goalDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                goalDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (goalDTO.Period == SD.Period_Month)
            {
                goalDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                goalDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (goalDTO.Period == SD.Period_Year)
            {
                goalDTO.StartTime = new DateTime(DateTime.Now.Year, 1, 1); ;
                goalDTO.EndTime = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
            }
            #endregion
            if (goalDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid goal amount");
            }
           
            if (goalDTO.Name == null || goalDTO.Name == "")
            {
                return BadRequest("");
            }
            var result = await _goalRepository.Create(goalDTO);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _goalRepository.Delete(id);
            if (result == 0)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _goalRepository.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPatch]
        [ActionName("Update")]
        public async Task<IActionResult> Update([FromBody] GoalDTO goalDTO)
        {
            #region Period Validation
            if (goalDTO.Period == SD.Period_Custom)
            {
                if (goalDTO.StartTime == DateTime.MinValue || goalDTO.EndTime == DateTime.MinValue || goalDTO.StartTime > goalDTO.EndTime)
                {
                    return BadRequest("Please enter valid start and end time for Custom budget");
                }
            }
            else if (goalDTO.Period == SD.Period_Week)
            {
                DateTime startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                goalDTO.StartTime = startOfWeek;
                goalDTO.EndTime = endOfWeek;
            }
            else if (goalDTO.Period == SD.Period_Week)
            {
                goalDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                goalDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (goalDTO.Period == SD.Period_Month)
            {
                goalDTO.StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); ;
                goalDTO.EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            }
            else if (goalDTO.Period == SD.Period_Year)
            {
                goalDTO.StartTime = new DateTime(DateTime.Now.Year, 1, 1); ;
                goalDTO.EndTime = new DateTime(DateTime.Now.Year, 12, DateTime.DaysInMonth(DateTime.Now.Year, 12));
            }
            #endregion

            if (goalDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid goal amount");
            }
            if (goalDTO.Category == null)
            {
                goalDTO.Category = SD.Category_All;
            }
            //Remember to add Status handling Logic here, also make sure if the above period validation is correct for the update method
            var result = await _goalRepository.Update(goalDTO);
            return Ok(result);
        }

    }
}
