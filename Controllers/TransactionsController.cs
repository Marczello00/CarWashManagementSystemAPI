using AutoMapper;
using CarWashManagementSystem.Constants;
using CarWashManagementSystem.Data;
using CarWashManagementSystem.Dtos;
using CarWashManagementSystem.Filters;
using CarWashManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace CarWashManagementSystem.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    [Authorize]
    public class TransactionsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionsController> _logger;
        private readonly IHttpClientFactory _clientFactory;
        public TransactionsController(DataContext context, IMapper mapper, ILogger<TransactionsController> logger, IHttpClientFactory clientFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
        }
        [HttpGet]
        [Authorize(Roles = UserRoles.Owner)]
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
        [Authorize(Roles = UserRoles.Owner)]
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
        [Authorize(Roles = UserRoles.Owner)]
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
        [Authorize(Roles = UserRoles.Owner)]
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
        [Authorize(Roles = UserRoles.Owner)]
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
        [AllowAnonymous]
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

            var station = _context.Stations
                .Include(s => s.StationType)
                .FirstOrDefault(s => s.StationNumber == createTransactionDto.StationNumber &&
                                     s.StationType.StationTypeName == createTransactionDto.StationTypeName);
            if (station == null)
                return NotFound("Station not found.");

            var transactionSource = _context.TransactionSources
                .FirstOrDefault(ts => ts.SourceName == createTransactionDto.TransactionSourceName);
            if (transactionSource == null)
                return NotFound("Transaction source not found.");


            var transaction = new Transaction
            {
                DateTime = DateTime.Now,
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
        [HttpPost("make-transaction")]
        [Authorize(Roles = UserRoles.Owner)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(502)]
        public async Task<IActionResult> MakeTransaction(int stationId, int amount)
        {
            if (amount < 1 || amount > 20)
            {
                return BadRequest("Amount should be between 1-20");
            }
            var station = _context.Stations.Include(s => s.AllowedIp)
                .FirstOrDefault(s => s.Id == stationId);
            if (station == null || !station.IsActive)
            {
                return BadRequest("Station is not active");
            }
            var stationIp = station.AllowedIp.IpAddress;
            var url = $"http://{stationIp}/transaction";
            var client = _clientFactory.CreateClient("nonStandardHttpClient");
            var json = JsonSerializer.Serialize(new { creditCount = amount });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest("Transaction failed");
                }
            }
            catch (Exception e)
            {
                return StatusCode(502, e.Message);
            }

        }

    }
}
