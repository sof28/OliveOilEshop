namespace ModularCommerce.Features.ContactFeature.SubmitContact;

public sealed record SubmitContactRequest(string Name, string Email, string? Phone, string Message, bool ConsentGiven);
