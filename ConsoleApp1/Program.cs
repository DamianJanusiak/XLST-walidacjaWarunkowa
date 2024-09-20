using ConsoleApp1.src;
using Newtonsoft.Json;
using System.Xml;

class Program
{
	static void Main()
	{
		// Load the XML document
		var xmlDoc = new XmlDocument();
		xmlDoc.Load("Test.xml");

		var rules = JsonConvert.DeserializeObject<List<ValidationRules>>(File.ReadAllText("Test.json"));

		var validator = new Validator();

		var errors = validator.Validate(xmlDoc, rules);

		foreach (var error in errors) {

			Console.WriteLine(error);
		}
	}
}

