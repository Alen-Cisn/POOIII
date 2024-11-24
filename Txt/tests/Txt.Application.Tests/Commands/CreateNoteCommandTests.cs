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

public class CreateNoteCommandTests
{
    private Mock<INotesModuleRepository> _notesModuleRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<CreateNoteCommandHandler>> _loggerMock;
    private CreateNoteCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _notesModuleRepositoryMock = new Mock<INotesModuleRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateNoteCommandHandler>>();
        _handler = new CreateNoteCommandHandler(_notesModuleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCreateNote_WhenValidRequest()
    {
        // Arrange
        var command = new CreateNoteCommand { Name = "New Note", ParentId = 1 };
        var folderEntity = new Folder { Id = 1, Name = "ParentFolder", Path = "/ParentFolder" };
        var noteDto = new NoteDto { Name = "New Note", Path = "/ParentFolder/New Note", ParentId = 1, Lines = [] };
        var noteEntity = new Note { Name = "New Note", ParentId = 1, Path = "/ParentFolder/New Note", Lines = [] };
        var folders = new List<Folder>()
        {
            folderEntity
        };
        _notesModuleRepositoryMock.Setup(repo => repo.FindFoldersWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Folder, bool>>>()))
            .Returns(folders.BuildMock());
        _notesModuleRepositoryMock.Setup(repo => repo.CreateNote(It.IsAny<Note>())).Returns(noteEntity);
        _notesModuleRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mapperMock.Setup(m => m.Map<NoteDto>(It.IsAny<Note>())).Returns(noteDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(n => n, e => e);
        Assert.That(resultValue, Is.InstanceOf<NoteDto>(), "Result should be a NoteDto");
        var note = resultValue as NoteDto;
        Assert.That(noteDto.Name, Is.EqualTo(note.Name), "Note name should match the provided name");
        _notesModuleRepositoryMock.Verify(repo => repo.CreateNote(It.IsAny<Note>()), Times.Once);
        _notesModuleRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldThrowValidationException_WhenParentFolderDoesNotExist()
    {
        // Arrange
        var command = new CreateNoteCommand { Name = "Child Note", ParentId = 1 };

        _notesModuleRepositoryMock.Setup(repo => repo.FindFoldersWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Folder, bool>>>()))
            .Returns(new List<Folder>().BuildMock()); // Simulate no folder found

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(n => n, e => e);
        Assert.That(resultValue, Is.InstanceOf<Error>(), "Result should be an Error");
        var error = resultValue as Error;
        Assert.That(error!.Details, Is.EqualTo("Given parent folder doesn't exist."), "Error details should match expected message.");
    }

    [Test]
    public async Task Handle_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreateNoteCommand { Name = "Faulty Note", ParentId = -1 };
        var exceptionMessage = "An unexpected error occurred. Please try again later.";

        _notesModuleRepositoryMock.Setup(repo => repo.FindFoldersWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Folder, bool>>>()))
            .Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(n => n, e => e);
        Assert.That(resultValue, Is.InstanceOf<Error>(), "Result should be an Error");
        var error = resultValue as Error;
        Assert.That(exceptionMessage, Is.EqualTo(error!.Details), "Error details should match the exception message");
    }
}
