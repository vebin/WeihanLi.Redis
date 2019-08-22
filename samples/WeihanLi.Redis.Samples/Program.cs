﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WeihanLi.Common;
using WeihanLi.Common.Helpers;
using WeihanLi.Common.Logging.Log4Net;

namespace WeihanLi.Redis.Samples
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LogHelper.LogFactory.AddLog4Net();

            IServiceCollection services = new ServiceCollection();
            services.AddRedisConfig(options =>
            {
                options.RedisServers = new[]
                {
                    new RedisServerConfiguration("127.0.0.1"),
                    //new RedisServerConfiguration("127.0.0.1", 16379),
                };
            });
            // custom serializer
            //services.AddSingleton<IDataSerializer, BinaryDataSerializer>();
            DependencyResolver.SetDependencyResolver(services);

            //var cacheClient = DependencyResolver.Current.ResolveService<ICacheClient>();
            //var customSerializerCacheKey = "TestCustomSerializer";
            //cacheClient.Set(customSerializerCacheKey, new PagedListModel<int>()
            //{
            //    Data = new int[0],
            //    PageNumber = 2,
            //    PageSize = 10,
            //    TotalCount = 10,
            //}, TimeSpan.FromMinutes(1));
            //var result = cacheClient.Get<PagedListModel<int>>(customSerializerCacheKey);
            //Console.WriteLine(result.ToJson());

            //var database = DependencyResolver.Current.GetRequiredService<IConnectionMultiplexer>().GetDatabase();
            //var val = database.StringDecrement("test_counter");
            //Console.WriteLine(val);

            try
            {
                var cts = new CancellationTokenSource();
                var task = Task.Delay(3000, cts.Token);
                var task2 = Task.Delay(1000);

                cts.Cancel(true);

                Thread.Sleep(1000);

                Console.WriteLine($"task.IsCompleted:{task.IsCompleted}, task.IsCanceled:{task.IsCanceled}");
                Console.WriteLine($"task2.IsCompleted:{task2.IsCompleted}, task2.IsCanceled:{task2.IsCanceled}");
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"task canceled, ex:{ex}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            ConfigurationChangedEventSample.MainTest();

            Console.ReadLine();
        }
    }
}
