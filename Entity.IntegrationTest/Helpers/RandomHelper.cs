using System;

namespace Entity.IntegrationTest.Helpers
{
    public static class RandomHelper
    {
        public static string GetRandomPhoneNumber()
        {
            var random = new Random();
            string randomPhone = string.Empty;
            for (int i = 0; i < 10; i++)
                randomPhone = String.Concat(randomPhone, random.Next(10).ToString());

            return randomPhone;
        }
    }
}
