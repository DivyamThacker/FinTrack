using FinTrack_Business.Repository.IRepository;
using FinTrack_Models;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecordController :  ControllerBase
    {
        private readonly IRecordRepository _recordRepository;
        public RecordController(IRecordRepository recordRepository)
        {
            _recordRepository = recordRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _recordRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var record = await _recordRepository.Get(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] RecordDTO recordDTO)
        {
            if (recordDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid amount");
            }
            recordDTO.Color = recordDTO.IsIncome ? "green" : "red";
            recordDTO.RecordDate = DateTime.Now;
            return Ok(await _recordRepository.Create(recordDTO));
        }

        [HttpPatch]
        [ActionName("Update")]
        public async Task<IActionResult> Update([FromBody] RecordDTO recordDTO)
        {
            if (recordDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid amount");
            }
            recordDTO.Color = recordDTO.IsIncome ? "green" : "red";

            var record = await _recordRepository.Update(recordDTO);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _recordRepository.Get(id);
            if (record == null)
            {
                return NotFound();
            }
            var deletedRecord = await _recordRepository.Delete(id);
            if (deletedRecord == 0)
            {
                return StatusCode(500);
            }
            return Ok();
        }   
    }
}
