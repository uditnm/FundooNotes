using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunDoNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollabController: ControllerBase
    {
        private readonly ICollabManager manager;
        private readonly IDistributedCache distributedCache;

        public CollabController(ICollabManager manager, IDistributedCache distributedCache)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;
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
        public async Task<IActionResult> GetCollaborators()
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var cacheKey = "CollabList";
                string serializedCollabList;
                var collab = new List<CollaboratorEntity>();
                var redisCollabList = await distributedCache.GetAsync(cacheKey);
                if (redisCollabList != null)
                {
                    serializedCollabList = Encoding.UTF8.GetString(redisCollabList);
                    collab = JsonConvert.DeserializeObject<List<CollaboratorEntity>>(serializedCollabList);
                }
                else
                {
                    collab = manager.GetAllCollaborators(UserId);
                    serializedCollabList = JsonConvert.SerializeObject(collab);
                    redisCollabList = Encoding.UTF8.GetBytes(serializedCollabList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisCollabList, options);
                }
                if (collab!=null)
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
                if (collab)
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
