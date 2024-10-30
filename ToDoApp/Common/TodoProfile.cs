using AutoMapper;
using ToDoApp.Dtos;
using ToDoApp.Entities;

namespace ToDoApp.Common;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        CreateMap<Todo, TodoDto>();
        CreateMap<TodoDto, Todo>();
    }
}