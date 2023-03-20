using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System;

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

    }
}
