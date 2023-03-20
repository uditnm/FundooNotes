using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;

namespace FunDoNotesApplication.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LabelController: ControllerBase
    {
        

        private readonly ILabelManager manager;
        public LabelController(ILabelManager manager)
        {
            this.manager = manager;
        }

        [Authorize]
        [HttpPost]

        public ActionResult AddLabel(LabelModel model)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var label = manager.CreateLabel(model,UserId);
                if(label!=null)
                {
                    return Ok(new ResponseModel<LabelEntity> { Status = true, Message = "Label Creation Successful", Data= label });
                }
                else
                {
                    return BadRequest(new ResponseModel<LabelEntity> { Status = false, Message = "Label Creation Unsuccessful" });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet]

        public ActionResult ViewAllLabels()
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var AllLabels = manager.GetLabels(UserId);
                if (AllLabels != null)
                {
                    return Ok(new ResponseModel<List<LabelEntity>> { Status=true, Message ="Labels Displayed Successfully",Data = AllLabels});
                }
                else
                {
                    return BadRequest(new ResponseModel<List<LabelEntity>> { Status = false, Message = "No Labels Found" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet("{NoteId}")]

        public ActionResult ViewLabelsByNote(long NoteId)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var Labels = manager.GetLabelsByNote(NoteId, UserId);
                if (Labels != null)
                {
                    return Ok(new ResponseModel<List<LabelEntity>> { Status = true, Message = "Labels Displayed Successfully", Data = Labels });
                }
                else
                {
                    return BadRequest(new ResponseModel<List<LabelEntity>> { Status = false, Message = "No Labels Found" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpDelete("{LabelId}")]

        public ActionResult Delete(int LabelId)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var NoteDeleted = manager.DeleteLabel(LabelId, UserId);
                if (NoteDeleted)
                {
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Label deleted Successfully", Data = NoteDeleted });
                }
                else
                {
                    return BadRequest(new ResponseModel<bool> { Status = true, Message = "Deletion Unsuccessful" });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
