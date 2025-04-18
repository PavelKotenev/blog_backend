﻿using Blog.Application.Business.Posts.Queries.CountPostsByCategories;
using Blog.Application.Business.Posts.Queries.GetPostsByCategory;
using Blog.Application.Business.Tags.Queries.GetTagsForPicker;
using Blog.Contracts.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Controllers;

[ApiController]
[Route("api/client")]
public class ClientController(IMediator mediator) : ControllerBase
{
    [HttpPost("post/count")]
    public async Task<ActionResult<CountPostsByCategoriesResponse>> CountPostsByCategories(
        [FromBody] CountPostsByCategoriesQuery query,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost("post/by_category")]
    public async Task<ActionResult<GetPostsByCategoryResponse>> GetPostsByCategory(
        [FromBody] GetPostsByCategoryQuery query,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost("tags/refresh_picker")]
    public async Task<ActionResult<GetTagsForPickerResponse>> GetTagsForPicker(
        [FromBody] GetTagsForPickerQuery query,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(query, cancellationToken);
    }
}