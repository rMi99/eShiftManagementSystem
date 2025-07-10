using eShiftManagementSystem.Models;
using System.Collections.Generic;

namespace eShiftManagementSystem.DataAccess.Interfaces
{
    public interface IQuoteRepository
    {
        /// <summary>
        /// Retrieves a quote by its unique identifier.
        /// </summary>
        /// <param name="quoteId">The ID of the quote to retrieve.</param>
        /// <returns>A Quote object if found; otherwise, null.</returns>
        Quote GetQuoteById(int quoteId);

        /// <summary>
        /// Retrieves all quotes from the database.
        /// </summary>
        /// <returns>A list of all Quote objects.</returns>
        List<Quote> GetAllQuotes();

        /// <summary>
        /// Adds a new quote to the database.
        /// </summary>
        /// <param name="quote">The Quote object to add.</param>
        /// <returns>The ID of the newly created quote.</returns>
        int AddQuote(Quote quote);

        /// <summary>
        /// Updates an existing quote in the database.
        /// </summary>
        /// <param name="quote">The Quote object with updated information.</param>
        void UpdateQuote(Quote quote);

        /// <summary>
        /// Deletes a quote from the database.
        /// </summary>
        /// <param name="quoteId">The ID of the quote to delete.</param>
        void DeleteQuote(int quoteId);
    }
}