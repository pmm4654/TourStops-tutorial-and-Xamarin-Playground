﻿using Minimize.Api.Client.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace Minimize.Api.Client
{
    public class ApiClient
    {
        private HttpClient _client;

        public string AuthorizationToken { get; set; } = "";

        public ApiClient(string BaseAddress = null)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(BaseAddress ?? "https://minimize.herokuapp.com/");
        }

        public async Task<SignupResponse> Signup(SignupRequest request)
        {
            var response = await PostAsJsonAsync<SignupResponse>(
                "signup", 
                new
                {
                    request.email,
                    request.first_name,
                    request.last_name,
                    request.password,
                    request.password_confirmation
                }, 
                authenticate: false);

            AuthorizationToken = response.AuthorizationToken;

            return response;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var response = await PostAsJsonAsync<LoginResponse>(
                "auth/login", 
                new
                {
                    request.email,
                    request.password
                });

            AuthorizationToken = response.auth_token;

            return response;
        }

        #region Things
        public async Task<List<Thing>> GetThings()
        {
            return await GetAsync<List<Thing>>("things");
        }

        public async Task<Thing> GetThing(int id)
        {
            return await GetAsync<Thing>($"things/{id}");
        }

        public async Task<Thing> PostThing(Thing thing)
        {
            return await PostAsJsonAsync<Thing>(
                "things", 
                new
                {
                    thing.title,
                    thing.category_id
                });
        }

        public async Task<bool> PutThing(int id, Thing thing)
        {
            return await PutAsJsonAsync(
                $"things/{id}", 
                new
                {
                    thing.title,
                    thing.category_id
                });
        }

        public async Task<bool> DeleteThing(int id)
        {
            return await DeleteAsync($"things/{id}");
        }
        #endregion

        #region Categories
        public async Task<IEnumerable<Category>> GetCategories()
        {
            return await GetAsync<IEnumerable<Category>>("categories");
        }

        public async Task<Category> GetCategory(int id)
        {
            return await GetAsync<Category>($"categories/{id}");
        }

        public async Task<Category> PostCategory(Category category)
        {
            return await PostAsJsonAsync<Category>(
                "categories", 
                new
                {
                    category.name,
                    category.user_id,
                    category.threshold
                });
        }

        public async Task<bool> PutCategory(int id, Category category)
        {
            return await PutAsJsonAsync(
                $"categories/{id}",
                new
                {
                    category.name,
                    category.user_id,
                    category.threshold
                });
        }

        public async Task<bool> DeleteCategory(int id)
        {
            return await DeleteAsync($"categories/{id}");
        }
        #endregion

        private async Task<TResponse> GetAsync<TResponse>(string url, bool authenticate = true)
        {
            SetHeaders(authenticate);
            
            var response = await _client.GetAsync(url);
            return await response.Content.ReadAsAsync<TResponse>();
        }

        private async Task<TResponse> PostAsJsonAsync<TResponse>(string url, object body, bool authenticate = true)
        {
            SetHeaders(authenticate);
            
            var response = await _client.PostAsJsonAsync(url, body);
            return await response.Content.ReadAsAsync<TResponse>();
        }

        private async Task<bool> PutAsJsonAsync(string url, object body, bool authenticate = true)
        {
            SetHeaders(authenticate);
            
            var response = await _client.PutAsJsonAsync(url, body);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        private async Task<bool> DeleteAsync(string url, bool authenticate = true)
        {
            SetHeaders(authenticate);
            
            var response = await _client.DeleteAsync(url);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }

        private void SetHeaders(bool authenticate)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Clear();

            _client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
                    "application/json"));

            if (authenticate)
                _client.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue(AuthorizationToken, "");
        }
    }
}