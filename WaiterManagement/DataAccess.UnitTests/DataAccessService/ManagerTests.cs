using System.Data.Entity;
using System.Linq;
using ClassLib;
using ClassLib.DataStructures;
using ClassLib.DbDataStructures;
using DataAccess.Migrations;
using Moq;
using Xunit;

namespace DataAccess.UnitTests.DataAccessService
{
	public class ManagerTests
	{
		#region Mocks

		private readonly Mock<ITokenGenerator> _tokenGeneratorMock;

		#endregion

		public DataAccessClass Instance
		{
			get
			{
				var instance = new DataAccessClass {TokenGenerator = _tokenGeneratorMock.Object};
				return instance;
			}
		}

		public ManagerTests()
		{
			//Upewnienie, że baza posiada najnowszy model danych
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>());

			using (var db = new DataAccessProvider())
			{
				db.Database.ExecuteSqlCommand("DELETE FROM MenuItemQuantityEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM OrderEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM UserContextEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM PasswordEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM MenuItemEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM MenuItemCategoryEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM TableEntities");
			}

			_tokenGeneratorMock = new Mock<ITokenGenerator>();
			_tokenGeneratorMock.Setup(x => x.GetToken()).Returns("xxx");
		}

		[Theory]
		[InlineData("Alojzy","Szczesny","terminator","lol123")]
		public void add_manager(string firstName,string lastName,string login,string password)
		{
			// Act
			Instance.AddManager(firstName,lastName,login,password);

			// Assert
			using (var db = new DataAccessProvider())
			{
				var user = db.Users.First();
				Assert.Equal(login,user.Login);
				Assert.Equal(firstName, user.FirstName);
				Assert.Equal(lastName, user.LastName);
				Assert.Equal(UserRole.Manager,user.Role);
			}
		}

		[Fact]
		public void login_manager_get_token_passed()
		{
			// Arrange
			using (var db = new DataAccessProvider())
			{
				var user = db.Users.Add(new UserContextEntity()
				{
					FirstName = "Alojzy",
					LastName = "Szczęsny",
					Login = "terminator",
					Role = UserRole.Manager
				});

				db.SaveChanges();

				db.Passwords.Add(new PasswordEntity()
				{
					Hash = "500:OjcNTBDqY095nHMRCmu66g==:n4uegxkcetWy7aZ2tSbjsBlhJUClcdU/",
					UserId = user.Id
				});

				db.SaveChanges();
			}

			_tokenGeneratorMock.Setup(x => x.GetToken()).Returns("LoginManagerLogintTestToken");

			// Act
			var token = Instance.LogIn("terminator", "500:testlollllllllll:t66LAkO0vAa02JRh3bNL6BTlyY1zrUvt");

			// Assert
			Assert.Equal("LoginManagerLogintTestToken",token);
		}

		
	}
}