namespace IRunes.App.Controllers
{
    using System.Linq;
    using System.Text;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;

    public class AlbumController : Controller
    {
        private const string AllAlbums = "AllAlbums";
        private const string NoAlbums = "There are currently no albums.";

        private readonly IAlbumService albumService;

        public AlbumController(IAlbumService albumService)
        {
            this.albumService = albumService;
        }

        [HttpGet]
        public IActionResult All()
        {
            if (!this.IsLoggedIn())
            {
                return new RedirectResult("/User/Login");
            }

            var allAlbums = this.albumService.GetAllAlbums();
            var sb = new StringBuilder();
            if (allAlbums.Any())
            {
                foreach (var album in allAlbums)
                {
                    var albumText = $@"<div><h4><a href=/albums/details?id={album.Id}>{album.Name}</a></h4></div><br/>";
                    sb.AppendLine(albumText);
                }

                this.Model.Data[AllAlbums] = sb.ToString();
            }
            else
            {
                this.Model.Data[AllAlbums] = NoAlbums;
            }

            return this.View();
        }
    }
}