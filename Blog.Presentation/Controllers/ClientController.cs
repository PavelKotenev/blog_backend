using Blog.Application.Business.Posts.Queries.GetAllCategoriesPosts;
using Blog.Application.Business.Posts.Queries.GetPostsByCategory;
using Blog.Application.Business.Tags.Queries.GetTagsForPicker;
using Blog.Domain.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Presentation.Controllers;

[ApiController]
[Route("api/client")]
public class ClientController(IMediator mediator) : ControllerBase
{
    [HttpPost("categories")]
    public async Task<ActionResult<SearchAllCategoriesResponse>> GetAllCategoriesPosts(
        [FromBody] GetAllCategoriesPostsQuery query,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost("category")]
    public async Task<ActionResult<GetPostsByCategoryResponse>> GetPostsByCategory(
        [FromBody] GetPostsByCategoryQuery query,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(query, cancellationToken);
    }

    [HttpPost("picker/tags")]
    public async Task<ActionResult<GetTagsForPickerResponse>> GetTagsForPicker(
        [FromBody] GetTagsForPickerQuery query,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(query, cancellationToken);
    }
}