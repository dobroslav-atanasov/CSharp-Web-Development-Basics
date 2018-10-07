namespace IRunes.App.Controllers
{
    using System.Linq;
    using System.Text;
    using System.Web;
    using Services;
    using Services.Contracts;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class AlbumsController : Controller
    {
        private const string AlbumExists = "<h1>Album already exists!</h1>";
        private const string NoAlbums = "There are currently no albums.";

        private readonly IAlbumsService albumsService;

        public AlbumsController()
        {
            this.albumsService = new AlbumsService();
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
                    var albumText = $@"<div><a href=/albums/create>{album.Name}</a></div><br/>";
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
    }
}