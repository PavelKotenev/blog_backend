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
[Route("api/handle/")]
public class HandlesController(IMediator mediator) : ControllerBase
{
    [HttpGet("transfer_posts_to_elastic")]
    public async Task<ActionResult<Unit>> TransferPostsToElasticAsync
    (
        [FromQuery] int fromId,
        [FromQuery] int toId,
        CancellationToken cancellationToken
    ) {
        var command = new TransferPostsToElasticsearchCommand(fromId, toId);
        return await mediator.Send(command, cancellationToken);
    }


    [HttpGet("generate_fake_posts")]
    public async Task<ActionResult<Unit>> GenerateFakePostsAsync
    (
        [FromQuery] int quantity,
        CancellationToken cancellationToken
    ) {
        var command = new GenerateFakePostsCommand(quantity);
        return await mediator.Send(command, cancellationToken);
    }


    [HttpGet("create_post_index")]
    public async Task<ActionResult<Unit>> CreatePostIndexAsync(
        CancellationToken cancellationToken
    ) {
        var command = new CreatePostIndexCommand();
        return await mediator.Send(command, cancellationToken);
    }


    [HttpGet("delete_post_index")]
    public async Task<ActionResult<Unit>> DeleteIndexInfoAsync(
        CancellationToken cancellationToken
    ) {
        var command = new DeletePostIndexCommand();
        return await mediator.Send(command, cancellationToken);
    }


    [HttpGet("create_default_tags")]
    public async Task<ActionResult<Unit>> CreateDefaultTagsAsync(
        CancellationToken cancellationToken
    ) {
        var command = new CreateDefaultTagsCommand();
        return await mediator.Send(command, cancellationToken);
    }


    [HttpGet("refresh_all_mv")]
    public async Task<ActionResult<Unit>> RefreshAllMv(
        CancellationToken cancellationToken) {
        var command = new RefreshAllMvCommand();
        return await mediator.Send(command, cancellationToken);
    }


    [HttpGet("create_all_mv")]
    public async Task<ActionResult<Unit>> CreateAllMv(
        CancellationToken cancellationToken
    ) {
        var command = new CreateAllMvCommand();
        return await mediator.Send(command, cancellationToken);
    }
}