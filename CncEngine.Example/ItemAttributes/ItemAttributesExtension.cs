/**
    Copyright (C) 2015  Carsten Blank

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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
