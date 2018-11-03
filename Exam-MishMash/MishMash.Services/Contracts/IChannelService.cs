namespace MishMash.Services.Contracts
{
    using System.Collections.Generic;
    using Models;

    public interface IChannelService
    {
        Channel AddChannel(string name, string description, string tagsToString, string type);

        int GetTotalFollowers(int channelId);

        List<Channel> GetYourChannels(int userId);

        Channel GetChannelById(int channelId);

        string GetAllTags(int channelId);
    }
}