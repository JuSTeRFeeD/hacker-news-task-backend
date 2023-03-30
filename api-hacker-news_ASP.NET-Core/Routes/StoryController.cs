using System;
using System.Collections.Generic;
using System.Linq;
using api_hacker_news_ASP.NET_Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api_hacker_news_ASP.NET_Core.Routes
{
    [ApiController]
    [Route("[controller]")]
    public class StoryController : ControllerBase
    {
        [HttpGet("getNewStories")]
        public async Task<IEnumerable<Story>> GetNewStories()
        {
            List<Story> stories = new ();
            
            try
            {
                using var httpClient = new HttpClient();

                var storiesIdArr = await GetNewStories(httpClient);

                if (storiesIdArr == null)
                    return stories;
                
                var tasks = new List<Task<Story>>();

                for (var  i = 0; i < 120; i++)
                    tasks.Add(GetItem<Story>(httpClient, storiesIdArr[i]));

                await Task.WhenAll(tasks.ToArray());

                foreach (var t in tasks.Where(t => t.Result != null))
                {
                    stories.Add(t.Result);
                    if (stories.Count == 100) break;
                }

                return stories.OrderByDescending(i => i.Time).ToList();
            } 
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }

            return stories.ToArray();
        }

        [HttpGet("getStory")]
        public async Task<Story> GetStory(int id)
        {
            try
            {
                using var httpClient = new HttpClient();
                return await GetItem<Story>(httpClient, id);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return null;
        }

        [HttpGet("getComments")]
        public async Task<IEnumerable<Comment>> GetComments([FromQuery] int[] ids)
        {
            var comments = new List<Comment>();
            try
            {
                using var httpClient = new HttpClient();
                var tasks = ids.Select(GetComment).ToList();
                await Task.WhenAll(tasks);
                comments.AddRange(tasks
                    .Where(t => t.Result != null)
                    .Select(i => i.Result));
                return comments;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return comments;
        }
        
        [HttpGet("getComment")]
        public async Task<Comment> GetComment(int id)
        {
            try
            {
                using var httpClient = new HttpClient();
                return await GetItem<Comment>(httpClient, id);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return null;
        }
        
        private static async Task<int[]> GetNewStories(HttpClient httpClient)
        {
            try
            {
                using var res = await httpClient.GetAsync(Params.ApiUrl + "newstories.json?print=pretty");
                using var content = res.Content;
                
                var data = await content.ReadAsStringAsync();
                if (data == string.Empty)
                {
                    return null;
                }
                return JsonConvert.DeserializeObject<int[]>(data);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            return null;
        }

        private static async Task<T> GetItem<T>(HttpClient client, int id) where T : class
        {
            using var res = await client.GetAsync(Params.ApiUrl + "item/" + id + ".json?print=pretty");
            using var content = res.Content;
            
            var data = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}