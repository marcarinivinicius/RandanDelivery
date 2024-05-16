using Microsoft.EntityFrameworkCore;
using Moq;
using RabbitMq.Notify.Interfaces;
using RabbitMQ.Client;
using User.Infra.Context;
using User.Infra.Messages;

namespace UserTest.Infra.Messages
{
    [TestFixture]
    public class UserRpcListenerTests
    {
        [Test]
        public void Consume_ValidQueue_ConsumesMessages()
        {
            // Arrange
            var persistentConnectionMock = new Mock<IRabbitConnection>();
            var contextFactoryMock = new Mock<IDbContextFactory<UserContext>>();
            var channelMock = new Mock<IModel>();

            // Configurando o mock para retornar false quando IsConnected for chamado inicialmente
            persistentConnectionMock.SetupSequence(p => p.IsConnected)
                .Returns(false) // primeira chamada retorna false
                .Returns(true); // segunda chamada retorna true após TryConnect

            // Configurando o mock para chamar TryConnect quando IsConnected retornar false
            persistentConnectionMock.Setup(p => p.TryConnect()).Callback(() =>
            {
                // Mudando IsConnected para true após TryConnect ser chamado
                persistentConnectionMock.Setup(p => p.IsConnected).Returns(true);
            });

            // Configurando o mock para retornar o canal criado
            persistentConnectionMock.Setup(p => p.CreateChannel()).Returns(channelMock.Object);

            contextFactoryMock.Setup(c => c.CreateDbContext()).Returns(new UserContext());

            var rpcListener = new UserRpcListener(persistentConnectionMock.Object, null, contextFactoryMock.Object);

            // Act
            rpcListener.Consume("publishUser");

            // Assert
            // Verifica se o método TryConnect foi chamado
            persistentConnectionMock.Verify(p => p.TryConnect(), Times.Once);
            // Verifica se o método CreateChannel foi chamado
            persistentConnectionMock.Verify(p => p.CreateChannel(), Times.Once);

        }



    }
}
