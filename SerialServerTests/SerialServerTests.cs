using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

public class SerialServerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ReadReturnsMessageWithMatchingHeaderAsInitialDataReceived()
    {
        // Arrange
        var data = "Hello, world!";
        byte[] bytes = Encoding.ASCII.GetBytes(data);

        ITranslate translator = Mock.Of<ITranslate>(t =>
            t.Translate(bytes) == data);

        var sut = new SerialServer(translator);
            
        // Act
        Message message = sut.Read(bytes);

        // Assert
        Assert.AreEqual(data, message.Header);
    }

    [Test]
    public void ReadSuccessfullyCallsInternalSerialPortStreamReadMethod()
    {
        // Arrange
        byte[] bytes = Enumerable.Empty<byte>().ToArray();
            
        ITranslate translator = Mock.Of<ITranslate>(t =>
            t.Translate(bytes) == string.Empty);
            
        var sut = new SerialServer(translator);
            
        // Act
        Message message = sut.Read(bytes);

        // Assert
        Mock.Get(translator).Verify(t => t.Translate(bytes));
    }

    [Test]
    public void ReadReturnsAnEmptyMessageWhenNoDataReceived()
    {
        //Arrange
        var bytes = Enumerable.Empty<byte>().ToArray();

        ITranslate translator = Mock.Of<ITranslate>(t =>
            t.Translate(bytes) == string.Empty);
        
        var sut = new SerialServer(translator);
        
        //Act
        Message message = sut.Read(bytes);

        //Assert
        Assert.IsTrue(message.IsEmpty);
    }
}

public interface ITranslate
{
    string Translate(byte[] bytes);
}

public class Message
{
    public string Header { get; }
    
    public Message(string header) => Header = header;

    public bool IsEmpty => string.IsNullOrEmpty(Header);
}

public class SerialServer
{
    private readonly ITranslate _translator;

    public SerialServer(ITranslate translator)
    {
        _translator = translator;
    }

    public Message Read(byte[] bytes)
    {
        string header = _translator.Translate(bytes);
        var msg = new Message(header);
            
        return msg;
    }
}