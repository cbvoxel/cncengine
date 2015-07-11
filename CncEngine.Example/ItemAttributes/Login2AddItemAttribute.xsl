<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<soapenv:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
			xmlns:ver="http://2289467164.plentymarkets-x1.com/plenty/api/soap/version113/">
			<soapenv:Header>
				<ver:verifyingToken>
					<UserID>
						<xsl:value-of select="//PlentyConfig/UserID" />
					</UserID>
					<Token>
						<xsl:value-of select="//PlentyConfig/Token" />
					</Token>
				</ver:verifyingToken>
			</soapenv:Header>
			<soapenv:Body>
				<ver:SetItemAttributes soapenv:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">
					<oPlentySoapRequest_SetItemAttributes
						xsi:type="ver:PlentySoapRequest_SetItemAttributes">
						<Attributes xsi:type="ver:ArrayOfPlentysoapobject_setitemattribute">
							<item xsi:type="ver:PlentySoapObject_SetItemAttribute">
								<BackendName>
									<xsl:value-of select="//CurrentType" />
								</BackendName>
								<Contentpage></Contentpage>
								<FrontendLang></FrontendLang>
								<FrontendName></FrontendName>
								<Id>
									<xsl:value-of
										select="//item/Id" />
								</Id>
								<Values xsi:type="ver:ArrayOfPlentysoapobject_setitemattributevalue">
									<xsl:for-each select="//Term">
									<xsl:variable name="term" select="." />
										<item xsi:type="ver:PlentySoapObject_SetItemAttributeValue">
											<BackendName>
												<xsl:value-of select="$term" />
											</BackendName>
											<FrontendName>
												<xsl:value-of select="$term" />
											</FrontendName>
											<ValueId>
												<xsl:value-of
													select="//item/Values/item[BackendName=$term]/ValueId/text()" />
											</ValueId>
										</item>
									</xsl:for-each>
								</Values>
							</item>
						</Attributes>
					</oPlentySoapRequest_SetItemAttributes>
				</ver:SetItemAttributes>
			</soapenv:Body>
		</soapenv:Envelope>
	</xsl:template>
</xsl:stylesheet>