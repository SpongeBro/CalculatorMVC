namespace Calculator.Managers
{
    public class ErrorHandler
    {
        private string m_logFileName;

        public ErrorHandler(string fileName = "log.txt")
        {
            m_logFileName = fileName;
        }
        public void SendError(Exception exception)
        {
            try {
                string message = $"[{DateTime.Now}] Exception: {exception.Message}\n";
                message += $"Stack Trace:\n{exception.StackTrace}\n\n\n";
                File.AppendAllText(m_logFileName, message);
            }
            catch (Exception ex) {
                Console.WriteLine($"Failed to log exception: {ex.Message}");
            }
        }
    }
}
