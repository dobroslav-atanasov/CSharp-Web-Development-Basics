namespace MishMash.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Type = Models.Enums.Type;

    public class ChannelService : IChannelService
    {
        public Channel AddChannel(string name, string description, string tagsToString, string type)
        {
            using (var db = new MishMashDbContext())
            {
                var channel = new Channel
                {
                    Name = name,
                    Description = description,
                    Type = Enum.Parse<Type>(type)
                };

                db.Channels.Add(channel);
                db.SaveChanges();

                var tags = tagsToString.Split(new[] {' ', ',', ';'}, StringSplitOptions.RemoveEmptyEntries);
                var list = new List<Tag>();
                foreach (var tagString in tags)
                {
                    var tag = new Tag
                    {
                        Name = tagString,
                        ChannelId = channel.Id
                    };
                    list.Add(tag);
                }

                db.Tags.AddRange(list);
                db.SaveChanges();

                return channel;
            }
        }

        public int GetTotalFollowers(int channelId)
        {
            using (var db = new MishMashDbContext())
            {
                var followers = db.UserChannels
                    .Where(c => c.ChannelId == channelId)
                    .Count();

                return followers;
            }
        }

        public List<Channel> GetYourChannels(int userId)
        {
            using (var db = new MishMashDbContext())
            {
                var channels = db.UserChannels
                    .Where(uc => uc.UserId == userId)
                    .Include(uc => uc.Channel)
                    .Select(uc => uc.Channel)
                    .ToList();

                return channels;
            }
        }

        public Channel GetChannelById(int channelId)
        {
            using (var db = new MishMashDbContext())
            {
                return db.Channels.FirstOrDefault(c => c.Id == channelId);
            }
        }

        public string GetAllTags(int channelId)
        {
            using (var db = new MishMashDbContext())
            {
                var tags = db.Tags
                    .Where(t => t.ChannelId == channelId)
                    .ToList();

                var list = new List<string>();
                foreach (var tag in tags)
                {
                    list.Add(tag.Name);
                }

                return string.Join(", ", list);
            }
        }
    }
}