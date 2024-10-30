using Microsoft.AspNetCore.Mvc;
using ToDoApp.Dtos;
using ToDoApp.Services;

namespace ToDoApp.Controllers;

[ApiController]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodoController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok(await _todoService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        var todoDto = await _todoService.GetAsyncTodoById(id);
        if (todoDto is null) return NotFound();
        return Ok(todoDto);
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> CreateTodo([FromBody] TodoDto todoDto)
    {
        return Ok(await _todoService.CreateAsyncTodo(todoDto));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TodoDto>> UpdateTodo(string id,[FromBody] TodoDto todoDto)
    {
        return Ok(await _todoService.UpdateAsyncTodo(id,todoDto));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> DeleteTodo(string id)
    {
        return Ok(await _todoService.DeleteAsyncTodo(id));
    }
    
}