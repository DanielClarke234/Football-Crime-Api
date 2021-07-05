using AND.Models.Exceptions;
using AND.Models.PostcodeLookup;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Football_Crime_Api.DAL.Postcode
{
    public interface IPostcodeLookup
    {
        PostcodeLookupResponseModel GetPostcodeDetails(string postcode);
    }

    public class PostcodeLookup : IPostcodeLookup
    {
        private readonly IConfiguration _config;

        public PostcodeLookup(IConfiguration config)
        {
            _config = config;
        }

        public PostcodeLookupResponseModel GetPostcodeDetails(string postcode)
        {
            //Call to the api and check if the postcode is valid
            using (var client = new WebClient())
            {
                var postcodeData = client.DownloadString(new Uri(_config.GetValue<string>("URLs:PostcodeLookup") + postcode));
                var returnModel = JsonConvert.DeserializeObject<PostcodeLookupResponseModel>(postcodeData);

                if (!string.IsNullOrEmpty(returnModel.error))
                {
                    throw new UserException("Postcode provided could not be found");
                }

                return returnModel;
            }
        }
    }
}
