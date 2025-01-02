namespace OptionChain
{
    public static class Utility
    {
        public static void LogDetails(string message)
        {
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Logs")))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Logs"));
            }
            using (StreamWriter writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), "Logs", DateTime.Now.Date.ToShortDateString() + ".txt"), append: true))
            {
                writer.WriteLine(DateTime.Now + " | " + message);
            }
        }
    }
}
