using AutoFixture;
using AutoFixture.Kernel;
using Lib.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace FinancesReceiptCreator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserCreatorController : ControllerBase
    {
        private readonly ILogger<UserCreatorController> _logger;
        private readonly IFixture _fixture;
        private readonly IProducer _kafkaProducer;

        public UserCreatorController(
            IProducer kafkaProducer,
            ILogger<UserCreatorController> logger)
        {
            _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
            _logger = logger;

            _fixture = new Fixture();
            _fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
            _fixture.Customizations.Add(new UtcRandomDateTimeSequenceGenerator());
        }
        
        [HttpGet]
        public async Task<IActionResult> StartCreatingMessages()
        {
            var names = new List<string>()
            {
                "Артамонов Михаил Васильевич",
                "Большаков Иван Иванович",
                "Калашникова Александра Андреевна",
                "Карпова Светлана Максимовна",
                "Мальцева Елена Ярославовна",
                "Медведев Максим Алексеевич",
                "Павлов Владимир Станиславович",
                "Сизова Ника Вадимовна",
                "Смирнов Никита Русланович",
                "Третьякова Арина Дмитриевна",
            };

            var listOfReceipts = new List<UserDto>();

            for (var i = 0; i < 10; i++)
            {
                var users = _fixture.Build<UserDto>().Create();

                users.Id = i;
                users.Name = names[i];
                
                listOfReceipts.Add(users);
            }

            await _kafkaProducer.SendAsync(listOfReceipts, "user");

            return Ok();
        }
    }

    internal class UtcRandomDateTimeSequenceGenerator : ISpecimenBuilder
    {
        private readonly ISpecimenBuilder innerRandomDateTimeSequenceGenerator;

        internal UtcRandomDateTimeSequenceGenerator()
        {
            this.innerRandomDateTimeSequenceGenerator =
                new RandomDateTimeSequenceGenerator();
        }

        public object Create(object request, ISpecimenContext context)
        {
            var result =
                this.innerRandomDateTimeSequenceGenerator.Create(request, context);
            if (result is NoSpecimen)
                return result;

            return ((DateTime)result).ToUniversalTime();
        }
    }
}
