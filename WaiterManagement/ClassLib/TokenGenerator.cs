namespace ClassLib
{
	public class TokenGenerator : ITokenGenerator
	{
		public string GetToken()
		{
			var currentTime = SystemTime.Now;

			return string.Format("{0}-{1}-{2}-{3}-{4}-{5}-{6}", currentTime.Year, currentTime.Month, currentTime.Day,
				currentTime.Hour, currentTime.Minute, currentTime.Second, currentTime.Millisecond);
		}
	}
}