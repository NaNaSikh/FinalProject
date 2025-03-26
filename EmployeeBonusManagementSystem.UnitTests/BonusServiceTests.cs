using EmployeeBonusManagementSystem.Application.Contracts.Persistence;
using EmployeeBonusManagementSystem.Application.Features.Bonuses.Commands.AddBonuses;
using EmployeeBonusManagementSystem.Domain.Entities;
using EmployeeBonusManagementSystem.Persistence;
using FakeItEasy;

namespace EmployeeBonusManagementSystem.UnitTests
{
    public class BonusServiceTests
    {
        private readonly IBonusRepository _bonusRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContextService _userContextService;
        private readonly ILoggingRepository _loggingRepository;
        private readonly AddBonusesQueryHandler _handler;

        public BonusServiceTests()
        {
            _bonusRepository = A.Fake<IBonusRepository>();
            _employeeRepository = A.Fake<IEmployeeRepository>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            _userContextService = A.Fake<IUserContextService>();
            _loggingRepository = A.Fake<ILoggingRepository>();

            _handler = new AddBonusesQueryHandler(_unitOfWork, _userContextService, _loggingRepository);

            A.CallTo(() => _unitOfWork.EmployeeRepository).Returns(_employeeRepository);
            A.CallTo(() => _unitOfWork.BonusRepository).Returns(_bonusRepository);

            A.CallTo(() => _userContextService.GetUserId()).Returns(2);
        }

        [Fact]
        public async Task AddBonus_employeeNotFound_ThrowsException()
        {
            var query = new AddBonusesCommand("12345678901", 500);

            // თანამშრომელი არ არსებობს
            A.CallTo(() => _employeeRepository.GetEmployeeExistsByPersonalNumberAsync(A<string>._))
                .Returns(Task.FromResult<(bool, int)>((false, 0)));

            await Assert.ThrowsAsync<Exception>(async () =>
                await _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task AddBonus_ValidEmployee_AddsBonusSuccessfully()
        {
            var query = new AddBonusesCommand("12345678901", 1000);

            A.CallTo(() => _employeeRepository.GetEmployeeExistsByPersonalNumberAsync(A<string>._))
                .Returns(Task.FromResult<(bool, int)>((true, 5)));

            A.CallTo(() => _bonusRepository.AddBonusAsync(A<BonusEntity>._, A<int>._))
               .Returns(Task.FromResult(new List<AddBonusesDto>
               {
                   new AddBonusesDto
                   {
                       EmployeeId = 5,
                       Amount = 1000
                   }
               }));

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(5, result[0].EmployeeId);
            Assert.Equal(1000, result[0].Amount);

            A.CallTo(() => _bonusRepository.AddBonusAsync(A<BonusEntity>._, A<int>._))
                .MustHaveHappenedOnceExactly();
        }
    }
}

