using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hefesto.base_hefesto.Util
{
    public class JSONListConverter<T>
    {
		public string ListToJSON(List<T> lista)
		{
			var jsonString = JsonSerializer.Serialize<List<T>>(lista);

			return jsonString;
		}

		public List<T> JsonToList(string textoJSON)
		{
			var obj = JsonSerializer.Deserialize<List<T>>(textoJSON);

			return obj;
		}
	}
}
