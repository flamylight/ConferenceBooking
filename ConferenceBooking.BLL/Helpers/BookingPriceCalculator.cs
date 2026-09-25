namespace ConferenceBooking.BLL.Helpers;

public static class BookingPriceCalculator
{
    public static decimal CalculateTotalPrice(
        decimal baseHourlyPrice,
        DateTime start,
        DateTime end,
        IEnumerable<decimal> amenityPrices)
    {
        decimal roomCost = 0;

        for (var i = start; i < end; i = i.AddHours(1))
        {
            int hour = i.Hour;

            decimal multiplier = hour switch
            {
                >= 6 and < 9 => 0.90m, 
                >= 12 and < 14 => 1.15m, 
                >= 18 and < 23 => 0.80m, 
                _ => 1.0m
            };
            
            roomCost += baseHourlyPrice * multiplier;
        }
        
        decimal amenitiesCost = amenityPrices.Sum();
        
        return roomCost + amenitiesCost;
    }
}