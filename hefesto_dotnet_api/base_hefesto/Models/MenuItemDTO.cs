using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace hefesto.base_hefesto.Models
{
    public class MenuItemDTO
    {
        public string Label { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RouterLink { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Url { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string To { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MenuItemDTO> Item { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Name { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string View { get; set; }

        public MenuItemDTO(){
    		this.Item = new List<MenuItemDTO>();
	    	Clean();
        }

        public MenuItemDTO(string label, string url) : base() {
            this.Label = label;
            this.Url = url;
            this.RouterLink = url;
            this.To = url;
            SetName(url);
        }        

        public MenuItemDTO(string label, string url, List<MenuItemDTO> item) : base() {
            this.Label = label;
            this.Url = url;
            this.RouterLink = url;
            this.To = url;
            this.Item = item;
            SetName(url);
        }

        private void SetName(string url)
        {
            if (url!=null)
            {
                if (url.Contains('/'))
                {
                    this.Name = url.Substring(url.LastIndexOf('/') + 1);
                    this.View = this.Name + "View";
                }
            }
        }

        public void Clean() {
            this.Label = "";
            this.RouterLink = "";
            this.Url = "";
            this.To = "";
            this.Item.Clear();
            this.Name = "";
            this.View = "";
        }

    }
}
