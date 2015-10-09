using System;
using Xunit;

namespace ClassLib.Tests
{
	public class TokenGeneratorTest : IDisposable
	{
		[Fact]
		public void generate_token()
		{
			// Arrange
			var time = new DateTime(2015, 10, 09, 23, 01, 03);
			time = time.AddMilliseconds(123);
			SystemTime.SetTime(time);

			// Act
			var token = new TokenGenerator().GetToken();

			// Assert
			Assert.Equal("2015-10-9-23-1-3-123", token);
		}


		public void Dispose()
		{
			SystemTime.Reset();
		}
	}
}