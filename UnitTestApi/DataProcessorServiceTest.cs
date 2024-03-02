using wundermanthompson_api.Controllers;
using wundermanthompson_api.services;
using Moq;
using wundermanthompson_api.DTO;
using wundermanthompson_api.persistence;
using wundermanthompson_api.model;
using System.Linq.Expressions;
using wundermanthompson_api.Enums;

namespace UnitTestApi;

 [TestFixture]
public class TestsDataProcessorController
{
    
    private DataProcessorService _dataProcessorService;
    private Mock<IDataJobRepository> _dataJobRepository;
    private Mock<ILinksRepository> _linksRepository;
    private Mock<IResultsRepository> _resultsRepository;

    [Test]
    public async Task GivenDataJob_WhenCreate_ThenReturnsDataJob()
    {
            // Arrange
            var dataJobDto = new DataJobDTO { FilePathToProcess = "test.txt", Name = "Test Job", Links = [] };
            _dataJobRepository.Setup(repo => repo.Create(It.IsAny<DataJob>())).ReturnsAsync(new DataJob { Id = Guid.NewGuid(), FilePathToProcess = "test.txt", Name = "Test Job" });

            // Act
            var result = await _dataProcessorService.Create(dataJobDto);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.Name, Is.EqualTo("Test Job"));
    }

    [Test]
    public async Task GivenDataJobId_GetBackgroundProcessStatus_ThenReturnsAllDataJob()
    {
            // Arrange
            var dataProcessId = Guid.NewGuid();
            _dataJobRepository.Setup(repo => repo.GetById(dataProcessId)).ReturnsAsync( new DataJob { Id = Guid.NewGuid(), FilePathToProcess = "test.txt", Name = "Test Job", Status = DataJobStatus.Processing });

            // Act
            var result = await _dataProcessorService.GetBackgroundProcessStatus(dataJobId: dataProcessId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.EqualTo(DataJobStatus.Processing));
    }

    [Test]
    public async Task GivenDataJobId_GetBackgroundProcessResults_ThenReturnsAllDataJob()
    {
            // Arrange
            var dataProcessId = Guid.NewGuid();
            _resultsRepository.Setup(repo => repo.GetResultsByDataJobId(dataProcessId)).ReturnsAsync(new List<Result> { new Result { DataJobId = dataProcessId, Value = "test.txt" } });

            // Act
            var result = await _dataProcessorService.GetBackgroundProcessResults(dataJobId: dataProcessId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result[0], Is.EqualTo("test.txt"));
    }

    [SetUp]
    public void Setup()
    {
        _dataJobRepository = new Mock<IDataJobRepository>();
        _linksRepository = new Mock<ILinksRepository>();
        _resultsRepository = new Mock<IResultsRepository>();
        _dataProcessorService = new DataProcessorService(_dataJobRepository.Object, _linksRepository.Object, _resultsRepository.Object);
    }

}