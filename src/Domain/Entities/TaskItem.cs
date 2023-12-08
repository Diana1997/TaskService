namespace CleanArchitecture.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string MethodName { get; set; }
}