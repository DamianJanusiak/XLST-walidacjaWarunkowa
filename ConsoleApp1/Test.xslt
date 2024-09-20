
<xsl:stylesheet version="2.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="MyElement">
		<xsl:apply-templates/>
		<xsl:choose>
			<xsl:when test="Id_Ser and not(A)">
				$Validation Error: Element 'A' must be present when Id_Ser is present.<xsl:text>&#xa;</xsl:text>
			</xsl:when>
			<xsl:when test="not(Id_Ser) and A">
				$Validation Error: Element 'A' must not be present when Id_Ser is not present.<xsl:text>&#xa;</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<!-- No validation errors -->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="C">
		<xsl:apply-templates/>
		<xsl:value-of select="."/>
		<xsl:choose>
			<xsl:when test="count(D) &gt; 1">
				$Validation Error: Element 'D' can be used only once<xsl:text>&#xa;</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<!-- No validation errors -->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
</xsl:stylesheet>
