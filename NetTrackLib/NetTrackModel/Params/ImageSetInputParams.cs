using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetTrackModel.Params
{
    public class ImageSetInputParams:InputParams
    {

        public String FullID { get; set; }
        public ImageSetInputParams(int sessionID,string fullID = "") : base(sessionID) 
        {
            FullID = fullID;
        }
    }
}
