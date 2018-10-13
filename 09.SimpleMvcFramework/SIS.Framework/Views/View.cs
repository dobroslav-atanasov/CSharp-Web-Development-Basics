namespace SIS.Framework.Views
{
    using System.IO;
    using ActionResults.Contracts;

    public class View : IRenderable
    {
        private readonly string fullyQualifiedTemplateName;

        public View(string fullyQualifiedTemplateName)
        {
            this.fullyQualifiedTemplateName = fullyQualifiedTemplateName;
        }

        private string ReadFile()
        {
            if (!File.Exists(this.fullyQualifiedTemplateName))
            {
                throw new FileNotFoundException();
            }

            var html = File.ReadAllText(this.fullyQualifiedTemplateName);
            return html;
        }

        public string Render()
        {
            var html = this.ReadFile();
            return html;
        }
    }
}