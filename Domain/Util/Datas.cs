namespace Domain.Util
{
    public class Datas
    {
        public int ConvertData(DateTime data)
        {
            var result = Task.Run(() =>
            {
                TimeSpan timeSpan = DateTime.Today.ToUniversalTime() - data;

                int idade = (int)(timeSpan.Days / 365.25);

                return idade;
            });

            return result.Result;
        }
    }
}
