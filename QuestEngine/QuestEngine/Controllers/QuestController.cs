
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QuestEngine.Models;
using QuestEngine.Handler;

namespace QuestEngine.Controllers
{
    public class QuestController : ApiController
    {
        // TODO : Refactor to use global exception attributes

        // GET http://localhost:50159/api/state/playerid
        [Route("~/api/state/{PlayerId}")]
        public IHttpActionResult Get(string PlayerId)
        {
            try
            {
                var src = QuestHandler.Instance.GetState(PlayerId);
                return Ok(src);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // POST http://localhost:50159/api/progress
        [Route("~/api/progress")]
        public IHttpActionResult Post([FromBody] ProgressRequest pr)
        {
            try
            {
                var prc = QuestHandler.Instance.GetProgress(pr);
                return Ok(prc);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        // DELETE http://localhost:50159/api/delete/playerid
        [Route("~/api/delete/{PlayerId}")]
        public IHttpActionResult Delete(string PlayerId)
        {
            try
            {
                QuestHandler.Instance.Delete(PlayerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
