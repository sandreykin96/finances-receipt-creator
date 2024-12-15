using AutoFixture;
using AutoFixture.Kernel;
using Lib.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace FinancesReceiptCreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReceiptCreatorController : ControllerBase
    {
        private readonly ILogger<ReceiptCreatorController> _logger;
        private readonly IFixture _fixture;
        private readonly IProducer _kafkaProducer;

        public ReceiptCreatorController(
            IProducer kafkaProducer,
            ILogger<ReceiptCreatorController> logger)
        {
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
            _logger = logger;

            _fixture = new Fixture();
            _fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
            _fixture.Customizations.Add(new UtcRandomDateTimeSequenceGenerator());
        }

        [HttpGet]
        public async Task<IActionResult> StartCreatingMessages([FromQuery] int amount)
        {
            
            var listOfReceipts = new List<ReceiptDto>();

            for (var i = 0; i < 100; i++)
            {
                var receipt = _fixture.Build<ReceiptDto>().Create();

                receipt.Id = i;
                receipt.User = $"user_{i}";

                listOfReceipts.Add(receipt);

            }

            await _kafkaProducer.SendAsync(listOfReceipts, "receipt");

            return Ok();
        }
    }
}
