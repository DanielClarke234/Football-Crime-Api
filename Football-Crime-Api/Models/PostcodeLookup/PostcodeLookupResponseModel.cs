using Football_Crime_Api.Models.PostcodeLookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AND.Models.PostcodeLookup
{
    public class PostcodeLookupResponseModel
    {
        public int status { get; set; }
        public PostcodeDetailsModel result {get;set;}
        public string error { get; set; }
    }
}
