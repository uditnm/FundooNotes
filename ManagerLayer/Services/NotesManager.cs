using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class NotesManager: INotesManager
    {
        private readonly INotesRepository repository;

        public NotesManager(INotesRepository repository)
        {
            this.repository = repository;
        }

        public NotesEntity AddNote(NotesCreationModel model, int UserId)
        {
            return repository.AddNote(model, UserId);
        }

        public NotesEntity UpdateNote(NotesUpdateModel model, int NotesId, int UserId)
        {
            return repository.UpdateNote(model, NotesId, UserId);
        }

        public List<NotesEntity> GetAllNotes(int UserId)
        {
            return repository.GetAllNotes(UserId);
        }

        public NotesEntity GetNoteById(int id, int UserId)
        {
            return repository.GetNoteById(id, UserId);
        }

        public bool DeleteNote(int UserId, int NotesId)
        {
            return repository.DeleteNote(UserId, NotesId);
        }

        public NotesEntity PinNote(int NotesId, int UserId)
        {
            return repository.PinNote(NotesId, UserId);
        }

        public NotesEntity ArchiveNote(int NotesId, int UserId)
        {
            return repository.ArchiveNote(NotesId, UserId);
        }

        public NotesEntity TrashNote(int NotesId, int UserId)
        {
            return repository.TrashNote(NotesId, UserId);
        }

        public NotesEntity NoteColor(NotesColorModel model, int NotesId, int UserId)
        {
            return repository.NoteColor(model, NotesId, UserId);
        }

        public NotesEntity UploadImage(int UserId, int NotesId, IFormFile file)
        {
            return repository.UploadImage(UserId, NotesId, file);
        }
    }
}
