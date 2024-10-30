using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using ToDoApp.Dtos;
using ToDoApp.Entities;

namespace ToDoApp.Services;

public interface ITodoService
{
    Task<TodoDto> CreateAsyncTodo(TodoDto todoDto);
    Task<List<TodoDto>> GetAllAsync();
    Task<TodoDto?> GetAsyncTodoById(string id);
    Task<TodoDto?> UpdateAsyncTodo(string id,TodoDto todoDto);
    Task<string> DeleteAsyncTodo(string id);
}

public class TodoService : ITodoService
{
    private readonly IMongoCollection<Todo> _todoCollection;
    private readonly IMapper _mapper;

    public TodoService(IMongoDatabase database, IMapper mapper)
    {
        _todoCollection = database.GetCollection<Todo>("Todos");
        _mapper = mapper;
    }

    public async Task<TodoDto> CreateAsyncTodo(TodoDto todoDto)
    {
        var todo = _mapper.Map<Todo>(todoDto);
        todo.Id = ObjectId.GenerateNewId().ToString();
        await _todoCollection.InsertOneAsync(todo);
        var createdTodo = _mapper.Map<TodoDto>(todo);
        return createdTodo;
    }

    public async Task<List<TodoDto>> GetAllAsync()
    {
        var todoList = await _todoCollection.Find(_ => true).ToListAsync();
        return todoList.Select(todo => _mapper.Map<TodoDto>(todo)).ToList();
    }

    public async Task<TodoDto?> GetAsyncTodoById(string id)
    {
        var todo = await _todoCollection.Find(todo => todo.Id.Equals(id)).FirstOrDefaultAsync();
        return _mapper.Map<TodoDto>(todo);
    }

    public async Task<TodoDto?> UpdateAsyncTodo(string id,TodoDto todoDto)
    {
        var filter = Builders<Todo>.Filter.Eq(t => t.Id, id);

        var update = Builders<Todo>.Update
            .Set(t => t.Title, todoDto.Title)
            .Set(t => t.Description, todoDto.Description)
            .Set(t => t.IsCompleted, todoDto.IsCompleted)
            .Set(t => t.ExpireDate, todoDto.ExpireDate)
            .Set(t => t.StartedDate, todoDto.StartedDate)
            .Set(t => t.EndedDate, todoDto.EndedDate);

        var result = await _todoCollection.UpdateOneAsync(filter, update);

        if (result.ModifiedCount <= 0) return null;
        var updatedTodo = await _todoCollection.Find(filter).FirstOrDefaultAsync();
        return _mapper.Map<TodoDto>(updatedTodo);

    }

    public async Task<string> DeleteAsyncTodo(string id)
    {
        var filter = Builders<Todo>.Filter.Eq(t => t.Id, id);
        try
        {
            await _todoCollection.DeleteOneAsync(filter);
            return "Deleted";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return "Couldn't delete";
        }
    }
}