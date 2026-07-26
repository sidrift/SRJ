using Microsoft.AspNetCore.Mvc;
using srj.Application.Dtos.Request.Items;
using srj.Application.Dtos.Response.Items;
using srj.Application.Interface.Services.Items;
using srj.Domain.Enums;
using srj.Domain.Models;

namespace TheSRJProject.Controller.Items;

[ApiController]
[Route("api/item-categories")]
public class ItemCategoryController : ControllerBase
{
    private readonly IItemCategoryService _service;

    public ItemCategoryController(IItemCategoryService service)
    {
        _service = service;
    }
    [HttpGet]
    public async Task<ActionResult<List<ItemCategoryResponse>>> GetAll(
        [FromQuery] Metal? metal,
        [FromQuery] string? name)
    {
        var items = await _service.GetAllAsync(metal, name);

        var response = items.Select(x => new ItemCategoryResponse
        {
            Id = x.Id,
            Name = x.Name,
            Metal = x.Metal
        }).ToList();

        return Ok(response);
    }
    [HttpGet("{id:long}")]
    public async Task<ActionResult<ItemCategoryResponse>> GetById(long id)
    {
        var item = await _service.GetByIdAsync(id);

        if (item == null) return NotFound();

        return Ok(new ItemCategoryResponse
        {
            Id = item.Id,
            Name = item.Name,
            Metal = item.Metal
        });
    }

    [HttpPost]
    public async Task<ActionResult<ItemCategoryResponse>> Create(
        CreateItemCategoryRequest request)
    {
        try
        {
            var item = new ItemCategory
            {
                Name = request.Name,
                Metal = request.Metal
            };

            var created = await _service.CreateAsync(item);

            var response = new ItemCategoryResponse
            {
                Id = created.Id,
                Name = created.Name,
                Metal = created.Metal
            };

            return CreatedAtAction(
                nameof(GetById),
                new { id = response.Id },
                response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An unexpected error occurred.",
                detail = ex.Message
            });
        }
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(long id, UpdateItemCategoryRequest request)
    {
        var item = new ItemCategory
        {
            Id = id,
            Name = request.Name
        };
        await _service.UpdateAsync(item);

        return Ok();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            await _service.DeleteAsync(id);

            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new
            {
                message = ex.Message
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }
}