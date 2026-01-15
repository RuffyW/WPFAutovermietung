<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" encoding="UTF-8" indent="yes"/>
    
    <xsl:template match="/doc">
        <html>
            <head>
                <title>XML-Dokumentation</title>
                <style>
                    body { font-family: Arial, sans-serif; }
                    h3 { color: #333; }
                </style>
            </head>
            <body>
                <h1>Dokumentation für <xsl:value-of select="assembly/name"/></h1>
                <xsl:apply-templates select="members/member"/>
            </body>
        </html>
    </xsl:template>
    
    <xsl:template match="member">
        <h3><xsl:value-of select="@name"/></h3>
        <p>
    <xsl:for-each select="summary">
        <xsl:value-of select="."/>
        <xsl:if test="position() != last()">; </xsl:if>
    </xsl:for-each>
		</p>
        <xsl:if test="remarks">
            <p><strong>Bemerkungen:</strong> <xsl:value-of select="remarks"/></p>
        </xsl:if>
        <xsl:if test="param">
            <p><strong>Parameter:</strong></p>
            <ul>
                <xsl:for-each select="param">
                    <li><xsl:value-of select="@name"/>: <xsl:value-of select="."/></li>
                </xsl:for-each>
            </ul>
        </xsl:if>
    </xsl:template>
</xsl:stylesheet>
