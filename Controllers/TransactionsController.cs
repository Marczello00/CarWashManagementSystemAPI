using Microsoft.AspNetCore.Mvc;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Models;
using AutoMapper;
using CarWashManagementSystem.Dtos;
using Microsoft.EntityFrameworkCore;
using CarWashManagementSystem.Filters;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CarWashManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;
        public TransactionsController(DataContext context, IMapper mapper, ILogger<TransactionsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDto>))]
        [ProducesResponseType(204)]
        public IActionResult GetLastFive()
        {
            var transactions = _context.Transactions
                .Include(t => t.Station)
                .ThenInclude(s => s.StationType)
                .Include(t => t.TransactionSource)
                .OrderByDescending(t => t.DateTime)
                .Take(5)
                .ToList();

            var mapped = _mapper.Map<List<TransactionDto>>(transactions);


            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }
            return Ok(mapped);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TransactionDto))]
        [ProducesResponseType(404)]
        public IActionResult GetTransactionById(long id)
        {
            var transaction = _context.Transactions
                .Include(t => t.Station)
                .ThenInclude(s => s.StationType)
                .Include(t => t.TransactionSource)
                .FirstOrDefault(t => t.Id == id);
            var mapped = _mapper.Map<TransactionDto>(transaction);
            if (mapped == null)
            {
                return NotFound();
            }
            return Ok(mapped);
        }
        [HttpGet("station/{stationId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDto>))]
        [ProducesResponseType(204)]
        public IActionResult GetTransactionsByStationId(long stationId)
        {
            var transactions = _context.Transactions
                .Include(t => t.Station)
                .ThenInclude(s => s.StationType)
                .Include(t => t.TransactionSource)
                .Where(t => t.StationId == stationId)
                .ToList();
            var mapped = _mapper.Map<List<TransactionDto>>(transactions);
            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }
            return Ok(mapped);
        }
        [HttpGet("stationType/{stationTypeId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDto>))]
        [ProducesResponseType(204)]
        public IActionResult GetTransactionsByStationTypeId(long stationTypeId)
        {
            var transactions = _context.Transactions
                .Include(t => t.Station)
                .ThenInclude(s => s.StationType)
                .Include(t => t.TransactionSource)
                .Where(t => t.Station.StationTypeId == stationTypeId)
                .ToList();
            var mapped = _mapper.Map<List<TransactionDto>>(transactions);
            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }
            return Ok(mapped);
        }
        [HttpGet("source/{sourceId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TransactionDto>))]
        [ProducesResponseType(204)]
        public IActionResult GetTransactionsBySourceId(long sourceId)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation($"Remote IP Address: {remoteIpAddress}");
            var transactions = _context.Transactions
                .Include(t => t.Station)
                .ThenInclude(s => s.StationType)
                .Include(t => t.TransactionSource)
                .Where(t => t.TransactionSourceId == sourceId)
                .ToList();
            var mapped = _mapper.Map<List<TransactionDto>>(transactions);
            if (mapped == null || (mapped.Count == 0))
            {
                return NoContent();
            }
            return Ok(mapped);
        }
        [HttpPost]
        [AllowStationIp]
        [ProducesResponseType(201, Type = typeof(TransactionDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public IActionResult CreateTransaction(CreateTransactionDto createTransactionDto)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            _logger.LogInformation($"Remote IP Address: {remoteIpAddress}");
            if (createTransactionDto == null)
                return BadRequest("Invalid data.");

            // Znajdź odpowiednią stację na podstawie StationNumber i StationTypeName
            var station = _context.Stations
                .Include(s => s.StationType)
                .FirstOrDefault(s => s.StationNumber == createTransactionDto.StationNumber &&
                                     s.StationType.StationTypeName == createTransactionDto.StationTypeName);
            if (station == null)
                return NotFound("Station not found.");

            // Znajdź odpowiednie źródło transakcji na podstawie TransactionSourceName
            var transactionSource = _context.TransactionSources
                .FirstOrDefault(ts => ts.SourceName == createTransactionDto.TransactionSourceName);
            if (transactionSource == null)
                return NotFound("Transaction source not found.");


            var transaction = new Transaction
            {
                DateTime = DateTime.Now,  // Ustaw bieżącą datę i godzinę
                WasFiscalized = createTransactionDto.WasFiscalized,
                Value = createTransactionDto.Value,
                StationId = station.Id,
                TransactionSourceId = transactionSource.Id
            };

            _context.Transactions.Add(transaction);
            _context.SaveChanges(); 
            var transactionDto = _mapper.Map<TransactionDto>(transaction);
            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transactionDto);

        }

    }
}
