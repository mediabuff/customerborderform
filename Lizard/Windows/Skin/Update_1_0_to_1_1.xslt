<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  
  <xsl:template match="styleLibrary">
    <xsl:element name="styleLibrary">
      <xsl:attribute name="schemaVersion">1.1</xsl:attribute>
      <xsl:attribute name="defaultStyleName">
        <xsl:value-of select="@defaultStyleName"/>
      </xsl:attribute>
      <xsl:apply-templates select="*" />
    </xsl:element>
  </xsl:template>

  <xsl:template match="formStyle">
    <xsl:element name="formStyle">
      <xsl:copy-of select="@*"/>
      <xsl:copy-of select="normalState"/>
      <xsl:element name="buttons">
        <xsl:apply-templates select="minimizeButton" />
        <xsl:apply-templates select="maximizeButton" />
        <xsl:apply-templates select="closeButton" />
        <xsl:apply-templates select="restoreButton" />
        <xsl:apply-templates select="helpButton" />


      </xsl:element>
    </xsl:element>
  </xsl:template>

  <xsl:template match="minimizeButton">
    <xsl:call-template name="copyButton">
      <xsl:with-param name="key">MinimizeButton</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="maximizeButton">
    <xsl:call-template name="copyButton">
      <xsl:with-param name="key">MaximizeButton</xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template match="closeButton">
    <xsl:call-template name="copyButton">
      <xsl:with-param name="key">CloseButton</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  
  <xsl:template match="restoreButton">
    <xsl:call-template name="copyButton">
      <xsl:with-param name="key">RestoreButton</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  
  <xsl:template match="helpButton">
    <xsl:call-template name="copyButton">
      <xsl:with-param name="key">HelpButton</xsl:with-param>
    </xsl:call-template>
  </xsl:template>
  
  <xsl:template name="copyButton">
    <xsl:param name="key"></xsl:param>

    <xsl:element name="buttonStyle">
      <xsl:attribute name="key">
        <xsl:value-of select="$key"/>
      </xsl:attribute>
      <xsl:apply-templates select="@* | node()" />
    </xsl:element>
  </xsl:template> 
  
  <xsl:template match="/ | @* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()" />
    </xsl:copy>
  </xsl:template>
  
</xsl:stylesheet>