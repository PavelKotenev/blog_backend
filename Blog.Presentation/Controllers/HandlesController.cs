using Blog.Application.Manage.Mv.Commands.CreateAllMv;
using Blog.Application.Manage.Mv.Commands.RefreshAllMv;
using Blog.Application.Manage.Posts.Commands.CreatePostIndex;
using Blog.Application.Manage.Posts.Commands.DeletePostIndex;
using Blog.Application.Manage.Posts.Commands.GenerateFakePosts;
using Blog.Application.Manage.Posts.Commands.TransferPostsToElasticsearch;
using Blog.Application.Manage.Tags.Commands.CreateDefaultTags;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HandlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public HandlesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("posts/transfer-to-elasticsearch")]
    public async Task<ActionResult<Unit>> TransferPostsToElasticAsync(
        [FromQuery] int fromId,
        [FromQuery] int toId,
        CancellationToken cancellationToken)
    {
        var command = new TransferPostsToElasticsearchCommand(fromId, toId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("posts/generate-fake")]
    public async Task<ActionResult<Unit>> GenerateFakePostsAsync(
        [FromQuery] int quantity,
        CancellationToken cancellationToken)
    {
        var command = new GenerateFakePostsCommand(quantity);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("posts/index")]
    public async Task<ActionResult<Unit>> CreatePostIndexAsync(
        CancellationToken cancellationToken)
    {
        var command = new CreatePostIndexCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("posts/index")]
    public async Task<ActionResult<Unit>> DeletePostIndexAsync(
        CancellationToken cancellationToken)
    {
        var command = new DeletePostIndexCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("tags/default")]
    public async Task<ActionResult<Unit>> CreateDefaultTagsAsync(
        CancellationToken cancellationToken)
    {
        var command = new CreateDefaultTagsCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPut("mv/refresh")]
    public async Task<ActionResult<Unit>> RefreshAllMvAsync(
        CancellationToken cancellationToken)
    {
        var command = new RefreshAllMvCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("mv")]
    public async Task<ActionResult<Unit>> CreateAllMvAsync(
        CancellationToken cancellationToken)
    {
        var command = new CreateAllMvCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}