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
		public IEnumerable<string> Validate(XmlDocument xml, IEnumerable<ValidationRules> validationRules)
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

		public XmlReader GenerateXlst(IEnumerable<ValidationRules> rules)
		{
			var validationStr = new StringBuilder();

			foreach (ValidationRules rule in rules)
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

			var str = @$"<?xml version=""1.0"" encoding=""UTF-8""?>
						<xsl:stylesheet version=""2.0""
							xmlns:xsl=""http://www.w3.org/1999/XSL/Transform"">
						{validationStr}
						</xsl:stylesheet>
						";

			return XmlReader.Create(new StringReader(str));
		}
	}
}
