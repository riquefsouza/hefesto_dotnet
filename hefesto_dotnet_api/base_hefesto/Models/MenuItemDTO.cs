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
        public List<MenuItemDTO> Items { get; set; }

        public MenuItemDTO(){
    		this.Items = new List<MenuItemDTO>();
	    	Clean();
        }

        public MenuItemDTO(string label, string url) : base() {
            this.Label = label;
            this.Url = url;
            this.RouterLink = url;
            this.To = url;
        }        

        public MenuItemDTO(string label, string url, List<MenuItemDTO> items) : base() {
            this.Label = label;
            this.Url = url;
            this.RouterLink = url;
            this.To = url;
            this.Items = items;
        }

        public void Clean() {
            this.Label = "";
            this.RouterLink = "";
            this.Url = "";
            this.To = "";
            this.Items.Clear();
        }

    }
}
