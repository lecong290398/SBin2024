﻿using System;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc;
using DTKH2024.SbinSolution.Chat;
using DTKH2024.SbinSolution.Storage;
using DTKH2024.SbinSolution.Web.Controllers;

namespace DTKH2024.SbinSolution.Web.Areas.AppAreaName.Controllers
{
    [Area("AppAreaName")]
    [AbpMvcAuthorize]
    public class ChatController : ChatControllerBase
    {
        public ChatController(IBinaryObjectManager binaryObjectManager, IChatMessageManager chatMessageManager) :
            base(binaryObjectManager, chatMessageManager)
        {
        }

        public async Task<ActionResult> GetImage(int id, string contentType)
        {
            var message = await ChatMessageManager.FindMessageAsync(id, AbpSession.GetUserId());
            var jsonMessage = JsonNode.Parse(message.Message.Substring("[image]".Length));
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var fileObject = await BinaryObjectManager.GetOrNullAsync(Guid.Parse(jsonMessage["id"].ToString()));
                if (fileObject == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                return File(fileObject.Bytes, contentType);
            }
        }

        public async Task<ActionResult> GetFile(int id, string contentType)
        {
            var message =await ChatMessageManager.FindMessageAsync(id, AbpSession.GetUserId());
            var jsonMessage = JsonNode.Parse(message.Message.Substring("[file]".Length));
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                var fileObject = await BinaryObjectManager.GetOrNullAsync(Guid.Parse(jsonMessage["id"].ToString()));
                if (fileObject == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound);
                }

                return File(fileObject.Bytes, contentType);
            }
        }

        public PartialViewResult AddFriendModal()
        {
            return PartialView("_AddFriendModal");
        }

        public PartialViewResult AddFromDifferentTenantModal()
        {
            return PartialView("_AddFromDifferentTenantModal");
        }
    }
}
