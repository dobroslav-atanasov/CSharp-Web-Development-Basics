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
        public void AddChannel(string name, string description, string tagString, string channelType)
        {
            using (var db = new MishMashDbContext())
            {
                var type = Enum.Parse<Type>(channelType, true);
                var tags = tagString.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToArray();

                var channel = new Channel
                {
                    Name = name,
                    Description = description
                };

                channel.Type = type;

                db.Channels.Add(channel);
                db.SaveChanges();

                foreach (var tagName in tags)
                {
                    var tag = new Tag
                    {
                        Name = tagName,
                        ChannelId = channel.Id
                    };

                    db.Tags.Add(tag);
                    db.SaveChanges();
                }
            }
        }

        public List<Channel> GetAllChannels()
        {
            using (var db = new MishMashDbContext())
            {
                return db.Channels.ToList();
                //var channels = db.UserChannels
                //    .Where(uc => uc.IsFollow == false)
                //    .Select(uc => uc.Channel)
                //    .ToList();


                //return channels;
                //var channelIds = db.Channels
                //    .Select(c => c.Id)
                //    .ToList();


                //var list = new List<Channel>();
                //foreach (var id in channelIds)
                //{

                //}
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

        public Channel GetChannel(int channelId)
        {
            using (var db = new MishMashDbContext())
            {
                var channel = db.Channels
                    .FirstOrDefault(c => c.Id == channelId);

                return channel;
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

        public void FollowChannel(int channelId, int userId)
        {
            using (var db = new MishMashDbContext())
            {
                var userChannel = new UserChannel
                {
                    ChannelId = channelId,
                    UserId = userId,
                    IsFollow = true
                };

                db.UserChannels.Add(userChannel);
                db.SaveChanges();
            }
        }

        public List<Channel> GetAllFollowChannels()
        {
            using (var db = new MishMashDbContext())
            {
                var channels = db.UserChannels
                    .Where(uc => uc.IsFollow)
                    .Select(uc => uc.Channel)
                    .ToList();

                return channels;
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
    }
}