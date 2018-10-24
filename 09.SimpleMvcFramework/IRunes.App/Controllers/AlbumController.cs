namespace IRunes.App.Controllers
{
    using System.Linq;
    using System.Text;
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Action;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;
    using SIS.HTTP.Extensions;
    using ViewModels;
    using ViewModels.Albums;

    public class AlbumController : Controller
    {
        private const string None = "none";
        private const string Inline = "inline";
        private const string NoAlbums = "There are currently no albums.";
        private const string AlbumNameExists = "Album name already exists!";
        private const string NoTracks = "There are currently no tracks.";

        private readonly IAlbumService albumService;
        private readonly ITrackService trackService;

        public AlbumController(IAlbumService albumService, ITrackService trackService)
        {
            this.albumService = albumService;
            this.trackService = trackService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult All()
        {
            var allAlbums = this.albumService.GetAllAlbums();
            var sb = new StringBuilder();
            if (allAlbums.Any())
            {
                foreach (var album in allAlbums)
                {
                    var albumText = $@"<div><h4><a href=/Album/Details?id={album.Id}>{album.Name}</a></h4></div><br/>";
                    sb.AppendLine(albumText);
                }
            }
            else
            {
                sb.Append(NoAlbums);
            }

            this.Model.Data["AlbumAllViewModel"] = new AlbumAllViewModel
            {
                AllAlbums = sb.ToString()
            };
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            this.Model.Data["ErrorViewModel"] = new ErrorViewModel
            {
                DisplayError = None
            };

            return this.View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AlbumCreateViewModel model)
        {
            var name = model.Name.UrlDecode();
            var cover = model.Cover.UrlDecode();

            if (this.albumService.ContainsAlbum(name))
            {
                this.Model.Data["ErrorViewModel"] = new ErrorViewModel
                {
                    DisplayError = Inline,
                    ErrorMessage = AlbumNameExists
                };
                return this.View();
            }

            this.albumService.AddAlbum(name, cover);
            return new RedirectResult("/Album/All");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Details()
        {
            if (!this.Request.QueryData.ContainsKey("id"))
            {
                return new RedirectResult("/Album/All");
            }

            var id = int.Parse(this.Request.QueryData["id"].ToString());
            var album = this.albumService.GetAlbum(id);

            if (album == null)
            {
                return new RedirectResult("/Album/All");
            }

            var allTracks = this.trackService.GetAllTracks(id);
            var sb = new StringBuilder();
            if (allTracks.Any())
            {
                sb.AppendLine("<ol>");
                foreach (var track in allTracks)
                {
                    sb.AppendLine($"<li><div><a href=/Track/Details?albumId={id}&trackId={track.Id}>{track.Name}</a></div></li>");
                }
                sb.Append("</ol>");
            }
            else
            {
                sb.Append(NoTracks);
            }

            this.Model.Data["AlbumDetailsViewModel"] = new AlbumDetailsViewModel
            {
                Cover = album.Cover,
                Name = album.Name,
                Price = $"${this.albumService.GetTotalPrice(id):F2}",
                AlbumId = id,
                AllTracks = sb.ToString()
            };

            return this.View();
        }
    }
}