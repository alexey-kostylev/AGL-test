using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGL.App.Models;
using System.Net.Http;
using RestSharp;
using System.Threading;
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

        public async Task<ICollection<Person>> GetPersons()
        {
                     
            var request = new RestRequest();
            
            var response = await _restClient.ExecuteGetTaskAsync<List<Person>>(request);          

            if (response == null)
            {
                throw new InvalidOperationException("response is null");
            }

            if (response.ResponseStatus != ResponseStatus.Completed || response.StatusCode != HttpStatusCode.OK)
            {                
                throw new InvalidOperationException(
                    $"Error executing request. Error message: '{response.ErrorMessage}',"+ 
                    $"response status: {response.ResponseStatus}, "+
                    $"status code: {response.StatusCode}, ",
                    response.ErrorException);
            }
            
            return response.Data;
        }
    }
}
