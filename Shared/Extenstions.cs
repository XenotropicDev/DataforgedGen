namespace DataforgedGen
{
    public static class Extenstions
    {
        /// <summary>
        /// I don't really like extending string but it's useful just to make this easier to use in cshtml pages. 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="minLines"></param>
        /// <returns></returns>
        public static int GetMultilineCount(this string text, int minLines = 2)
        {
            if (String.IsNullOrWhiteSpace(text)) return minLines;

            var lineCount = 1;

            for (int i = 0; i < text.Length; i++) 
            {
                if (text[i] == '\n') lineCount++;
                if (text[i] == '\r' && text.Length >= i + 2 && text[i + 1] != '\n') lineCount++;
            }

            if (lineCount < minLines) return minLines; 
            
            return lineCount;
        }
    }
}
