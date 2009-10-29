<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl">
	<xsl:output method="xml" doctype-public="-//W3C//DTD XHTML 1.0 Strict//EN"
              media-type="application/html+xml" encoding="utf-8" omit-xml-declaration="yes" indent="no"/>
	<xsl:param name="language"></xsl:param>
	<xsl:template match="/">
		Selected language: <xsl:value-of select="$language"/>
		<xsl:copy-of select="//*[@language=$language]/*"/>
	</xsl:template>
</xsl:stylesheet>