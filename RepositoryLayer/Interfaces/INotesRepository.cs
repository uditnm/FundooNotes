using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interfaces
{
    public interface INotesRepository
    {
        public NotesEntity AddNote(NotesCreationModel model, int UserId);
        public NotesEntity UpdateNote(NotesUpdateModel model, int NotesId, int UserId);
        public List<NotesEntity> GetAllNotes(int UserId);
        public NotesEntity GetNoteById(int id, int UserId);
        public bool DeleteNote(int UserId, int NotesId);
        public NotesEntity PinNote(int NotesId, int UserId);
        public NotesEntity ArchiveNote(int NotesId, int UserId);
        public NotesEntity TrashNote(int NotesId, int UserId);
        public NotesEntity NoteColor(NotesColorModel model, int NotesId, int UserId);
        public NotesEntity UploadImage(int UserId, int NotesId, IFormFile file);
    }
}
