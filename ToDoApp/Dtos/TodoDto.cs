namespace ToDoApp.Dtos;

public class TodoDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ExpireDate { get; set; }
    public DateTime StartedDate { get; set; }
    public DateTime EndedDate { get; set; }
    public bool IsCompleted { get; set; }
}