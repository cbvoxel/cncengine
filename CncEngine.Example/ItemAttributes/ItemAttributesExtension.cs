using CncEngine.Common;
using CncEngine.Common.Http;
using CncEngine.Common.Xml;
using CncEngine.Example.GetToken;

namespace CncEngine.Example.ItemAttributes
{
    public static class ItemAttributesExtension
    {
        public static Message GetItemAttributes(this Message message, HttpConnectorConfiguration plentyConfig)
        {
            return message
                .LoadToken(plentyConfig)
                .SetPayload(m => string.Format("<Token>{0}</Token>", m.Variables["Token"]))
                .Combine(m => m.LoadModuleResourceTextFile<Module>(@"PlentyConfig.xml"))
                .XslTransformFromModuleResource<Module>("ItemAttributes/Login2GetItemAttributes.xsl")
                .SetVariable("HttpHeadersSoapAction", "GetItemAttributes")
                .HttpPostMessage(plentyConfig, "");
        }
    }
}
