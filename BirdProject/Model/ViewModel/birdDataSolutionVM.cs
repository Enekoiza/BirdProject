using System;
using Newtonsoft.Json;

namespace BirdProject.Model.ViewModel
{
	public class birdDataSolutionVM
	{
        [JsonProperty("metalRingID")]
        public string metalRingID;

        [JsonProperty("birdData")]
        public List<birdRecordVM> birdData;
    }
}

