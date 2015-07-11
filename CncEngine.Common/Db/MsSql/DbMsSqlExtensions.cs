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

using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using CncEngine.Common.Xml;
using log4net;

namespace CncEngine.Common.Db.MsSql
{
    public static class DbMsSqlExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(XsltExtensions));

        public static Message MsSqlSelect(this Message message, MsSqlConnectionConfig config, string selectStatement)
        {
            Logger.Debug("Step");

            var adapter = new SqlDataAdapter(selectStatement, config.ToConnectionString());
            var dataSet = new DataSet();
            adapter.Fill(dataSet);
            var result = new XDocument(new XElement("MsSqlSelectResult"));
            foreach (DataTable table in dataSet.Tables)
            {
                var doc = new XElement(table.TableName);
                foreach (DataRow row in table.Rows)
                {
                    var xmlRow = new XElement("Row");
                    foreach (DataColumn column in table.Columns)
                    {
                        xmlRow.Add(new XElement(column.ColumnName, row[column.Ordinal].ToString()));
                    }
                    doc.Add(xmlRow);
                }
                result.Root.Add(doc);
            }
            message.SetPayload(result.ToString());
            return message;
        }

        public static Message MsSqlSelect(this Message message, string selectStatement)
        {
            if (!message.Variables.ContainsKey("Db.Host") ||
                !message.Variables.ContainsKey("Db.Port") ||
                !message.Variables.ContainsKey("Db.Username") ||
                !message.Variables.ContainsKey("Db.Password") ||
                !message.Variables.ContainsKey("Db.Database"))
                throw new InvalidOperationException("Configuration incorrect");

            var config = new MsSqlConnectionConfig
            {
                Host = message.Variables["Db.Host"].ToString(),
                Port = int.Parse(message.Variables["Db.Port"].ToString()),
                Username = message.Variables["Db.Username"].ToString(),
                Password = message.Variables["Db.Password"].ToString(),
                Database = message.Variables["Db.Database"].ToString()
            };
            return message.MsSqlSelect(config, selectStatement);
        }
    }
}
