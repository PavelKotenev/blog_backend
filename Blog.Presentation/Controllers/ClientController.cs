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
    [HttpPost("get_all_categories_posts")]
    public async Task<ActionResult<SearchAllCategoriesResponse>> GetAllCategoriesPosts(
        [FromBody] GetAllCategoriesPostsQuery query)
    {
        var response = await mediator.Send(query);
        return Ok(response);
    }

    [HttpPost("get_posts_by_category")]
    public async Task<ActionResult<GetPostsByCategoryResponse>> GetPostsByCategory(
        [FromBody] GetPostsByCategoryQuery byCategoryQuery)
    {
        var response = await mediator.Send(byCategoryQuery);
        return Ok(response);
    }

    [HttpPost("get_tags_for_picker")]
    public async Task<ActionResult<GetTagsForPickerResponse>> GetTagsForPicker(
        [FromBody] GetTagsForPickerQuery query)
    {
        var response = await mediator.Send(query);
        return Ok(response);
    }
}