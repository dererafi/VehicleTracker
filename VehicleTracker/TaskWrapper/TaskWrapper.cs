namespace Location.TaskWrapper;

public class TaskWrapper<T> : ITaskWrapper<T>
{
    private readonly Func<Task<T>> _taskFactory;
    private Task<T>? task;

    public TaskWrapper(Func<Task<T>> taskFactory)
    {
        this._taskFactory = taskFactory;
    }
    
    public async Task<T> InitAsync()
    {
        this.task = this._taskFactory();
        return await this.task;
    }
}
