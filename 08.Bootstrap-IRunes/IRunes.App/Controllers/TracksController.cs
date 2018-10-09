namespace IRunes.App.Controllers
{
    using System.Web;
    using Extensions;
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class TracksController : Controller
    {
        private const string AlbumDoesNotExist = "Album does not exist!";
        private const string InvalidData = "Invalid data!";
        private const string TrackAlreadyExists = "Track already exists!";
        private const string TrackDoesNotExist = "Track does not exist!";

        private readonly IAlbumsService albumsService;
        private readonly ITrackService trackService;

        public TracksController()
        {
            this.albumsService = new AlbumsService();
            this.trackService = new TrackService();
        }

        public IHttpResponse Create(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                return new RedirectResult("/albums/all");
            }

            var albumId = request.QueryData["albumId"].ToString();
            this.ViewBag["StartForm"] = $"<form method=\"post\" action=\"/tracks/create?albumId={albumId}\">";
            this.ViewBag["EndForm"] = "</form>";

            return this.NewView("create", this.ViewBag);
        }

        public IHttpResponse DoCreate(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId"))
            {
                this.ApplyError(AlbumDoesNotExist);
                return new RedirectResult("/albums/all");
            }

            var albumId = int.Parse(request.QueryData["albumId"].ToString());
            var album = this.albumsService.GetAlbum(albumId);

            if (album == null)
            {
                this.ApplyError(AlbumDoesNotExist);
                return new BadRequestResult(HttpResponseStatusCode.NotFound);
            }

            var name = request.FormData["name"].ToString().UrlDecode();
            var link = request.FormData["link"].ToString().UrlDecode();
            var price = decimal.Parse(request.FormData["price"].ToString());


            if (name.Length <= 2 || link.Length <= 1 || price <= 0)
            {
                this.ApplyError(InvalidData);
                return this.View();
            }

            if (this.trackService.ContainsTrack(name))
            {
                this.ApplyError(TrackAlreadyExists);
                return this.View();
            }

            this.trackService.AddTrack(name, link, price, albumId);

            var response = new RedirectResult($"/albums/details?id={albumId}");
            return response;
        }

        public IHttpResponse Details(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            if (!request.QueryData.ContainsKey("albumId") || !request.QueryData.ContainsKey("trackId"))
            {
                this.ViewBag["Error"] = AlbumDoesNotExist;
                return this.NewView("error", this.ViewBag);
                //this.ApplyError(AlbumDoesNotExist);
                //return new RedirectResult("/albums/all");
            }

            var albumId = int.Parse(request.QueryData["albumId"].ToString());
            var trackId = int.Parse(request.QueryData["trackId"].ToString());

            var track = this.trackService.GetTrack(trackId);

            if (track == null)
            {
                this.ViewBag["Error"] = TrackDoesNotExist;
                return this.NewView("error", this.ViewBag);
                //this.ApplyError(TrackDoesNotExist);
                //return new BadRequestResult(HttpResponseStatusCode.NotFound);
            }

            this.ViewBag["name"] = track.Name;
            this.ViewBag["price"] = $"${track.Price}";
            this.ViewBag["albumId"] = albumId.ToString();
            this.ViewBag["link"] = track.Link;

            return this.View();
        }
    }
}