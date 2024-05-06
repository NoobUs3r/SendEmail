using System.Text;
using System.Text.Json;
using EmailSenderLibrary.Models;

namespace EmailSenderLibrary;

public class EmailSender
{
    private readonly HttpClient _httpClient;

    public EmailSender(string baseUrl, string authToken)
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(baseUrl);
        _httpClient.DefaultRequestHeaders.Add("Authorization", authToken);
    }

    public async Task<HttpResponseMessage> SendAsync(Sender emailSender,
                                                     Recipient emailRecipient,
                                                     string emailSubject,
                                                     string emailText,
                                                     string emailHtml = "",
                                                     List<string>? emailAttachments = null,
                                                     CancellationToken cancellationToken = default)
    {
        var attachmentObjects = EncodeAttachmentsFromPaths(emailAttachments);

        var request = new
        {
            from = new { email = emailSender.Email, name = emailSender.Name },
            to = new[] { new { email = emailRecipient.Email, name = emailRecipient.Name } },
            subject = emailSubject,
            text = emailText,
            html = emailHtml,
            attachments = attachmentObjects
        };

        var jsonRequest = JsonSerializer.Serialize(request);
        var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
        
        return await _httpClient.PostAsync("send", content, cancellationToken).ConfigureAwait(false);
    }

    private static List<object> EncodeAttachmentsFromPaths(List<string>? emailAttachments)
    {
        var attachmentObjects = new List<object>();

        if (emailAttachments == null)
            return attachmentObjects;

        foreach (var attachmentPath in emailAttachments)
        {
            if (!File.Exists(attachmentPath))
            {
                throw new FileNotFoundException($"Attachment not found: {attachmentPath}");
            }

            var fileBytes = File.ReadAllBytes(attachmentPath);
            var base64Content = Convert.ToBase64String(fileBytes);
            var attachment = new
            {
                content = base64Content,
                filename = Path.GetFileName(attachmentPath),
                type = "application/octet-stream",
                disposition = "attachment"
            };

            attachmentObjects.Add(attachment);
        }

        return attachmentObjects;
    }
}