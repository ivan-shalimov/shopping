namespace Shopping.Services.Interfaces
{
    public interface IBackgroundTaskManager
    {
        public void AddTask(Func<IServiceProvider, CancellationToken, Task> taskProvider);

        public Task<Func<IServiceProvider, CancellationToken, Task>> GetNextTaskProvider();
    }
}