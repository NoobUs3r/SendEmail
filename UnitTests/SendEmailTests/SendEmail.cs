using EmailSenderLibrary;
using EmailSenderLibrary.Models;

namespace UnitTests.SendEmailTests;

using System.Net;

[TestClass]
public class SendEmailTests
{
    private const string AuthToken = "Bearer 381b7ca7c67a29d0b8c6f9d27746b5e5";
    private const string SendEmailBaseUrl = "https://send.api.mailtrap.io/api/";

    private static readonly Sender TestEmailSender = new("Test Sender", "mailtrap@demomailtrap.com");
    private static readonly Recipient TestEmailRecipient = new("Test Recipient", "kochubeymisha@gmail.com");

    [TestMethod]
    public async Task Should_Send_Email()
    {
        var emailSenderAgent = new EmailSender(SendEmailBaseUrl, AuthToken);
        var subject = "Send email unit test";
        var text = "Some text here.";

        var sendEmailResponse = await emailSenderAgent.SendAsync(TestEmailSender, TestEmailRecipient, subject, text).ConfigureAwait(false);
        Assert.AreEqual(HttpStatusCode.OK, sendEmailResponse.StatusCode, "Failed to send email.");
    }

    [TestMethod]
    public async Task Should_Send_Email_With_Attachment()
    {
        var emailSenderAgent = new EmailSender(SendEmailBaseUrl, AuthToken);
        var subject = "Send email unit test";
        var text = "Some text here.";
        var attachments = new List<string> { "SendEmailTests/Attachments/space.jpg" };

        var sendEmailResponse = await emailSenderAgent.SendAsync(TestEmailSender, TestEmailRecipient, subject, text, emailAttachments: attachments).ConfigureAwait(false);
        Assert.AreEqual(HttpStatusCode.OK, sendEmailResponse.StatusCode, "Failed to send email.");
    }

    [TestMethod]
    public async Task Should_Send_Email_With_All_Optional_Parameters()
    {
        var emailSenderAgent = new EmailSender(SendEmailBaseUrl, AuthToken);
        var subject = "Send email unit test";
        var text = "Some text here.";
        var html = "<p>This is a <b>test</b> email sent via API.</p>";
        var attachments = new List<string> { "SendEmailTests/Attachments/space.jpg" };

        var sendEmailResponse = await emailSenderAgent.SendAsync(TestEmailSender, TestEmailRecipient, subject, text, html, attachments).ConfigureAwait(false);
        Assert.AreEqual(HttpStatusCode.OK, sendEmailResponse.StatusCode, "Failed to send email.");
    }

    [TestMethod]
    public async Task Should_Fail_Sending_Email_To_Non_Existing_Email()
    {
        var emailSenderAgent = new EmailSender(SendEmailBaseUrl, AuthToken);
        var subject = "You are awesome!";
        var text = "Congrats for sending test email with Mailtrap!";
        var nonExistingRecipient = new Recipient("Non Existing", "non-existing-email@non-existing.com");

        var sendEmailResponse = await emailSenderAgent.SendAsync(TestEmailSender, nonExistingRecipient, subject, text).ConfigureAwait(false);
        Assert.AreEqual(HttpStatusCode.Forbidden, sendEmailResponse.StatusCode, "Different status code with non-existing email.");
    }
}