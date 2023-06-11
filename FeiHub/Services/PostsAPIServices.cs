using FeiHub.Models;
using FeiHub.Views;
using Microsoft.Xaml.Behaviors.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FeiHub.Services
{
    public class PostsAPIServices
    {
        private readonly HttpClient httpClient;

        public PostsAPIServices()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:8083/apipostsfeihub");
        }
        public async Task<List<Posts>> GetPostsWithoutFollowings(string target)
        {
            try
            {
                string apiUrl = $"/posts/postsTarget/{target}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, httpClient.BaseAddress + apiUrl);
                request.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<Posts> postList = new List<Posts>();

                if (response.IsSuccessStatusCode)
                {
                    JArray jsonArray = JArray.Parse(jsonResponse);
                    foreach (JToken jsonToken in jsonArray)
                    {
                        JObject jsonObject = (JObject)jsonToken;
                        Posts post = new Posts();
                        post.id = jsonObject.GetValue("id").ToString();
                        post.title = jsonObject.GetValue("title").ToString();
                        post.author = jsonObject.GetValue("author").ToString();
                        post.body = jsonObject.GetValue("body").ToString();
                        post.dateOfPublish = DateTime.Parse(jsonObject.GetValue("dateOfPublish").ToString());

                        JArray photosArray = jsonObject.GetValue("photos") as JArray;
                        if (photosArray != null)
                        {
                            post.photos = photosArray.ToObject<Photo[]>();
                        }

                        post.target = jsonObject.GetValue("target").ToString();
                        post.likes = int.Parse(jsonObject.GetValue("likes").ToString());
                        post.dislikes = int.Parse(jsonObject.GetValue("dislikes").ToString());

                        JArray commentsArray = jsonObject.GetValue("comments") as JArray;
                        if (commentsArray != null)
                        {
                            post.comments = commentsArray.ToObject<Comment[]>();
                        }

                        post.StatusCode = response.StatusCode;

                        postList.Add(post);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Posts post = new Posts();
                    post.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    postList.Add(post);
                }

                return postList;
            }
            catch
            {
                List<Posts> postList = new List<Posts>();
                Posts post = new Posts();
                post.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                postList.Add(post);
                return postList;
            }
        }
        public async Task<List<Posts>> GetPostsByTarget(List<User> followings, string target)
        {
            try
            {
                string apiUrl = "/posts/principalPostsTarget";

                var authors = followings.Select(user => user.username).ToList();
                authors.Add(SingletonUser.Instance.Username);
                var requestData = new
                {
                    authors = authors,
                    target = target
                };

                string jsonRequest = JsonConvert.SerializeObject(requestData);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                content.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress + apiUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<Posts> postList = new List<Posts>();

                if (response.IsSuccessStatusCode)
                {
                    JArray jsonArray = JArray.Parse(jsonResponse);
                    foreach (JToken jsonToken in jsonArray)
                    {
                        JObject jsonObject = (JObject)jsonToken;
                        Posts post = new Posts();
                        post.id = jsonObject.GetValue("id").ToString();
                        post.title = jsonObject.GetValue("title").ToString();
                        post.author = jsonObject.GetValue("author").ToString();
                        post.body = jsonObject.GetValue("body").ToString();
                        post.dateOfPublish = DateTime.Parse(jsonObject.GetValue("dateOfPublish").ToString());

                        JArray photosArray = jsonObject.GetValue("photos") as JArray;
                        if (photosArray != null)
                        {
                            post.photos = photosArray.ToObject<Photo[]>();
                        }

                        post.target = jsonObject.GetValue("target").ToString();
                        post.likes = int.Parse(jsonObject.GetValue("likes").ToString());
                        post.dislikes = int.Parse(jsonObject.GetValue("dislikes").ToString());

                        JArray commentsArray = jsonObject.GetValue("comments") as JArray;
                        if (commentsArray != null)
                        {
                            post.comments = commentsArray.ToObject<Comment[]>();
                        }

                        post.StatusCode = response.StatusCode;

                        postList.Add(post);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Posts post = new Posts();
                    post.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    postList.Add(post);
                }

                return postList;
            }
            catch
            {
                List<Posts> postList = new List<Posts>();
                Posts post = new Posts();
                post.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                postList.Add(post);
                return postList;
            }
        }
        public async Task<List<Posts>> GetPrincipalPosts(List<User> followings, string target)
        {
            try
            {
                string apiUrl = "/posts/principalPosts";
                var authors = followings.Select(user => user.username).ToList();
                var requestData = new
                {
                    authors = authors,
                    target = target,
                    author = SingletonUser.Instance.Username
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                content.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress + apiUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<Posts> postList = new List<Posts>();

                if (response.IsSuccessStatusCode)
                {
                    JArray jsonArray = JArray.Parse(jsonResponse);
                    foreach (JToken jsonToken in jsonArray)
                    {
                        JObject jsonObject = (JObject)jsonToken;
                        Posts post = new Posts();
                        post.id = jsonObject.GetValue("id").ToString();
                        post.title = jsonObject.GetValue("title").ToString();
                        post.author = jsonObject.GetValue("author").ToString();
                        post.body = jsonObject.GetValue("body").ToString();
                        post.dateOfPublish = DateTime.Parse(jsonObject.GetValue("dateOfPublish").ToString());

                        JArray photosArray = jsonObject.GetValue("photos") as JArray;
                        if (photosArray != null)
                        {
                            post.photos = photosArray.ToObject<Photo[]>();
                        }

                        post.target = jsonObject.GetValue("target").ToString();
                        post.likes = int.Parse(jsonObject.GetValue("likes").ToString());
                        post.dislikes = int.Parse(jsonObject.GetValue("dislikes").ToString());

                        JArray commentsArray = jsonObject.GetValue("comments") as JArray;
                        if (commentsArray != null)
                        {
                            post.comments = commentsArray.ToObject<Comment[]>();
                        }

                        post.StatusCode = response.StatusCode;

                        postList.Add(post);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Posts post = new Posts();
                    post.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    postList.Add(post);
                }

                return postList;
            }
            catch
            {
                List<Posts> postList = new List<Posts>();
                Posts post = new Posts();
                post.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                postList.Add(post);
                return postList;
            }
        }

        public async Task<Posts> CreatePost(Posts newPosts)
        {
            Posts post = new Posts();
            try
            {
                string apiUrl = "/posts/createPost";
                string jsonRequest = JsonConvert.SerializeObject(newPosts);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                content.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress + apiUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    post.id = jsonObject.GetValue("id").ToString();
                    post.title = jsonObject.GetValue("title").ToString();
                    post.author = jsonObject.GetValue("author").ToString();
                    post.body = jsonObject.GetValue("body").ToString();
                    post.dateOfPublish = DateTime.Parse(jsonObject.GetValue("dateOfPublish").ToString());
                    JArray photosArray = jsonObject.GetValue("photos") as JArray;
                    if (photosArray != null)
                    {
                        post.photos = photosArray.ToObject<Photo[]>();
                    }
                    post.target = jsonObject.GetValue("target").ToString();
                    post.likes = int.Parse(jsonObject.GetValue("likes").ToString());
                    post.dislikes = int.Parse(jsonObject.GetValue("dislikes").ToString());
                    JArray commentsArray = jsonObject.GetValue("comments") as JArray;
                    if (commentsArray != null)
                    {
                        post.comments = commentsArray.ToObject<Comment[]>();
                    }

                    post.StatusCode = response.StatusCode;

                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    post.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                }
                return post;
            }
            catch
            {
                Posts errorPost = new Posts();
                errorPost.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return errorPost;
            }
        }
        public async Task<List<Posts>> GetPostsByUsername(string username)
        {
            try
            {
                string apiUrl = $"/posts/postsAuthor/{username}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, httpClient.BaseAddress + apiUrl);
                request.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                List<Posts> postList = new List<Posts>();

                if (response.IsSuccessStatusCode)
                {
                    JArray jsonArray = JArray.Parse(jsonResponse);
                    foreach (JToken jsonToken in jsonArray)
                    {
                        JObject jsonObject = (JObject)jsonToken;
                        Posts post = new Posts();
                        post.id = jsonObject.GetValue("id").ToString();
                        post.title = jsonObject.GetValue("title").ToString();
                        post.author = jsonObject.GetValue("author").ToString();
                        post.body = jsonObject.GetValue("body").ToString();
                        post.dateOfPublish = DateTime.Parse(jsonObject.GetValue("dateOfPublish").ToString());

                        JArray photosArray = jsonObject.GetValue("photos") as JArray;
                        if (photosArray != null)
                        {
                            post.photos = photosArray.ToObject<Photo[]>();
                        }

                        post.target = jsonObject.GetValue("target").ToString();
                        post.likes = int.Parse(jsonObject.GetValue("likes").ToString());
                        post.dislikes = int.Parse(jsonObject.GetValue("dislikes").ToString());

                        JArray commentsArray = jsonObject.GetValue("comments") as JArray;
                        if (commentsArray != null)
                        {
                            post.comments = commentsArray.ToObject<Comment[]>();
                        }

                        post.StatusCode = response.StatusCode;

                        postList.Add(post);
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    Posts post = new Posts();
                    post.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    postList.Add(post);
                }

                return postList;
            }
            catch
            {
                List<Posts> postList = new List<Posts>();
                Posts post = new Posts();
                post.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                postList.Add(post);
                return postList;
            }
        }
        
        public async Task<HttpResponseMessage> DeletePost(Posts postToDelete)
        {
            try
            {
                string apiUrl = $"/posts/deletePost/{postToDelete.id}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, httpClient.BaseAddress + apiUrl);
                request.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }
        }

        public async Task<HttpResponseMessage> EditPost(Posts newPosts)
        {
            try
            {
                string apiUrl = "/posts/editPost";
                string jsonRequest = JsonConvert.SerializeObject(newPosts);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                content.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.PutAsync(httpClient.BaseAddress + apiUrl, content);
                return response;
            }
            catch (Exception ex)
            {
                HttpResponseMessage response = new HttpResponseMessage();
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }
        }
        
        public async Task<Chats> GetChatByUsername(string username)
        {
            Chats chat = new Chats();
            try
            {
                string apiUrl = "/chats/getChat";
                var requestData = new
                {
                    usernameFrom = SingletonUser.Instance.Username,
                    usernameTo = username
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                content.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress + apiUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    JArray chatFound = jsonObject.GetValue("chat") as JArray;
                    if (chatFound != null)
                    {
                        chat.chats = chatFound.ToObject<Chats.Chat[]>();
                    }
                }
                chat.StatusCode = response.StatusCode;
            }
            catch
            {
                chat.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return chat;
        }
        public async Task<Chats> SendMessage(Chats.Chat messageToSend, string username)
        {
            Chats chat = new Chats();
            try
            {
                string apiUrl = "/chats/addNewMessage";
                var requestData = new
                {
                    usernameFrom = SingletonUser.Instance.Username,
                    usernameTo = username,
                    newMessage = messageToSend.Message,
                    date = messageToSend.DateOfMessageString
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                content.Headers.Add("token", SingletonUser.Instance.Token);

                HttpResponseMessage response = await httpClient.PutAsync(httpClient.BaseAddress + apiUrl, content);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    JObject jsonObject = JObject.Parse(jsonResponse);

                    JArray participants = jsonObject.GetValue("participants") as JArray;
                    if(participants != null)
                    {
                        chat.participants = participants.ToObject<Chats.Participant[]>();
                    }

                    JArray messages = jsonObject.GetValue("messages") as JArray;
                    if (messages != null)
                    {
                        chat.messages = messages.ToObject<Chats.Message[]>();
                    }
                }
                chat.StatusCode = response.StatusCode;
            }
            catch
            {
                chat.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return chat;
        }
    }
}
