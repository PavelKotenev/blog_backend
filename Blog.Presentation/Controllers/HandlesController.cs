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
    public async Task<ActionResult<string>> TransferPostsToElasticAsync
    (
        [FromQuery] int fromId,
        [FromQuery] int toId
    )
    {
        var command = new TransferPostsToElasticsearchCommand(fromId, toId);
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("generate_fake_posts")]
    public async Task<ActionResult<string>> GenerateFakePostsAsync
    (
        [FromQuery] int quantity
    )
    {
        var command = new GenerateFakePostsCommand(quantity);
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("create_post_index")]
    public async Task<ActionResult<string>> CreatePostIndexAsync()
    {
        var command = new CreatePostIndexCommand();
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("delete_post_index")]
    public async Task<ActionResult<string>> DeleteIndexInfoAsync()
    {
        var command = new DeletePostIndexCommand();
        var response = await mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("create_default_tags")]
    public async Task<ActionResult<string>> CreateDefaultTagsAsync()
    {
        var command = new CreateDefaultTagsCommand();
        var response = await mediator.Send(command);
        return Ok(response);
    }
    
    [HttpGet("refresh_all_mv")]
    public async Task<ActionResult<string>> RefreshAllMv()
    {
        var command = new RefreshAllMvCommand();
        var response = await mediator.Send(command);
        return Ok(response);
    }
    [HttpGet("create_all_mv")]
    public async Task<ActionResult<string>> CreateAllMv()
    {
        var command = new CreateAllMvCommand();
        var response = await mediator.Send(command);
        return Ok(response);
    }
}