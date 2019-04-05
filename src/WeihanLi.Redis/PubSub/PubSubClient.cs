﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using WeihanLi.Extensions;

// ReSharper disable once CheckNamespace
namespace WeihanLi.Redis
{
    /// <summary>
    /// Redis订阅消息Model
    /// </summary>
    public class PubSubMessageModel : IPubSubMessage
    {
        /// <summary>
        /// 订阅类型
        /// </summary>
        public string SubscribeType { get; set; }

        /// <summary>
        /// 订阅消息
        /// </summary>
        public string SubscribeMessage { get; set; }
    }

    internal class PubSubClient : BaseRedisClient, IPubSubClient
    {
        private static string GetRealChannelName(string channelName) => $"{RedisManager.RedisConfiguration.ChannelPrefix}{RedisManager.RedisConfiguration.KeySeparator}{channelName}";

        public PubSubClient(ILogger<PubSubClient> logger) : base(logger, new RedisWrapper("PubSub"))
        {
        }

        public bool IsConnected(string channelName = null)
            => channelName.IsNullOrWhiteSpace() ? Wrapper.Subscriber.Value.IsConnected() : Wrapper.Subscriber.Value.IsConnected(GetRealChannelName(channelName));

        public long Publish(string channelName, IPubSubMessage message, CommandFlags flags = CommandFlags.None) => Wrapper.Subscriber.Value.Publish(GetRealChannelName(channelName), Wrapper.Wrap(message), flags);

        public Task<long> PublishAsync(string channelName, IPubSubMessage message, CommandFlags flags = CommandFlags.None) => Wrapper.Subscriber.Value.PublishAsync(GetRealChannelName(channelName), Wrapper.Wrap(message), flags);

        public void Subscribe(string channelName, Action<IPubSubMessage> action, CommandFlags flag = CommandFlags.None) => Wrapper.Subscriber.Value.Subscribe(GetRealChannelName(channelName), (c, v) => action(Wrapper.Unwrap<PubSubMessageModel>(v)));

        public Task SubscribeAsync(string channelName, Action<IPubSubMessage> action, CommandFlags flag = CommandFlags.None) => Wrapper.Subscriber.Value.SubscribeAsync(GetRealChannelName(channelName), (c, v) => action(Wrapper.Unwrap<PubSubMessageModel>(v)));

        public void Unsubscribe(string channelName, CommandFlags flags = CommandFlags.None) => Wrapper.Subscriber.Value.Unsubscribe(GetRealChannelName(channelName), flags: flags);

        public Task UnsubscribeAsync(string channelName, CommandFlags flags = CommandFlags.None) => Wrapper.Subscriber.Value.UnsubscribeAsync(GetRealChannelName(channelName), flags: flags);

        public void UnsubscribeAll(CommandFlags flags = CommandFlags.None) => Wrapper.Subscriber.Value.UnsubscribeAll(flags);

        public Task UnsubscribeAllAsync(CommandFlags flags = CommandFlags.None) => Wrapper.Subscriber.Value.UnsubscribeAllAsync(flags);
    }
}
