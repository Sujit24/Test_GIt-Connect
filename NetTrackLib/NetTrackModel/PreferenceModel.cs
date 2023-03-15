using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace NetTrackModel
{
    public class PreferenceModel
    {
        public int SessionId { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }

        [DisplayName("Preferred Map:")]
        public int? PreferredMap { get; set; }

        [DisplayName("Map Size:")]
        public int? MapSize { get; set; }

        [DisplayName("Map Zoom:")]
        public string MapZoom { get; set; }

        [DisplayName("Map Refresh:")]
        public int? MapRefresh { get; set; } 

        [DisplayName("Map Center:")]
        public int? MapCenter { get; set; }

        [DisplayName("Legends Window:")]
        public int? LegendsWindow { get; set; }

        [DisplayName("Status Window:")]
        public int? StatusWindow { get; set; }

        [DisplayName("Max Points Displayed:")]
        public string MaxPointsDisplayed { get; set; }

        [DisplayName("Allow Paging:")]
        public int? AllowPaging { get; set; }

        [DisplayName("Default Report Format:")]
        public string DefaultReportFormat { get; set; }

        [DisplayName("Show Classes:")]
        public int? ShowClasses { get; set; }

        [DisplayName("Open Resizable:")]
        public int? WindowResizable { get; set; }

        public string MapClassses { get; set; }
    }
}
