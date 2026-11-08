using ModularCommerce.Abstractions;
using ModularCommerce.Entities;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.ContactFeature.SubmitContact;

public sealed class SubmitContactHandler(
    IRepository<ContactMessage> contactRepository,
    IUnitOfWork unitOfWork) : IHandler<SubmitContactRequest, Result<SubmitContactResponse>>
{
    public async Task<Result<SubmitContactResponse>> HandleAsync(
        SubmitContactRequest command, CancellationToken cancellationToken)
    {
        var message = new ContactMessage
        {
            Name = command.Name.Trim(),
            Email = command.Email.Trim(),
            Phone = string.IsNullOrWhiteSpace(command.Phone) ? null : command.Phone.Trim(),
            Message = command.Message.Trim(),
            ConsentGiven = command.ConsentGiven
        };

        await contactRepository.AddAsync(message, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(new SubmitContactResponse(message.Id, message.CreatedAt));
    }
}
