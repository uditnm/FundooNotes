using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using RepositoryLayer.FundoContext;
using RepositoryLayer.Migrations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunDoNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly INotesManager manager;
        private readonly IDistributedCache distributedCache;
        private readonly FundoAppContext context;


        public NotesController(INotesManager manager, IDistributedCache distributedCache, FundoAppContext context)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;
            this.context = context;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddNote(NotesCreationModel model)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var note = manager.AddNote(model, UserId);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Added Successfull", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Note not added", Data = note });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("{NotesId}")]
        public ActionResult UpdateNote(int NotesId, NotesUpdateModel model)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var note = manager.UpdateNote(model, NotesId, UserId);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Updated Successfull", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Note not updated", Data = note });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Pin/{NotesId}")]
        public ActionResult PinNote(int NotesId)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var note = manager.PinNote(NotesId, UserId);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Pinned Successfully", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Pin failed", Data = note });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Archive/{NotesId}")]
        public ActionResult ArchiveNote(int NotesId)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var note = manager.ArchiveNote(NotesId, UserId);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note Archived Successfully", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Archive failed", Data = note });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Trash/{NotesId}")]
        public ActionResult TrashNote(int NotesId)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var note = manager.TrashNote(NotesId, UserId);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Note sent to Trash Successfully", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Trash failed", Data = note });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("BackgroundColor/{NotesId}")]
        public ActionResult NoteColor(NotesColorModel model, int NotesId)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var note = manager.NoteColor(model, NotesId, UserId);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Color Change Successful", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Color Change Unsuccessful", Data = note });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("Image/{NotesId}")]
        public ActionResult UploadImage(int NotesId, IFormFile file)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var note = manager.UploadImage(UserId, NotesId, file);
                if (note != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Color Change Successful", Data = note });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Color Change Unsuccessful", Data = note });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var cacheKey = "notesList";
                string serializedNotesList;
                var notes = new List<NotesEntity>();
                var redisNotesList = await distributedCache.GetAsync(cacheKey);
                if (redisNotesList != null)
                {
                    serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                    notes = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
                }
                else
                {
                    notes = manager.GetAllNotes(UserId);
                    serializedNotesList = JsonConvert.SerializeObject(notes);
                    redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisNotesList, options);
                }

                if (notes != null)
                {
                    return Ok(new ResponseModel<List<NotesEntity>> { Status = true, Message = "Notes display Successfull", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<List<NotesEntity>> { Status = false, Message = "Notes display Unsuccessfull", Data = notes });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("{NotesId}")]
        public ActionResult DisplayNotesById(int NotesId)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var notes = manager.GetNoteById(NotesId, UserId);
                if (notes != null)
                {
                    return Ok(new ResponseModel<NotesEntity> { Status = true, Message = "Notes display Successfull", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<NotesEntity> { Status = false, Message = "Notes display Unsuccessfull", Data = notes });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpDelete("{NotesId}")]
        public ActionResult DeleteNote(int NotesId)
        {
            var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
            var delNote = manager.DeleteNote(UserId, NotesId);
            if (delNote)
            {
                return Ok(new ResponseModel<bool> { Status = true, Message = "Delete Successfull" });
            }
            else
            {
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Delete Unsuccessfull" });
            }

        }

        [Authorize]
        [HttpGet("FindNote/{keyword}")]
        public ActionResult FindNotes(string keyword)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var notes = manager.FindNotesByKeyword(keyword, UserId);
                if (notes != null)
                {
                    return Ok(new ResponseModel<Tuple<int, List<NotesEntity>>> { Status = true, Message = "Found Matching Notes", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<Tuple<int, List<NotesEntity>>> { Status = false, Message = "Notes not found" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("FindNote/{Keyword}/{PageSize}/{PageNo}")]
        public ActionResult FindNotesPageSize(string Keyword, int PageSize, int PageNo)
        {
            try
            {
                var UserId = Convert.ToInt32(User.FindFirst("UserId").Value);
                var notes = manager.FindNotesPageSize(Keyword, PageNo, PageSize, UserId);
                if (notes != null)
                {
                    return Ok(new ResponseModel<Tuple<int, List<NotesEntity>>> { Status = true, Message = "Found Matching Notes", Data = notes });
                }
                else
                {
                    return BadRequest(new ResponseModel<Tuple<int, List<NotesEntity>>> { Status = false, Message = "Notes not found" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
