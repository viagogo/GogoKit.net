﻿using System;
using System.Threading.Tasks;
using GogoKit.Models.Response;

namespace GogoKit.Services
{
    public class InMemoryOAuth2TokenStore : IOAuth2TokenStore
    {
        private OAuth2Token _token;

        public InMemoryOAuth2TokenStore()
        {
            _token = null;
        }

        public Task<OAuth2Token> GetTokenAsync()
        {
            return Task.FromResult(_token);
        }

        public Task<OAuth2Token> GetCachedTokenAsync(string cacheKey, Func<Task<OAuth2Token>> getTokenAsyncFunc)
        {
            return getTokenAsyncFunc != null ? getTokenAsyncFunc() : Task.FromResult<OAuth2Token>(null);
        }

        public Task SetTokenAsync(OAuth2Token token)
        {
            Requires.ArgumentNotNull(token, nameof(token));

            _token = token;
            return Task.FromResult(0);
        }

        public Task DeleteTokenAsync()
        {
            _token = null;
            return Task.FromResult(0);
        }
    }
}