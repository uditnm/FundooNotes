using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using RepositoryLayer.FundoContext;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using CloudinaryDotNet;
using RepositoryLayer.Migrations;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using System.IO;
using CloudinaryDotNet.Actions;

namespace RepositoryLayer.Services
{
    public class NotesRepository: INotesRepository
    {
        private readonly FundoAppContext context;
        private Cloudinary cloudinary;

        
        public NotesRepository(FundoAppContext context, IConfiguration config)
        {
            this.context = context;
            Account account = new Account(
                config["AccountSettings:Cloudname"],
                config["AccountSettings:ApiKey"],
                config["AccountSettings:ApiSecret"]);
            cloudinary = new Cloudinary(account);
        }


        public NotesEntity AddNote(NotesCreationModel model, int UserId)
        {
            try
            {
                NotesEntity notes = new NotesEntity();
                notes.Title = model.Title;
                notes.Description = model.Description;
                notes.Reminder = model.Reminder;
                notes.BackgroundColor = "white";
                notes.Image = model.Image;
                notes.Pin = false;
                notes.Created = DateTime.Now;
                notes.Edited = DateTime.Now;
                notes.Archive = true;
                notes.Trash = false;
                notes.UserId = UserId;
                var check = context.Notes.Add(notes);
                context.SaveChanges();
                if (check != null)
                {
                    return notes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public NotesEntity UpdateNote(NotesUpdateModel model, int NotesId, int UserId)
        {
            try
            {
                var notes = context.Notes.FirstOrDefault(x => x.NotesId == NotesId && x.UserId == UserId);
                if (notes != null)
                {
                    notes.Title = model.Title;
                    notes.Description = model.Description;
                    notes.Reminder = model.Reminder;
                    notes.Image = model.Image;
                    notes.Edited = DateTime.Now;
                    context.SaveChanges();
                    return notes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity PinNote(int NotesId, int UserId)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.NotesId == NotesId && x.UserId == UserId);
                if (note != null)
                {
                    if(note.Pin == false)
                    {
                        note.Pin = true;
                    }
                    else
                    {
                        note.Pin = false;
                    }
                    context.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity ArchiveNote(int NotesId, int UserId)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.NotesId == NotesId && x.UserId == UserId);
                if (note != null)
                {
                    if (note.Archive == false)
                    {
                        note.Archive = true;
                    }
                    else
                    {
                        note.Archive = false;
                    }
                    context.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity TrashNote(int NotesId, int UserId)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.NotesId == NotesId && x.UserId == UserId);
                if (note != null)
                {
                    if (note.Trash == false)
                    {
                        note.Trash = true;
                    }
                    else
                    {
                        note.Trash = false;
                    }
                    context.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity NoteColor(NotesColorModel model, int NotesId, int UserId)
        {
            try
            {
                var note = context.Notes.FirstOrDefault(x => x.NotesId == NotesId && x.UserId == UserId);
                if (note != null)
                { 
                    note.BackgroundColor = model.BackgroundColor;
                    note.Edited = DateTime.Now;
                    context.SaveChanges();
                    return note;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<NotesEntity> GetAllNotes(int UserId)
        {
            try
            {
                var AllNotes = context.Notes.Where(x=>x.UserId==UserId).ToList();
                if (AllNotes.Count> 0)
                {
                    return AllNotes;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public NotesEntity GetNoteById(int id, int UserId)
        {
            try
            {
                var checkNote = context.Notes.FirstOrDefault(o => o.NotesId == id && o.UserId==UserId);
                if (checkNote != null)
                {
                    return checkNote;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public bool DeleteNote(int UserId, int NotesId)
        {
            try
            {
                var checkNote = context.Notes.FirstOrDefault(o => o.NotesId == NotesId && o.UserId == UserId);
                if (checkNote != null)
                {
                    context.Notes.Remove(checkNote);
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NotesEntity UploadImage(int UserId, int NotesId, IFormFile file)
        {
            try
            {
                if(file == null || file.Length == 0)
                {
                    return null;
                }
                var note = context.Notes.FirstOrDefault(x => x.NotesId == NotesId && x.UserId == UserId);
                if (note != null)
                {
                    
                    using (var stream = file.OpenReadStream())
                    {
                        var UploadParams = new ImageUploadParams()
                        {
                            File = new FileDescription(file.FileName, stream)
                        };
                        var uploadResult = cloudinary.Upload(UploadParams);
                        note.Image = uploadResult.Url.ToString();
                        note.Edited = DateTime.Now;
                        context.SaveChanges();
                        return note;
                    }
                    
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}