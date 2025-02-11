using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Ats_Demo.Application.Features.Employee.Queries.GetAll;
using Ats_Demo.Application.IRepositories;
using Ats_Demo.Application.IUnitOfWork;
using Ats_Demo.Application.Services;
using Ats_Demo.Domain.Dtos;
using Ats_Demo.Domain.Entities;
using AutoMapper;
using FluentAssertions;

namespace Ats_Demo.Tests
{
    public class GetAllEmployeesQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IRedisCacheService> _cacheServiceMock;
        private readonly GetAllEmployeesQueryHandler _handler;

        public GetAllEmployeesQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var employeeWriteRepoMock = new Mock<IEmployeeWriteRepository>();
            _unitOfWorkMock.Setup(u => u.EmployeeWriteRepository).Returns(employeeWriteRepoMock.Object);

            _mapperMock = new Mock<IMapper>();
            _cacheServiceMock = new Mock<IRedisCacheService>();

            _handler = new GetAllEmployeesQueryHandler(
                _cacheServiceMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnEmployees_WhenFoundInCache()
        {
            // Arrange
            var cachedEmployees = new List<EmployeeDetailsDto>
            {
                new EmployeeDetailsDto { Id = Guid.NewGuid(), Name = "Cached Employee 1" },
                new EmployeeDetailsDto { Id = Guid.NewGuid(), Name = "Cached Employee 2" }
            };

            _cacheServiceMock.Setup(c => c.GetCachedDataAsync<IEnumerable<EmployeeDetailsDto>>("AllEmployees"))
                             .ReturnsAsync(cachedEmployees);

            var query = new GetAllEmployeesQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(cachedEmployees);

            _unitOfWorkMock.Verify(u => u.EmployeeWriteRepository.GetAllAsync(null), Times.Never);
        }
        
    }
}
