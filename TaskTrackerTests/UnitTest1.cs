using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

using TaskTracker.Controllers;
using TaskTracker.Data.Dto;
using TaskTracker.Repositories;
using System;
using Project = TaskTracker.Data.Models.Project;

namespace TaskTrackerTests
{
    public class UnitTest1
    {
        private readonly ProjectsController _controller;
        private readonly Mock<IProjectRepository> _mockRepository;
        public UnitTest1()
        {
            _mockRepository = new Mock<IProjectRepository>();
            _controller = new ProjectsController(_mockRepository.Object);

        }

        [Fact]
        public void Get_AllItemsResultType()
        {
            var projects = _controller.Get(null, null, null, null, null);

            Assert.IsAssignableFrom<Task<ActionResult<IEnumerable<ProjectDtoResponse>>>>(projects);
        }

        [Fact]
        public void GetById_InvalidId_ReturnsNotFoundResult()
        {
            var result = _controller.Get(123);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsObjectResult()
        {
            var project = new ProjectDtoResponse()
            {
                Id = 1,
                Name = "Project #1",
                Priority = 1
            };
            _mockRepository.Setup(p => p.Get(1)).ReturnsAsync(project);
            var result = await _controller.Get(1);
            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public void Create_ValidDto_ReturnsCreatedAtRouteResult()
        {
            ProjectDtoRequest project = new() { Name = "Project test", Priority = 1 };
            var result = _controller.Create(project);

           Assert.IsType<CreatedAtRouteResult>(result.Result);
        }

        [Fact]
        public void Create_InvalidDto_ReturnsBadRequestObjectResult()
        {
            ProjectDtoRequest project = new() { Priority = 1 };
            var result = _controller.Create(project);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Delete_NotValidId_ReturnsBadRequestObjectResult()
        {
            var result = await _controller.Delete(3);
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}