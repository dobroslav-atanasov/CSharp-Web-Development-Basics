namespace IRunes.App.Controllers
{
    using Services.Contracts;
    using SIS.Framework.ActionResults;
    using SIS.Framework.ActionResults.Contracts;
    using SIS.Framework.Attributes.Methods;
    using SIS.Framework.Controllers;
    using SIS.HTTP.Extensions;
    using ViewModels;

    public class TrackController : Controller
    {
        private const string None = "none";
        private const string Inline = "inline";
        private const string DisplayError = "DisplayError";
        private const string ErrorMessage = "ErrorMessage";
        private const string TrackAlreadyExists = "Track already exists!";

        private readonly ITrackService trackService;
        private readonly IAlbumService albumService;

        public TrackController(ITrackService trackService, IAlbumService albumService)
        {
            this.trackService = trackService;
            this.albumService = albumService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!this.IsLoggedIn())
            {
                return new RedirectResult("/User/Login");
            }

            if (!this.Request.QueryData.ContainsKey("albumId"))
            {
                return new RedirectResult("/Album/All");
            }

            var albumId = int.Parse(this.Request.QueryData["albumId"].ToString());
            var album = this.albumService.GetAlbum(albumId);
            if (album == null)
            {
                return new RedirectResult("/Album/All");
            }

            this.Model.Data[DisplayError] = None;
            this.Model.Data["AlbumId"] = albumId;
            this.Model.Data["Action"] = $"/Track/Create?albumId={albumId}";

            return this.View();
        }

        [HttpPost]
        public IActionResult Create(TrackCreateViewModel model)
        {
            if (!this.IsLoggedIn())
            {
                return new RedirectResult("/User/Login");
            }

            if (!this.Request.QueryData.ContainsKey("albumId"))
            {
                return new RedirectResult("/Album/All");
            }

            var albumId = int.Parse(this.Request.QueryData["albumId"].ToString());
            var album = this.albumService.GetAlbum(albumId);
            if (album == null)
            {
                return new RedirectResult("/Album/All");
            }

            var name = model.Name.UrlDecode();
            var link = model.Link.UrlDecode();
            var price = model.Price;

            if (this.trackService.ContainsTrack(name))
            {
                this.Model.Data[DisplayError] = Inline;
                this.Model.Data[ErrorMessage] = TrackAlreadyExists;
                return this.View();
            }

            this.trackService.AddTrack(name, link, price, albumId);

            return new RedirectResult($"/Album/Details?id={albumId}");
        }

        [HttpGet]
        public IActionResult Details()
        {
            if (!this.IsLoggedIn())
            {
                return new RedirectResult("/User/Login");
            }

            if (!this.Request.QueryData.ContainsKey("albumId") || !this.Request.QueryData.ContainsKey("trackId"))
            {
                return new RedirectResult("/Album/All");
            }

            var albumId = int.Parse(this.Request.QueryData["albumId"].ToString());
            var trackId = int.Parse(this.Request.QueryData["trackId"].ToString());

            var album = this.albumService.GetAlbum(albumId);
            var track = this.trackService.GetTrack(trackId);

            if (album == null)
            {
                return new RedirectResult("/Album/All");
            }

            if (track == null)
            {
                return new RedirectResult($"/Album/Details?id={albumId}");
            }

            this.Model.Data["Name"] = track.Name;
            this.Model.Data["Price"] = track.Price;
            this.Model.Data["AlbumId"] = albumId;
            this.Model.Data["Link"] = track.Link;

            return this.View();
        }
    }
}