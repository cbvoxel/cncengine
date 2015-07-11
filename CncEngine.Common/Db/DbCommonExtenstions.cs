namespace CncEngine.Common.Db
{
    public static class DbCommonExtenstions
    {
        public static Message ExtractHost(this Message message, string xPath)
        {
            return message.ExtractVariable(xPath, "Db.Host");
        }

        public static Message ExtractPort(this Message message, string xPath)
        {
            return message.ExtractVariable(xPath, "Db.Port");
        }

        public static Message ExtractUsername(this Message message, string xPath)
        {
            return message.ExtractVariable(xPath, "Db.Username");
        }

        public static Message ExtractPassword(this Message message, string xPath)
        {
            return message.ExtractVariable(xPath, "Db.Password");
        }

        public static Message ExtractDatabase(this Message message, string xPath)
        {
            return message.ExtractVariable(xPath, "Db.Database");
        }
    }
}
