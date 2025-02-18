using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.SensitiveWord;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FlashApp.Application.SensitiveWords.Add;
internal sealed class AddWordRequestHandler(ISensitiveWordRepository repository, ILogger<AddWordRequestHandler> logger) : IRequestHandler<AddWordRequest, Result<AddWordResponse>>
{
    public async Task<Result<AddWordResponse>> Handle(AddWordRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var newWord = SensitiveWord.Create(request.Word);

            await repository.AddAsync(newWord, cancellationToken);

            return Result.Success(new AddWordResponse(newWord.Id));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex?.Message, nameof(AddWordRequestHandler));
            return Result.Failure<AddWordResponse>(UserErrors.AddWordFailure);
        }
    }
}
