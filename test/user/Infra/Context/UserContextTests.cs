using Microsoft.EntityFrameworkCore;
using User.Infra.Context;

namespace UserTest.Infra.Context
{
    [TestFixture]
    public class UserContextTests
    {
        [Test]
        public void Ensure_DbContext_CanBeCreated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Act
            using (var context = new UserContext(options))
            {
                // Assert
                Assert.IsNotNull(context);
            }
        }

        [Test]
        public void Ensure_ClientsDbSet_IsAvailable()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Act
            using (var context = new UserContext(options))
            {
                // Assert
                Assert.IsNotNull(context.Clients);
            }
        }
    }
}
