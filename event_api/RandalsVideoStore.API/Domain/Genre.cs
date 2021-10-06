using System;

namespace RandalsVideoStore.API
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.flagsattribute?view=net-5.0
    // Welcome to the glorious world of bit-shifting.
    // Notice that doing it this way makes our swagger less readable as the enum values are only displayed as an integer.
    // Most of the times this is done for increased readability in the code and/or performance.
    // Consider don't using an enum and just work with value objects that are stored in the database.
    [Flags]
    public enum Genre
    {
        Adventure = 0,
        SciFi = 1,
        Drama = 2,
        Romance = 4,
        Mystery = 8,
        Thriller = 16,

    }
}
