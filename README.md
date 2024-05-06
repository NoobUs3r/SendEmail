# Email Sender Library

## Overview
This library provides functionality to send emails using a specified API endpoint. It is designed to simplify the process of sending emails programmatically by abstracting away the complexities of HTTP requests and JSON serialization.

## Features
- Send emails with text and/or HTML content.
- Add attachments to emails.
- Specify sender and recipient information.
- Customize HTTP headers and endpoint URL.

## Usage

### Installation
To use this library, follow these steps:
1. Include the `EmailSenderLibrary` namespace in your project.
2. Instantiate an `EmailSender` object with the base URL of the email sending API and an authentication token.

### Sending Emails
To send an email, use the `SendAsync` method of the `EmailSender` class. Provide the necessary parameters such as sender, recipient, subject, text, and optionally HTML content and attachments.

### Example
```csharp
using EmailSenderLibrary;

// Instantiate EmailSender with base URL and authentication token
var emailSender = new EmailSender("https://api.example.com/", "your_auth_token");

// Prepare sender, recipient, and email content
var sender = new Sender("sender@example.com", "Sender Name");
var recipient = new Recipient("recipient@example.com", "Recipient Name");
var subject = "Test Email";
var text = "This is a test email sent using EmailSenderLibrary.";

// Send email without attachments
var response = await emailSender.SendAsync(sender, recipient, subject, text);
