using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Txt.Application.Commands;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;

namespace Txt.Application.Tests.Commands;

public class CreateNoteLineCommandTests
{
    private Mock<INotesModuleRepository> _notesModuleRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private Mock<ILogger<CreateNoteLineCommandHandler>> _loggerMock;
    private CreateNoteLineCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _notesModuleRepositoryMock = new Mock<INotesModuleRepository>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CreateNoteLineCommandHandler>>();
        _handler = new CreateNoteLineCommandHandler(_notesModuleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Test]
    public async Task Handle_ShouldCreateNoteLine_WhenValidRequest()
    {
        // Arrange
        var command = new CreateNoteLineCommand { NoteId = 1, Content = "This is a note line.", OrderIndex = 0 };
        var noteLineDto = new NoteLineDto { NoteId = 1, Content = "This is a note line.", OrderIndex = 0 };
        var noteLineEntity = new NoteLine { NoteId = 1, Content = "This is a note line.", OrderIndex = 0 };

        // Setup repository and mapper mocks
        _notesModuleRepositoryMock.Setup(repo => repo.CreateNoteLine(It.IsAny<NoteLine>())).Returns(noteLineEntity);
        _notesModuleRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _mapperMock.Setup(m => m.Map<NoteLineDto>(It.IsAny<NoteLine>())).Returns(noteLineDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(nl => nl, e => e);
        Assert.That(resultValue, Is.InstanceOf<NoteLineDto>(), "Result should be a NoteLineDto");
        var noteLine = resultValue as NoteLineDto;
        Assert.That(noteLineDto.Content, Is.EqualTo(noteLine.Content), "Content should match the provided content");
        Assert.That(noteLineDto.OrderIndex, Is.EqualTo(noteLine.OrderIndex), "OrderIndex should match the provided order index");
        _notesModuleRepositoryMock.Verify(repo => repo.CreateNoteLine(It.IsAny<NoteLine>()), Times.Once);
        _notesModuleRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_ShouldReturnError_WhenExceptionOccurs()
    {
        // Arrange
        var command = new CreateNoteLineCommand { NoteId = 1, Content = "Faulty note line", OrderIndex = 0 };
        var exceptionMessage = "An unexpected error occurred. Please try again later.";

        // Setup to throw an exception when creating a note line
        _notesModuleRepositoryMock.Setup(repo => repo.CreateNoteLine(It.IsAny<NoteLine>())).Throws(new Exception(exceptionMessage));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultValue = result.Match<object?>(nl => nl, e => e);
        Assert.That(resultValue, Is.InstanceOf<Error>(), "Result should be an Error");
        var error = resultValue as Error;
        Assert.That(exceptionMessage, Is.EqualTo(error!.Details), "Error details should match the exception message");
    }
}
