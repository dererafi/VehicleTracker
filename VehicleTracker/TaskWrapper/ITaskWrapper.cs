namespace Location.TaskWrapper;

public interface ITaskWrapper<T>
{
    Task<T> InitAsync();
}