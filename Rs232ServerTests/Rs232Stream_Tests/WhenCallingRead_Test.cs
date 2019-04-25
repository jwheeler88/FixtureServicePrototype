using System.Text;
using Moq;
using NUnit.Framework;

[Category("RS232Server")]
public class WhenCallingRead
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ReturnsMessageWithMatchingHeaderAsInitialDataReceived()
    {
        // Arrange
        var data = "Hello, world!";
        byte[] bytes = Encoding.ASCII.GetBytes(data);

        ITranslate translator = Mock.Of<ITranslate>(t =>
            t.Translate(bytes) == data);

        var sut = new Rs232Stream(translator);
            
        // Act
        Message message = sut.Read(bytes);

        // Assert
        Assert.AreEqual(data, message.Header);
    }
}

public interface ITranslate
{
    string Translate(byte[] bytes);
    byte[] Translate(string messageHeader);
}

public class Message
{
    public string Header { get; }
    
    public Message(string header) => Header = header;

    public bool IsEmpty => string.IsNullOrEmpty(Header);
}

public class Rs232Stream
{
    private readonly ITranslate _translator;

    public Rs232Stream(ITranslate translator)
    {
        _translator = translator;
    }

    public Message Read(byte[] bytes)
    {
        string header = _translator.Translate(bytes);
        var msg = new Message(header);
            
        return msg;
    }

    public void Write(Message message)
    {
        _translator.Translate(message.Header);
    }
}