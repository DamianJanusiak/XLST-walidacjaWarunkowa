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

		var rules = JsonConvert.DeserializeObject<ValidatorConfiguration>(File.ReadAllText("Test.json"));

		var validator = new Validator();

		var xmlRules = XmlReader.Create(new StringReader(File.ReadAllText("Test.xslt")));

		var errors = validator.Validate(xmlDoc, rules);

		foreach (var error in errors) {

			Console.WriteLine(error);
		}
	}
}

