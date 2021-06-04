using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace hefesto.base_hefesto.Models
{
    public class MenuItemDTO
    {
        public MenuItemDTO(){
    		this.Item = new List<MenuItemDTO>();
	    	Clean();
        }
        public MenuItemDTO(String label, String url) : base() {
            this.Label = label;
            this.Url = url;
            this.RouterLink = url;
            this.To = url;
        }        
        public MenuItemDTO(String label, String url, List<MenuItemDTO> item) : base() {
            this.Label = label;
            this.Url = url;
            this.RouterLink = url;
            this.To = url;
            this.Item = item;
        }
        public void Clean() {
            this.Label = "";
            this.RouterLink = "";
            this.Url = "";
            this.To = "";
            this.Item.Clear();
        }
        public string Label { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string RouterLink { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Url { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string To { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<MenuItemDTO> Item { get; set; }

    }
}
