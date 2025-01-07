namespace OptionChain
{
    public static class Utility
    {
        public static void LogDetails(string message)
        {
            try
            {
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Logs")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Logs"));
                }

                string fileName = DateTime.Now.Date.ToString("yyyy-MM-dd") + ".txt";

                using (StreamWriter writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Logs", fileName), append: true))
                {
                    writer.WriteLine(DateTime.Now + " | " + message);
                }
            }
            catch
            {

            }
        }
    }
}
