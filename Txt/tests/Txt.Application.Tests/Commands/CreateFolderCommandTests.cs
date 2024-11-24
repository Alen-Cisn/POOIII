using AutoMapper;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using Txt.Application.Commands;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;

namespace Txt.Application.Tests.Commands;

public class CreateFolderCommandTests
{
    private Mock<INotesModuleRepository> _notesModuleRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<CreateFolderCommandHandler>> _loggerMock;
    private CreateFolderCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _notesModuleRepositoryMock = new Mock<INotesModuleRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateFolderCommandHandler>>();
        _handler = new CreateFolderCommandHandler(_notesModuleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCreateFolder_WhenValidRequest()
    {
        // Arrange
        var command = new CreateFolderCommand { Name = "New Folder", ParentId = null };
        var folderDto = new FolderDto { Name = "New Folder", Path = "/New Folder" };
        var folderEntity = new Folder { Name = "New Folder", Path = "/New Folder" };
        _notesModuleRepositoryMock.Setup(repo => repo.CreateFolder(It.IsAny<Folder>())).Returns(folderEntity);
        _notesModuleRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mapperMock.Setup(m => m.Map<FolderDto>(It.IsAny<Folder>())).Returns(folderDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(f => f, e => e);
        Assert.That(resultValue, Is.InstanceOf<FolderDto>(), "Result should be a FolderDto");
        var folder = resultValue as FolderDto;
        Assert.That(folderDto.Name, Is.EqualTo(folder.Name), "Folder name should be the same as the provided name");
        _notesModuleRepositoryMock.Verify(repo => repo.CreateFolder(It.IsAny<Folder>()), Times.Once);
        _notesModuleRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldThrowValidationException_WhenParentFolderDoesNotExist()
    {
        // Arrange
        var command = new CreateFolderCommand { Name = "Child Folder", ParentId = 1 };

        _notesModuleRepositoryMock.Setup(repo => repo.FindFoldersWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Folder, bool>>>()))
            .Returns(new List<Folder>().BuildMock());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(f => f, e => e);
        Assert.That(resultValue, Is.InstanceOf<Error>(), "Result should be an Error");
        var error = resultValue as Error;
        Assert.That(error!.Details, Is.EqualTo("Given parent folder doesn't exist."), "Error details should be the same as the expected error message.");
    }


    [Test]
    public async Task Handle_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreateFolderCommand { Name = "Faulty Folder", ParentId = null };
        var exceptionMessage = "An unexpected error occurred. Please try again later.";

        _notesModuleRepositoryMock.Setup(repo => repo.CreateFolder(It.IsAny<Folder>())).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(f => f, e => e);
        Assert.That(resultValue, Is.InstanceOf<Error>(), "Result should be an Error");
        var error = resultValue as Error;
        Assert.That(exceptionMessage, Is.EqualTo(error!.Details), "Error details should be the same as the exception message");
    }

}