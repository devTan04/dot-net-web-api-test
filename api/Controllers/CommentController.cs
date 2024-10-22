using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interface;
using api.Mappers;
using api.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ApplicationDBContext _context;
    private readonly ICommentRepository _commentRepo;

    public CommentController(ApplicationDBContext context, ICommentRepository commentRepo)
    {
        _context = context;
        _commentRepo = commentRepo;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var comment = await _commentRepo.GetAllAsync();
        var commentDto = comment.Select(s => s.ToCommentDto());
        return Ok(commentDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var comment = await _commentRepo.GetByIdAsync(id);
        if (comment == null)
        {
            return NotFound();
        }
        return Ok(comment.ToCommentDto());
    }

}
