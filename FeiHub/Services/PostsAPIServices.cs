﻿using FeiHub.Models;
using FeiHub.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
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
        public async Task<Posts> AddComment(Comment newComment, string idPost)
        {
            Posts post = new Posts();
            try
            {
                string apiUrl = "/posts/addComment";
                var requestData = new
                {
                    author = newComment.author,
                    body = newComment.body,
                    dateOfComment = newComment.dateOfComment,
                    idPost = idPost
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
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
        public async Task<Posts> EditComment(Comment newComment, string idPost)
        {
            Posts post = new Posts();
            try
            {
                string apiUrl = "/posts/editComment";
                var requestData = new
                {
                    postId = idPost,
                    commentId = newComment.commentId,
                    newBody = newComment.body
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                StringContent content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                content.Headers.Add("token", SingletonUser.Instance.Token);
                HttpResponseMessage response = await httpClient.PutAsync(httpClient.BaseAddress + apiUrl, content);
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
        public async Task<Posts> DeleteComment(string commentId, string idPost)
        {
            Posts post = new Posts();
            try
            {
                string apiUrl = "/posts/deleteComment";
                var requestData = new
                {
                    postId = idPost,
                    commentId = commentId
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, httpClient.BaseAddress + apiUrl);
                request.Headers.Add("token", SingletonUser.Instance.Token);
                request.Content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    JObject jsonObject = JObject.Parse(jsonResponse);
                    JObject postObject = jsonObject.GetValue("post") as JObject;
                    if (postObject != null)
                    {
                        post.id = postObject.GetValue("id").ToString();
                        post.title = postObject.GetValue("title").ToString();
                        post.author = postObject.GetValue("author").ToString();
                        post.body = postObject.GetValue("body").ToString();
                        post.dateOfPublish = DateTime.Parse(postObject.GetValue("dateOfPublish").ToString());
                        JArray photosArray = postObject.GetValue("photos") as JArray;
                        if (photosArray != null)
                        {
                            post.photos = photosArray.ToObject<Photo[]>();
                        }
                        post.target = postObject.GetValue("target").ToString();
                        post.likes = int.Parse(postObject.GetValue("likes").ToString());
                        post.dislikes = int.Parse(postObject.GetValue("dislikes").ToString());
                        JArray commentsArray = postObject.GetValue("comments") as JArray;
                        if (commentsArray != null)
                        {
                            post.comments = commentsArray.ToObject<Comment[]>();
                        }
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


        public async Task<HttpResponseMessage> AddLike(string postId)
        {
            try
            {
                string apiUrl = $"/posts/like/{postId}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, httpClient.BaseAddress + apiUrl);
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
        public async Task<HttpResponseMessage> AddDislike(string postId)
        {
            try
            {
                string apiUrl = $"/posts/dislike/{postId}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, httpClient.BaseAddress + apiUrl);
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
        public async Task<HttpResponseMessage> RemoveLike(string postId)
        {
            try
            {
                string apiUrl = $"/posts/removeLike/{postId}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, httpClient.BaseAddress + apiUrl);
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
        public async Task<HttpResponseMessage> RemoveDislike(string postId)
        {
            try
            {
                string apiUrl = $"/posts/removeDislike/{postId}";
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, httpClient.BaseAddress + apiUrl);
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
        public async Task<HttpResponseMessage> AddReport(string idPost, int totalReports)
        {
            try
            {
                string apiUrl = "/posts/addReport";
                var requestData = new
                {
                    postId = idPost,
                    totalReports = totalReports
                };
                string jsonRequest = JsonConvert.SerializeObject(requestData);
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
    }
}
