using AutoMapper;
using Microsoft.Extensions.Logging;
using MockQueryable;
using Moq;
using NUnit.Framework;
using Txt.Application.Commands;
using Txt.Domain.Entities;
using Txt.Domain.Repositories.Interfaces;
using Txt.Shared.Commands;
using Txt.Shared.Dtos;
using Txt.Shared.ErrorModels;

namespace Txt.Application.Tests.Commands
{
    public class UpdateNoteCommandTests
    {
        private Mock<INotesModuleRepository> _notesModuleRepositoryMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<UpdateNoteCommandHandler>> _loggerMock;
        private UpdateNoteCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _notesModuleRepositoryMock = new Mock<INotesModuleRepository>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateNoteCommandHandler>>();
            _handler = new UpdateNoteCommandHandler(_notesModuleRepositoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task Handle_ShouldUpdateNote_WhenValidRequest()
        {
            // Arrange
            var command = new UpdateNoteCommand { NoteId = 1, Name = "Updated Note", ParentId = 2 };
            var existingNote = new Note { Id = 1, Name = "Old Note", ParentId = 2, Path = "/Old Note", Lines = [] };
            var updatedNoteDto = new NoteDto { Id = 1, Name = "Updated Note", Path = "/NewFolder/Updated Note", Lines = [], ParentId = 2 };
            var folderEntity = new Folder { Id = 2, Path = "/NewFolder", Name = "NewFolder" };
            var folders = new List<Folder>()
            {
                folderEntity
            };
            var notes = new List<Note>()
            {
                existingNote
            };
            _notesModuleRepositoryMock.Setup(repo => repo.FindNotesWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Note, bool>>>()))
                .Returns(notes.BuildMock());
            _notesModuleRepositoryMock.Setup(repo => repo.FindFoldersWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Folder, bool>>>()))
                .Returns(folders.BuildMock());
            _notesModuleRepositoryMock.Setup(repo => repo.UpdateNote(It.IsAny<Note>()));
            _notesModuleRepositoryMock.Setup(repo => repo.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<NoteDto>(It.IsAny<Note>())).Returns(updatedNoteDto);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var resultValue = result.Match<object?>(n => n, e => e);
            Assert.That(resultValue, Is.InstanceOf<NoteDto>(), "Result should be a NoteDto");
            var noteDto = resultValue as NoteDto;
            Assert.That(updatedNoteDto.Name, Is.EqualTo(noteDto.Name), "Updated note name should match the provided name");
            Assert.That(updatedNoteDto.Path, Is.EqualTo(noteDto.Path), "Updated note path should match the expected path");
            _notesModuleRepositoryMock.Verify(repo => repo.UpdateNote(It.IsAny<Note>()), Times.Once);
            _notesModuleRepositoryMock.Verify(repo => repo.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldThrowNotFoundException_WhenNoteDoesNotExist()
        {
            // Arrange
            var command = new UpdateNoteCommand { NoteId = 1, Name = "Nonexistent Note", ParentId = 2 };

            var notes = new List<Note>();
            _notesModuleRepositoryMock.Setup(repo => repo.FindNotesWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Note, bool>>>()))
                .Returns(notes.BuildMock());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var resultValue = result.Match<object?>(n => n, e => e);
            Assert.That(resultValue, Is.InstanceOf<Error>(), "Result should be an Error");
            var error = resultValue as Error;
            Assert.That(error!.Details, Is.EqualTo("Given note doesn't exist."), "Error details should match expected message.");
        }

        [Test]
        public async Task Handle_ShouldThrowValidationException_WhenParentFolderDoesNotExist()
        {
            // Arrange
            var command = new UpdateNoteCommand { NoteId = 1, Name = "Updated Note", ParentId = 2 };
            var existingNote = new Note { Id = 1, Name = "Old Note", ParentId = 3, Lines = [] };

            var notes = new List<Note>()
            {
                existingNote
            };
            var folders = new List<Folder>();
            _notesModuleRepositoryMock.Setup(repo => repo.FindNotesWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Note, bool>>>()))
                .Returns(notes.BuildMock());

            _notesModuleRepositoryMock.Setup(repo => repo.FindFoldersWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Folder, bool>>>()))
                .Returns(folders.BuildMock());

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
            var command = new UpdateNoteCommand { NoteId = 1, Name = "Faulty Update", ParentId = 3 };
            var exceptionMessage = "An unexpected error occurred. Please try again later.";
            var existingNote = new Note { Id = 1, Name = "Old Note", ParentId = 3, Lines = [] };

            var notes = new List<Note>()
            {
                existingNote
            };
            _notesModuleRepositoryMock.Setup(repo => repo.FindNotesWhere(It.IsAny<System.Linq.Expressions.Expression<Func<Note, bool>>>()))
                .Returns(notes.BuildMock());

            _notesModuleRepositoryMock.Setup(repo => repo.UpdateNote(It.IsAny<Note>())).Throws(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            var resultValue = result.Match<object?>(n => n, e => e);
            Assert.That(resultValue, Is.InstanceOf<Error>(), "Result should be an Error");
            var error = resultValue as Error;
            Assert.That(exceptionMessage, Is.EqualTo(error!.Details), "Error details should match the exception message");
        }
    }
}
