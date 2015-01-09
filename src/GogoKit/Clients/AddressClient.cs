﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GogoKit.Helpers;
using GogoKit.Http;
using GogoKit.Models;
using GogoKit.Resources;

namespace GogoKit.Clients
{
    public class AddressClient : IAddressClient
    {
        private readonly IUserClient _userClient;
        private readonly IApiConnection _apiConnection;
        private readonly IResourceLinkComposer _resourceLinkComposer;

        public AddressClient(IUserClient userClient, IApiConnection apiConnection, IResourceLinkComposer resourceLinkComposer)
        {
            _userClient = userClient;
            _apiConnection = apiConnection;
            _resourceLinkComposer = resourceLinkComposer;
        }

        public async Task<IReadOnlyList<Address>> GetAllAddressesAsync()
        {
            var user = await _userClient.GetAsync();
            return await _apiConnection.GetAllPagesAsync<Address>(user.Links["user:addresses"], null);
        }

        public async Task<PagedResource<Address>> GetAddresses(int page, int pageSize)
        {
            var user = await _userClient.GetAsync();
            return await _apiConnection.GetAsync<PagedResource<Address>>(user.Links["user:addresses"], new Dictionary<string, string>()
                                                                        {
                                                                            {"page", page.ToString()},
                                                                            {"page_size", pageSize.ToString()}
                                                                        });
        }

        public async Task<Address> CreateAddress(AddressCreate addressCreate)
        {
            var addresses = await GetAddresses(1, 1);
            var createAddressLink = addresses.Links["address:create"];
            return await _apiConnection.PostAsync<Address>(createAddressLink, null, addressCreate);
        }

        public async Task<Address> UpdateAddress(int addressId, AddressUpdate addressUpdate)
        {
            var updateAddressLink = await _resourceLinkComposer.ComposeLinkWithAbsolutePathForResource(ApiUrls.UpdateAddress(addressId));
            return await _apiConnection.PatchAsync<Address>(updateAddressLink, null, addressUpdate);
        }
    }
}