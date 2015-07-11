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

using System.Data;
using System.Xml.Linq;
using MySql.Data.MySqlClient;

namespace CncEngine.Common.Db.MySql
{
    public static class DbMySqlExtensions
    {
        public static Message MySqlSelect(this Message message, MySqlConnectionConfig config, string selectStatement)
        {
            var adapter = new MySqlDataAdapter(selectStatement, config.ToConnectionString());
            var dataSet = new DataSet();
            adapter.Fill(dataSet);
            var result = new XDocument(new XElement("MySqlSelectResult"));
            foreach (DataTable table in dataSet.Tables)
            {
                var doc = new XElement(table.TableName);
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn column in table.Columns)
                    {
                        doc.Add(new XElement(column.ColumnName, row[column.Ordinal].ToString()));
                    }
                }
                result.Root.Add(doc);
            }
            message.SetPayload(result.ToString());
            return message;
        }
    }
}
