﻿using System.Net.Http.Headers;

namespace DemyAI.Services;

public class HttpService(IAppService appService, HttpClient httpClient) : IHttpService {

    public async Task<T?> GetAsync<T>(string url, string authToken = " ") {

        try {

            if(!string.IsNullOrEmpty(authToken)) {

                httpClient.DefaultRequestHeaders.Clear();

                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            }

            var respose = await httpClient.GetAsync(url);
            if(respose != null && respose.IsSuccessStatusCode) {

                var data = await respose.Content.ReadFromJsonAsync<T>();
                return data!;
            }

        } catch(Exception e) {

            await appService.DisplayAlert("Error", e.Message, "OK");
        }

        return default;
    }


    public async Task<HttpResponseMessage?> PostAsync(string url, HttpContent content, string key) {
        try {

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", key);

            var response = await httpClient.PostAsync(url, content);

            return response;

        } catch(Exception e) {
            await appService.DisplayAlert("Error", e.Message, "OK");
        }

        return null;
    }
}
