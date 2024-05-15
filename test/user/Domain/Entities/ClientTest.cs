using User.Domain.Entities;
using User.Domain.Exceptions;

namespace UserTest.Domain.Entities
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void Validate_ClientWithValidData_ReturnsTrue()
        {
            // Arrange
            var client = new Client(
                name: "John Doe",
                email: "john.doe@example.com",
                password: "password123",
                cpfCnpj: "123.456.789-00",
                birth: new DateTime(1997, 2, 15),
                cnhNumber: "16941870601",
                cnhType: "AB",
                cnhImage: "https://www.shutterstock.com/image-photo/context-word-written-on-wood-260nw-1115628689.jpg",
                role: Client.EnumRole.User
            );

            // Act
            bool isValid = client.Validate();

            // Assert
            Assert.That(isValid, Is.True);
        }

        [Test]
        public void Validate_ClientWithInvalidData_ThrowsException()
        {
            // Arrange
            var client = new Client(
                name: "", // Invalid: Name cannot be empty
                email: "invalidemail", // Invalid: Invalid email format
                password: "password", // Invalid: Password too short
                cpfCnpj: "12345678900", // Invalid: CPF/CNPJ format
                birth: new DateTime(2024, 5, 15), // Invalid: Future birthdate
                cnhNumber: "", // Invalid: CNH Number cannot be empty
                cnhType: "", // Invalid: CNH Type cannot be empty
                cnhImage: "", // Invalid: CNH Image cannot be empty
                role: (Client.EnumRole)100 // Invalid: Unknown role
            );

            // Act & Assert
            Assert.Throws<PersonalizeExceptions>(() => client.Validate());
        }
    }
}
