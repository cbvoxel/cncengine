<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output omit-xml-declaration="yes"/>
	<xsl:template match="/">
		<soapenv:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
			xmlns:ver="http://2289467164.plentymarkets-x1.com/plenty/api/soap/version114/">
			<soapenv:Header>
				<ver:verifyingToken>
					<UserID>
						<xsl:value-of select="//PlentyConfig/UserID" />
					</UserID>
					<Token>
						<xsl:value-of select="//Token" />
					</Token>
				</ver:verifyingToken>
			</soapenv:Header>
			<soapenv:Body>
        <ver:GetItemAttributes soapenv:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">
          <PlentySoapRequest_GetItemAttributes xsi:type="ver:PlentySoapRequest_GetItemAttributes">
            <GetValues xsi:type="xsd:boolean">True</GetValues>
          </PlentySoapRequest_GetItemAttributes>
        </ver:GetItemAttributes>
			</soapenv:Body>
		</soapenv:Envelope>
	</xsl:template>
</xsl:stylesheet>