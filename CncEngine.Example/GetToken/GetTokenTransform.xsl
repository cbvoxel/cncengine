<?xml version="1.0" encoding="utf-8" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output indent="yes" omit-xml-declaration="yes"/>
  <xsl:template match="/">
    <soapenv:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
                  xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
                  xmlns:ver="http://2289467164.plentymarkets-x1.com/plenty/api/soap/version114/">
      <soapenv:Header/>
      <soapenv:Body>
        <ver:GetAuthentificationToken soapenv:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">
          <oLogin xsi:type="ver:PlentySoapRequest_GetAuthentificationToken">
            <!--You may enter the following 2 items in any order-->
            <Username xsi:type="xsd:string">
              <xsl:value-of select="//Username"/>
            </Username>
            <Userpass xsi:type="xsd:string">
              <xsl:value-of select="//Password"/>
            </Userpass>
          </oLogin>
        </ver:GetAuthentificationToken>
      </soapenv:Body>
    </soapenv:Envelope>
  </xsl:template>
</xsl:stylesheet>