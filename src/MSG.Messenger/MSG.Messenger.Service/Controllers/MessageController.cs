using Microsoft.AspNetCore.Mvc;

namespace MSG.Messenger.Service.Controllers;

[Route("api/chat/{chatId:guid}/messages")]
[ApiController]
public class MessageController : ControllerBase
{

    public MessageController()
    {
        
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send(Guid chatId)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("delete/{messageId:guid}")]
    public async Task<IActionResult> Delete(Guid chatId, Guid messageId) 
    {
        throw new NotImplementedException();
    }

    [HttpPatch("redact/{messageId:guid}")]
    public async Task<IActionResult> Redact(Guid chatId, Guid messageId)
    {
        throw new NotImplementedException();
    }
}