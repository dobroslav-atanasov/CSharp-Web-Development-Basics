namespace MishMash.Services.Contracts
{
    using System.Collections.Generic;
    using Models;

    public interface IChannelService
    {
        void AddChannel(string name, string description, string tagString, string channelType);

        List<Channel> GetAllChannels();

        int GetTotalFollowers(int channelId);

        Channel GetChannel(int channelId);

        string GetAllTags(int channelId);

        void FollowChannel(int channelId, int userId);

        List<Channel> GetAllFollowChannels();

        List<Channel> GetYourChannels(int userId);
    }
}