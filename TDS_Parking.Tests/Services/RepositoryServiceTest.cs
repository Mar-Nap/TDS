using TDS_Parking.Repository;

namespace TDS_Parking.Tests.Services
{
    public class RepositoryServiceTest
    {

        [Fact]
        public void GetParkingSlots_ReturnsRepositoryCounts()
        {
            var repository = new ParkingRepository();
            var emptySlots = repository.GetAvailableSpacesCount();
            var busySlots = repository.GetParkedCarsCount();

            Assert.Equal(8, emptySlots);
            Assert.Equal(0, busySlots);

            repository.AssignParkingEntryToSlot(1, 1);

            emptySlots = repository.GetAvailableSpacesCount();
            busySlots = repository.GetParkedCarsCount();

            Assert.Equal(7, emptySlots);
            Assert.Equal(1, busySlots);
        }
    }
}
