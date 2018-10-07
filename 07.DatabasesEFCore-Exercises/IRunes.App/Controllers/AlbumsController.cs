namespace IRunes.App.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Web;
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class AlbumsController : Controller
    {
        private const string AlbumExists = "<h1>Album already exists!</h1>";
        private const string NoAlbums = "There are currently no albums.";
        private const string AlbumDoesNotExist = "Album does not exist!";
        private const string NoTracks = "There are currently no tracks.";

        private readonly IAlbumsService albumsService;
        private readonly ITrackService trackService;

        public AlbumsController()
        {
            this.albumsService = new AlbumsService();
            this.trackService = new TrackService();
        }

        public IHttpResponse All(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var allAlbums = this.albumsService.GetAllAlbums();
            var sb = new StringBuilder();
            if (allAlbums.Any())
            {
                foreach (var album in allAlbums)
                {
                    var albumText = $@"<div><a href=/albums/details?id={album.Id}>{album.Name}</a></div><br/>";
                    sb.AppendLine(albumText);
                }

                this.ViewBag["allAlbums"] = sb.ToString();
            }
            else
            {
                this.ViewBag["allAlbums"] = NoAlbums;
            }

            return this.View();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            this.SetViewBagData();
            return this.View();
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            var name = request.FormData["name"].ToString();
            var cover = request.FormData["cover"].ToString();

            var nameDecode = HttpUtility.UrlDecode(name);
            var coverDecode = HttpUtility.UrlDecode(cover);

            if (this.albumsService.ContainsAlbum(nameDecode))
            {
                this.ApplyError(AlbumExists);
                return this.View();
            }

            this.albumsService.AddAlbum(nameDecode, coverDecode);

            var response = new RedirectResult("/albums/all");
            return response;
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("id"))
            {
                this.ApplyError(AlbumDoesNotExist);
                return new RedirectResult("/albums/all");
            }

            var albumId = int.Parse(request.QueryData["id"].ToString());
            var album = this.albumsService.GetAlbum(albumId);

            if (album == null)
            {
                this.ApplyError(AlbumDoesNotExist);
                return new BadRequestResult(HttpResponseStatusCode.NotFound);
            }

            this.ViewBag["cover"] = album.Cover;
            this.ViewBag["name"] = album.Name;
            this.ViewBag["price"] = $"${this.albumsService.GetTotalPrice(albumId):F2}";
            this.ViewBag["albumId"] = albumId.ToString();

            var allTracks = this.trackService.GetAllTracks();
            var sb = new StringBuilder();
            if (allTracks.Any())
            {
                sb.AppendLine("<ol>");
                foreach (var track in allTracks)
                {
                    var trackText = $@"<li><div><a href=/tracks/details?albumId={albumId}&trackId={track.Id}>{track.Name}</a></div></li><br/>";
                    sb.AppendLine(trackText);
                }
                sb.AppendLine("</ol>");

                this.ViewBag["allTracks"] = sb.ToString();
            }
            else
            {
                this.ViewBag["allTracks"] = NoTracks;
            }

            return this.View();
        }
    }
}