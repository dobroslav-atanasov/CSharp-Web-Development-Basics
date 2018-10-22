namespace SIS.SecurityDemo
{
    using Framework;

    public class Launcher
    {
        public static void Main()
        {
            WebHost.Start(new StartUp());
        }
    }
}