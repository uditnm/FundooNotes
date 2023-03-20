using CommonLayer.Models;
using Microsoft.AspNetCore.Http;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interfaces
{
    public interface INotesManager
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
        public Tuple<int, List<NotesEntity>> FindNotesByKeyword(string keyword, int UserId);
        public Tuple<int, List<NotesEntity>> FindNotesPageSize(string Keyword, int PageNumber, int PageSize, int UserId);
    }
}   
