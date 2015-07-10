<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<soapenv:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
			xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/"
			xmlns:ver="http://2289467164.plentymarkets-x1.com/plenty/api/soap/version113/">
			<soapenv:Header>
				<ver:verifyingToken>
					<UserID>
						<xsl:value-of select="//Login/UserID" />
					</UserID>
					<Token>
						<xsl:value-of select="//Login/Token" />
					</Token>
				</ver:verifyingToken>
			</soapenv:Header>
			<soapenv:Body>
				<ver:SetItemAttributes soapenv:encodingStyle="http://schemas.xmlsoap.org/soap/encoding/">
					<oPlentySoapRequest_SetItemAttributes
						xsi:type="ver:PlentySoapRequest_SetItemAttributes">
						<Attributes xsi:type="ver:ArrayOfPlentysoapobject_setitemattribute">
							<item xsi:type="ver:PlentySoapObject_SetItemAttribute">
								<!-- <AmazonVariation>Integer</AmazonVariation> -->
								<BackendName>
									<xsl:value-of select="//Database/Type" />
								</BackendName>
								<Contentpage></Contentpage>
								<FrontendLang></FrontendLang>
								<FrontendName></FrontendName>
								<!-- <GoogleProductsVariation>Integer</GoogleProductsVariation> -->
								<Id>
									<xsl:value-of
										select="//Attributes/item[BackendName=//Database/Type]/Id" />
								</Id>
								<!-- <ImageAttribute>Boolean</ImageAttribute> -->
								<!-- <MarkupPercental>Double</MarkupPercental> -->
								<!-- <NeckermannAttribute>Integer</NeckermannAttribute> -->
								<!-- <OttoVariation>Integer</OttoVariation> -->
								<!-- <PixmaniaAttribute>Integer</PixmaniaAttribute> -->
								<!-- <Position>Integer</Position> -->
								<!-- <ShopperellaVariation>Integer</ShopperellaVariation> -->
								<Values xsi:type="ver:ArrayOfPlentysoapobject_setitemattributevalue">
									<xsl:for-each select="/BuildRequestInput/Database/Terms/TermEntry/Term">
									<xsl:variable name="term" select="." />
										<item xsi:type="ver:PlentySoapObject_SetItemAttributeValue">
											<BackendName>
												<xsl:value-of select="$term" />
											</BackendName>
											<!-- <Comment></Comment> -->
											<FrontendName>
												<xsl:value-of select="$term" />
											</FrontendName>
											<!-- <Markup></Markup> -->
											<!-- <Position></Position> -->
											<ValueId>
												<xsl:value-of
													select="/BuildRequestInput//Attributes/item[BackendName=/BuildRequestInput/Database/Type/text()]/Values/item[BackendName=$term]/ValueId/text()" />
											</ValueId>
										</item>
									</xsl:for-each>
								</Values>
								<!-- <CallItemsLimit>Integer</CallItemsLimit> -->
							</item>
						</Attributes>
					</oPlentySoapRequest_SetItemAttributes>
				</ver:SetItemAttributes>
			</soapenv:Body>
		</soapenv:Envelope>
	</xsl:template>
</xsl:stylesheet>