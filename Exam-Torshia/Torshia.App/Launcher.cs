namespace Torshia.App
{
    using SIS.Demo;
    using SIS.Framework;

    public class Launcher
    {
        public static void Main()
        {
            WebHost.Start(new StartUp());
        }
    }
}