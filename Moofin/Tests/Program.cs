using Moofin.Core.Services;

namespace example
{
    public class Program
    {
        public static void Main()
        {
            var service = new FlashcardService();
            service.AddFlashcard("Capital Italy?", "Rome");
            var card = service.GetNextDueCard();
            if (card != null)
            {
                service.UpdateCardProgress(card.Id, true);
                var stats = service.GetStats();
            }
        }
    }
}