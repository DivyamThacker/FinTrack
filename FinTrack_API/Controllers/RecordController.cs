using AutoMapper;
using FinTrack_Business.Repository;
using FinTrack_Business.Repository.IRepository;
using FinTrack_DataAccess;
using FinTrack_Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            return Ok(await _recordRepository.GetAll(userId));
        }

        //[HttpGet]
        //[ActionName("GetAll")]
        ////[Route("Get")]
        //public async Task<IActionResult> GetAll([FromQuery] RecordDTO recordDTO)
        //{
        //    Expression<Func<RecordDTO, bool>> NameFilter = (record) => recordDTO.Name == null ? true :
        //record.Name.StartsWith(recordDTO.Name);

        //    var entities = await _recordRepository.GetFilteredAsync(new Expression<Func<RecordDTO, bool>>[]
        //{
        //    NameFilter
        //}, null, null, []);

        //    return Ok(entities);
        //}


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
            //recordDTO.Color = recordDTO.IsIncome ? "green" : "red";
            //if (recordDTO.RecordDate == DateTime.MinValue)
            //recordDTO.RecordDate = DateTime.Now;
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
