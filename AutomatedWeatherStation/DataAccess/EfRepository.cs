using AutomatedWeatherStation.DataAccess.EF;

namespace AutomatedWeatherStation.DataAccess
{
    public interface IRepository
    {
        IDataService<Station> Stations { get; }
        IDataService<Measurement> Measurements { get; }
        IDataService<Sensor> Sensors { get; }
        IDataService<User> Users { get; }
    }

    public class EfRepository : IRepository
    {
        public IDataService<Station> Stations { get; } = new EfDataService<Station>();
        public IDataService<Measurement> Measurements { get; } = new EfDataService<Measurement>();
        public IDataService<Sensor> Sensors { get; } = new EfDataService<Sensor>();
        public IDataService<User> Users { get; } = new EfDataService<User>();
    }
}