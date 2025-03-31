using Blog.Application.Manage.Mv.Commands.CreateAllMv;
using Blog.Application.Manage.Mv.Commands.RefreshAllMv;
using Blog.Application.Manage.Posts.Commands.CreatePostIndex;
using Blog.Application.Manage.Posts.Commands.DeletePostIndex;
using Blog.Application.Manage.Posts.Commands.GenerateFakePosts;
using Blog.Application.Manage.Posts.Commands.TransferPostsToElasticsearch;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Controllers;

[ApiController]
[Route("api/handle")]
public class HandlesController(IMediator mediator) : ControllerBase
{
    [HttpPost("posts/transfer-to-elasticsearch")]
    public async Task<ActionResult<Unit>> TransferPostsToElasticAsync(
        [FromBody] TransferPostsToElasticsearchCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("posts/generate-fake")]
    public async Task<ActionResult<Unit>> GenerateFakePostsAsync(
        [FromBody] GenerateFakePostsCommand command,
        CancellationToken cancellationToken
    )
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("posts/index")]
    public async Task<ActionResult<Unit>> CreatePostIndexAsync(
        CancellationToken cancellationToken)
    {
        var command = new CreatePostIndexCommand();
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("posts/index")]
    public async Task<ActionResult<Unit>> DeletePostIndexAsync(
        CancellationToken cancellationToken)
    {
        var command = new DeletePostIndexCommand();
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("mv/refresh")]
    public async Task<ActionResult<Unit>> RefreshAllMvAsync(
        CancellationToken cancellationToken)
    {
        var command = new RefreshAllMvCommand();
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpPost("mv/create")]
    public async Task<ActionResult<Unit>> CreateAllMvAsync(
        CancellationToken cancellationToken)
    {
        var command = new CreateAllMvCommand();
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}