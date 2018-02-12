using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AGL.App.Models;
using RestSharp;
using System.Net;

namespace AGL.App.Adapters
{
    public class AzurePeopleAdapter : IPeopleAdapter
    {       
        
        private IRestClient _restClient;


        public AzurePeopleAdapter(IRestClient restClient)
        {            
            if (restClient == null)
            {
                throw new ArgumentNullException(nameof(restClient));
            }

            _restClient = restClient;
        }

        public async Task<ICollection<Person>> GetPetOwners()
        {
                     
            var request = new RestRequest();
            
            var response = await _restClient.ExecuteGetTaskAsync<List<Person>>(request);          

            if (response == null)
            {
                throw new InvalidOperationException("response is null");
            }

            if (response.ErrorException != null || response.StatusCode != HttpStatusCode.OK || response.ResponseStatus == ResponseStatus.Error)
            {                
                throw new WebException(
                    $"Error executing request. Error message: '{response.ErrorMessage}',"+ 
                    $"response status: {response.ResponseStatus}, "+
                    $"status code: {response.StatusCode}, ",
                    response.ErrorException);
            }
            
            return response.Data;
        }
    }
}
