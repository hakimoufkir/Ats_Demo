using Xunit;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ats_Demo.Application.Features.Employee.Queries.GetById;
using Ats_Demo.Application.IUnitOfWork;
using Ats_Demo.Application.Services;
using Ats_Demo.Application.Exceptions;
using Ats_Demo.Domain.Dtos;
using Ats_Demo.Domain.Entities;
using AutoMapper;
using FluentAssertions;

namespace Ats_Demo.Tests
{
    public class GetEmployeeByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly GetEmployeeByIdQueryHandler _handler;

        public GetEmployeeByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();

            _handler = new GetEmployeeByIdQueryHandler(
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _cacheServiceMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnEmployee_WhenFoundInCache()
        {
            // Arrange
            var employeeId = Guid.NewGuid();
            var cachedEmployee = new EmployeeDetailsDto { Id = employeeId, Name = "Cached Employee" };

            _cacheServiceMock.Setup(c => c.GetCachedDataAsync<EmployeeDetailsDto>($"Employee_{employeeId}"))
                             .ReturnsAsync(cachedEmployee);

            var query = new GetEmployeeByIdQuery(employeeId);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(cachedEmployee);

            _unitOfWorkMock.Verify(u => u.EmployeeWriteRepository.GetAsync(It.IsAny<Expression<Func<Employee, bool>>>(), It.IsAny<bool>()), Times.Never);
        }
    }
}
