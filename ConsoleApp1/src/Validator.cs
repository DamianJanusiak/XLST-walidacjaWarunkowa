using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;
using XmlPrime;

namespace ConsoleApp1.src
{
	public class Validator
	{
		public IEnumerable<string> Validate(XmlDocument xml, ValidatorConfiguration validationRules)
		{
			var xlst = GenerateXlst(validationRules);

			return Validate(xml, xlst);
		}


		public IEnumerable<string> Validate(XmlDocument xml, XmlReader xlst)
		{
			try
			{
				using (var sw = new StringWriter())
				{
					using (var xw = new XmlTextWriter(sw))
					{
						var xlstCompiler = new XslCompiledTransform();
						xlstCompiler.Load(xlst);
						xlstCompiler.Transform(xml, null, xw);

						return Regex.Matches(sw.ToString(), @"\$Validation Error:.*\n").Select(x=>x.Value[("$ValidationError:".Length+1)..]);
					}

				}
			}
			catch (Exception ex)
		{
				Console.WriteLine(ex.ToString());
				throw;
			}
		}

		public XmlReader GenerateXlst(ValidatorConfiguration configuration)
		{
			var validationStr = new StringBuilder();

			foreach (ValidationRules rule in configuration.ValidationRules)
			{
				var conditions = new StringBuilder();

				foreach (var condition in rule.Rules)
				{
					conditions.AppendLine(@$"<xsl:when test=""{condition.Condition}"">
				$Validation Error:{condition.Message}<xsl:text>&#xa;</xsl:text>
				</xsl:when>");
				}


				validationStr.AppendLine(@$"  <xsl:template match=""{rule.NodeContext}"">
		<xsl:apply-templates/>
				<xsl:choose>
					{conditions}
			  <xsl:otherwise>
				<!-- No validation errors -->
			  </xsl:otherwise>
			</xsl:choose>
		  </xsl:template>");
			}

			var namespaceStr = string.Empty;

			foreach (var ns in configuration.Namespaces)
			{
				namespaceStr += $"xmlns:{ns.Key}=\"{ns.Value}\"";
			}

			var str = @$"<?xml version=""1.0"" encoding=""UTF-8""?>
						<xsl:stylesheet version=""2.0""
						{namespaceStr}
							xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
						{validationStr}
						</xsl:stylesheet>
						";

			return XmlReader.Create(new StringReader(str));
		}
	}
}
