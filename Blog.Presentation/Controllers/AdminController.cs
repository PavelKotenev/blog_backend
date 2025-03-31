using Blog.Application.Admin.Posts.Commands.BulkCreate;
using Blog.Application.Admin.Posts.Commands.BulkDelete;
using Blog.Application.Admin.Posts.Commands.BulkUpdate;
using Blog.Application.Admin.Tags.Commands.BulkCreate;
using Blog.Application.Admin.Tags.Commands.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Controllers;

[ApiController]
[Route("admin/")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpPost("post/create")]
    public async Task<ActionResult<Unit>> BulkCreatePosts(
        [FromBody] BulkCreatePostCommand command,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPost("post/delete")]
    public async Task<ActionResult<Unit>> BulkDeletePost(
        [FromBody] BulkDeletePostCommand command,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPut("post/update")]
    public async Task<ActionResult<Unit>> UpdatePost(
        [FromBody] BulkUpdatePostCommand command,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPost("tag/create")]
    public async Task<ActionResult<Unit>> BulkCreateTag(
        [FromBody] BulkCreateTagCommand command,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPost("tag/delete")]
    public async Task<ActionResult<Unit>> BulkDeleteTag(
        [FromBody] BulkDeletePostCommand command,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(command, cancellationToken);
    }

    [HttpPut("tag/update")]
    public async Task<ActionResult<Unit>> UpdateTag(
        [FromBody] UpdateTagCommand command,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(command, cancellationToken);
    }
}