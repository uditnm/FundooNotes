using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;

namespace FunDoNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController: ControllerBase
    {
        private readonly ICollabManager manager;

        public CollabController(ICollabManager manager)
        {
            this.manager = manager;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddCollaborator(CollabModel model)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var colab = manager.AddCollab(model, UserId);
                if (colab != null)
                {
                    return Ok(new ResponseModel<CollaboratorEntity> { Status=true, Message="Collaborator Added successfully", Data= colab });
                }
                else
                {
                    return BadRequest(new ResponseModel<CollaboratorEntity> { Status = false, Message = "Collaborator Add failed", Data = colab });
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetCollaborators()
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var collab = manager.GetAllCollaborators(UserId);
                if(collab!=null)
                {
                    return Ok(new ResponseModel<List<CollaboratorEntity>> { Status = true, Message = "Collaborators retrieved successfully", Data = collab });
                }
                else
                {
                    return BadRequest(new ResponseModel<List<CollaboratorEntity>> { Status = false, Message = "Collaborator retrieve failed", Data = collab });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpDelete("{CollabId}")]
        public ActionResult DeleteCollaborators(int CollabId)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var collab = manager.DeleteCollaborator(UserId,CollabId);
                if (collab != null)
                {
                    return Ok(new ResponseModel<CollaboratorEntity> { Status = true, Message = "Collaborator deleted successfully" });
                }
                else
                {
                    return BadRequest(new ResponseModel<CollaboratorEntity> { Status = false, Message = "Collaborator delete failed" });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
