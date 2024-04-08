namespace Service.Utilitarios
{
    public static class TokenEvento
    {
        public static async Task<string> GenerateToken()
        {
            return await Task.Run(() =>
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                Random random = new Random();
                char[] tokenArray = new char[18];

                for (int i = 0; i < 18; i++)
                {
                    tokenArray[i] = chars[random.Next(chars.Length)];
                }

                return new string(tokenArray);
            });
        }
    }
}
