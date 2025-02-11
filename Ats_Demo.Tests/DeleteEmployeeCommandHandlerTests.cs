using Xunit;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ats_Demo.Application.Features.Employee.Commands.DeleteById;
using Ats_Demo.Application.IRepositories;
using Ats_Demo.Application.IUnitOfWork;
using Ats_Demo.Application.Services;
using Ats_Demo.Application.Exceptions;
using Ats_Demo.Domain.Entities;
using AutoMapper;
using FluentAssertions;

namespace Ats_Demo.Tests
{
    public class DeleteEmployeeCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly DeleteEmployeeCommandHandler _handler;

        public DeleteEmployeeCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cacheServiceMock = new Mock<IRedisCacheService>();

            _handler = new DeleteEmployeeCommandHandler(
                _unitOfWorkMock.Object,
                _cacheServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldDeleteEmployee_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var existingEmployee = new Employee { Id = employeeId };

            _unitOfWorkMock.Setup(u => u.EmployeeWriteRepository.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>(), It.IsAny<bool>()))
                           .ReturnsAsync(existingEmployee);

            _unitOfWorkMock.Setup(u => u.EmployeeWriteRepository.RemoveAsync(existingEmployee))
                           .Returns(Task.CompletedTask);

            var command = new DeleteEmployeeCommand { Id = employeeId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be($"Employee with ID {employeeId} deleted successfully.");

            _unitOfWorkMock.Verify(u => u.EmployeeWriteRepository.RemoveAsync(existingEmployee), Times.Once);
        }
    }
}
