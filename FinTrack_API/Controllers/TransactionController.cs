using AutoMapper;
using FinTrack_Business.Repository;
using FinTrack_Business.Repository.IRepository;
using FinTrack_Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinTrack_API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionController(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAll(string accountId)
        {
            return Ok(await _transactionRepository.GetAll(accountId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var record = await _transactionRepository.Get(id);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create([FromBody] TransactionDTO transactionDTO)
        {
            if (transactionDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid amount");
            }
            //transactionDTO.TransactionDate = DateTime.Now;
            return Ok(await _transactionRepository.Create(transactionDTO));
        }

        [HttpPatch]
        [ActionName("Update")]
        public async Task<IActionResult> Update([FromBody] TransactionDTO transactionDTO)
        {
            if (transactionDTO.Amount <= 0)
            {
                return BadRequest("Please enter valid amount");
            }

            var record = await _transactionRepository.Update(transactionDTO);
            if (record == null)
            {
                return NotFound();
            }
            return Ok(record);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _transactionRepository.Get(id);
            if (record == null)
            {
                return NotFound();
            }
            var deletedRecord = await _transactionRepository.Delete(id);
            if (deletedRecord == 0)
            {
                return StatusCode(500);
            }
            return Ok();
        }
    }
}
