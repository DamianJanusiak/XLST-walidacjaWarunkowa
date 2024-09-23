<xsl:stylesheet version="2.0"
	xmlns:t="http://test.com"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="t:MyElement">
		<xsl:apply-templates/>
		<xsl:choose>
			<xsl:when test="Id_Ser and not(A)">
				$Validation Error: Element 'A' must be present when Id_Ser is present.<xsl:text>&#xa;</xsl:text>
			</xsl:when>
			<xsl:when test="not(Id_Ser[text()='AAA']) and A">
				$Validation Error: Element 'A' must not be present when Id_Ser is not AAA.<xsl:text>&#xa;</xsl:text>
			</xsl:when>
			<xsl:when test="((A[text()='ABC'] and B[text()='PL']) or (C[text() = 'CK0314' or text() = 'CK0315' or text() = 'CK0316'] and D[text() = 'CK0314' or text() = 'CK0315' or text() = 'CK0316'])) and not(E)">
				$Validation Error: Test.<xsl:text>&#xa;</xsl:text>
			</xsl:when>
			<xsl:otherwise>
				<!-- No validation errors -->
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="t:C">
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



