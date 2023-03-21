using CommonLayer.Models;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FunDoNotesApplication.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class LabelController: ControllerBase
    {
        

        private readonly ILabelManager manager;
        private readonly IDistributedCache distributedCache;
        public LabelController(ILabelManager manager, IDistributedCache distributedCache)
        {
            this.manager = manager;
            this.distributedCache = distributedCache;
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

        public async Task<IActionResult> ViewAllLabels()
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var cacheKey = "labelsList";
                string serializedLabelsList;
                var AllLabels = new List<LabelEntity>();
                var redisNotesList = await distributedCache.GetAsync(cacheKey);
                if (redisNotesList != null)
                {
                    serializedLabelsList = Encoding.UTF8.GetString(redisNotesList);
                    AllLabels = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLabelsList);
                }
                else
                {
                    AllLabels = manager.GetLabels(UserId);
                    serializedLabelsList = JsonConvert.SerializeObject(AllLabels);
                    redisNotesList = Encoding.UTF8.GetBytes(serializedLabelsList);
                    var options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                        .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                    await distributedCache.SetAsync(cacheKey, redisNotesList, options);
                }
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

        [Authorize]
        [HttpPut("{LabelId}")]

        public ActionResult EditLabel(int LabelId, string LabelName)
        {
            try
            {
                var UserId = Convert.ToInt64(User.FindFirst("UserId").Value);
                var NewLabel = manager.EditLabel(LabelId, UserId, LabelName);
                if (NewLabel != null)
                {
                    return Ok(new ResponseModel<LabelEntity> { Status = true,Message = "Label Update Successful", Data = NewLabel});
                }
                else
                {
                    return BadRequest(new ResponseModel<LabelEntity> { Status = false, Message = "Label Update Unsuccessful"});
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
