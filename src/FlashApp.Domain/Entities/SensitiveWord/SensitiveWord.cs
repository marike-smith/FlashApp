using FlashApp.Domain.Entities.Abstractions;
using FlashApp.Domain.Entities.SensitiveWord.Events;
using FlashApp.Domain.Shared.ValueObjects;

namespace FlashApp.Domain.Entities.SensitiveWord
{
    public sealed class SensitiveWord: Entity
    {
        private SensitiveWord(Word value)
        {
        }

        public new static SensitiveWord Create(Word value)
        {
            var word = new SensitiveWord(value);

            DomainEvents.RaiseEvent(new SensitiveWordCreatedDomainEvent(word.Id));

            return word;
        }
    }
}
