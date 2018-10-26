namespace Torshia.App
{
    using Controllers;
    using Services;
    using Services.Contracts;
    using SIS.Framework.Api;
    using SIS.Framework.Services;

    public class StartUp : MvcApplication
    {
        public override void ConfigureServices(IDependencyContainer dependencyContainer)
        {
            dependencyContainer.RegisterDependency<HomeController, HomeController>();
            dependencyContainer.RegisterDependency<UsersController, UsersController>();
            dependencyContainer.RegisterDependency<TasksController, TasksController>();
            dependencyContainer.RegisterDependency<ReportersController, ReportersController>();

            dependencyContainer.RegisterDependency<IUserService, UserService>();
            dependencyContainer.RegisterDependency<ITaskService, TaskService>();
        }
    }
}