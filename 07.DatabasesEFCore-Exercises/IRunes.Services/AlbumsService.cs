namespace IRunes.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Models;

    public class AlbumsService : IAlbumsService
    {
        public Album GetAlbum(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool ContainsAlbum(string name)
        {
            using (var context = new IRunesDbContext())
            {
                var isExist = context
                    .Albums
                    .Any(a => a.Name == name);

                return isExist;
            }
        }

        public void AddAlbum(string name, string cover)
        {
            using (var context = new IRunesDbContext())
            {
                var album = new Album()
                {
                    Name = name,
                    Cover = cover
                };

                context.Albums.Add(album);
                context.SaveChanges();
            }
        }

        public List<Album> GetAllAlbums()
        {
            using (var context = new IRunesDbContext())
            {
                var allAlbums = context.Albums.ToList();

                return allAlbums;
            }
        }
    }
}