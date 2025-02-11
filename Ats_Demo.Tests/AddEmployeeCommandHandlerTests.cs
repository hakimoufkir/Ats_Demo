using Xunit;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Ats_Demo.Application.Features.Employee.Commands.Add;
using Ats_Demo.Application.IRepositories;
using Ats_Demo.Application.IUnitOfWork;
using Ats_Demo.Application.Services;
using Ats_Demo.Domain.Dtos;
using Ats_Demo.Domain.Entities;
using AutoMapper;
using FluentAssertions;

namespace Ats_Demo.Tests
{
    public class AddEmployeeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly AddEmployeeCommandHandler _handler;

        public AddEmployeeCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();

            _handler = new AddEmployeeCommandHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _cacheServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldCreateEmployee_WhenValidRequest()
        {
            // Arrange
            var employeeDto = new CreateEmployeeDto
            {
                Name = "John Doe",
                Position = "Developer",
                Office = "NY",
                Age = 30,
                Salary = 75000
            };

            var employeeEntity = new Employee
            {
                Id = Guid.NewGuid(),
                Name = employeeDto.Name,
                Position = employeeDto.Position,
                Office = employeeDto.Office,
                Age = employeeDto.Age,
                Salary = employeeDto.Salary,
                CreatedDate = DateTime.UtcNow
            };

            var employeeDetailsDto = new EmployeeDetailsDto
            {
                Id = employeeEntity.Id,
                Name = employeeEntity.Name,
                Position = employeeEntity.Position,
                Office = employeeEntity.Office
            };

            _mapperMock.Setup(m => m.Map<Employee>(It.IsAny<CreateEmployeeDto>())).Returns(employeeEntity);
            _mapperMock.Setup(m => m.Map<EmployeeDetailsDto>(employeeEntity)).Returns(employeeDetailsDto);

            _unitOfWorkMock.Setup(u => u.EmployeeWriteRepository.CreateAsync(employeeEntity))
                          .Returns(Task.CompletedTask);

            var command = new AddEmployeeCommand(employeeEntity.Name, employeeDetailsDto.Position, employeeDetailsDto.Office, employeeDetailsDto.Age, employeeDetailsDto.Salary);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("John Doe");
            result.Position.Should().Be("Developer");

            _unitOfWorkMock.Verify(u => u.EmployeeWriteRepository.CreateAsync(It.IsAny<Employee>()), Times.Once);
            _cacheServiceMock.Verify(c => c.RemoveCacheDataAsync("AllEmployees"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenEmployeeDtoIsNull()
        {
            // Arrange
            var command = new AddEmployeeCommand(null, null, null, null, null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
