using System.Text;
using NUnit.Framework;

namespace Tests
{
    //TODO: create a wrapper/simple adapter around nuget SerialPortStream
    public class FooTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ReadReturnsMessageWithMatchingHeaderAsInitialData()
        {
            // Arrange
            ITranslate msgTranslator = new MessageTranslator();
            var sut = new Foo(msgTranslator);
            
            var data = "Hello, world!";
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            
            // Act
            Message message = sut.Read(bytes);

            // Assert
            Assert.AreEqual(message.Header, data);
        }
    }

    public class MessageTranslator : ITranslate
    {
        public string Translate(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }

    public interface ITranslate
    {
        string Translate(byte[] bytes);
    }

    public class Message
    {
        public string Header { get; set; }
    }

    public class Foo
    {
        private readonly ITranslate _translator;

        public Foo(ITranslate translator)
        {
            _translator = translator;
        }

        public Message Read(byte[] bytes)
        {
            var msg = new Message();
            msg.Header = _translator.Translate(bytes);
            
            return msg;
        }
    }
}