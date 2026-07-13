using Microsoft.AspNetCore.Mvc.Rendering;
using SalonBookingSystem.Models;
using SalonBookingSystem.Models.Enums;
using SalonBookingSystem.Models.ViewModels;
using SalonBookingSystem.Repositories.Interfaces;
using SalonBookingSystem.Services.Interfaces;

namespace SalonBookingSystem.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IBeautyServiceRepository _beautyServiceRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public ReservationService(
            IReservationRepository reservationRepository,
            IBeautyServiceRepository beautyServiceRepository,
            IEmployeeRepository employeeRepository)
        {
            _reservationRepository = reservationRepository;
            _beautyServiceRepository = beautyServiceRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await _reservationRepository.GetAllAsync();
        }

        public async Task<ReservationFormViewModel> GetCreateModelAsync()
        {
            var services = await _beautyServiceRepository.GetAllAsync();

            var employees = await _employeeRepository.GetAllAsync();

            return new ReservationFormViewModel
            {
                ReservationDate = DateOnly.FromDateTime(DateTime.Today),

                StartTime = new TimeOnly(9, 0),

                Services = services
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Name
                    })
                    .ToList(),

                Employees = employees
                    .Select(e => new SelectListItem
                    {
                        Value = e.Id.ToString(),
                        Text = $"{e.User.FirstName} {e.User.LastName}"
                    })
                    .ToList()
            };
        }

        public async Task<List<SelectListItem>> GetEmployeesByServiceAsync(int serviceId)
        {
            var employees = await _employeeRepository
                .GetByServiceIdAsync(serviceId);

            return employees
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = $"{e.User.FirstName} {e.User.LastName}"
                })
                .ToList();
        }

        public async Task CreateAsync(int clientId, ReservationFormViewModel model)
        {
            Employee employee = await _employeeRepository.GetByIdAsync(model.EmployeeId);

            if (employee == null)
                throw new Exception("Служителят не съществува.");


            if (!employee.IsActive)
                throw new Exception("Служителят не работи в момента.");


            var service = await _beautyServiceRepository
                .GetByIdAsync(model.BeautyServiceId);

            if (service == null)
                throw new Exception("Услугата не съществува.");


            // Проверка дали служителят предлага услугата

            bool canPerform = employee.EmployeeServices
                .Any(es =>
                    es.BeautyServiceId == model.BeautyServiceId);


            if (!canPerform)
                throw new Exception(
                    "Този служител не извършва тази услуга.");


            // Проверка за уикенд

            DayOfWeek day =
                model.ReservationDate
                .ToDateTime(TimeOnly.MinValue)
                .DayOfWeek;


            if (day == DayOfWeek.Saturday ||
               day == DayOfWeek.Sunday)
            {
                throw new Exception(
                    "Салонът не работи през уикенда.");
            }


            // Изчисляване на крайния час

            TimeOnly endTime =
                model.StartTime.AddMinutes(service.DurationMinutes);


            // Проверка работно време

            if (model.StartTime < employee.WorkStart ||
               endTime > employee.WorkEnd)
            {
                throw new Exception(
                    "Часът е извън работното време.");
            }


            // Проверка за конфликт

            bool conflict =
                await _reservationRepository.HasConflictAsync(
                    employee.Id,
                    model.ReservationDate,
                    model.StartTime,
                    endTime);


            if (conflict)
            {
                throw new Exception(
                    "Този час вече е зает.");
            }


            Reservation reservation = new()
            {
                ClientId = clientId,

                EmployeeId = employee.Id,

                BeautyServiceId = service.Id,

                ReservationDate = model.ReservationDate,

                StartTime = model.StartTime,

                EndTime = endTime,

                Status = ReservationStatus.Reserved
            };


            await _reservationRepository
                .CreateAsync(reservation);
        }

        public async Task<List<Reservation>> GetClientReservationsAsync(int clientId)
        {
            return await _reservationRepository
                .GetByClientIdAsync(clientId);
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _reservationRepository.GetByIdAsync(id);
        }


        public async Task UpdateAsync(Reservation reservation)
        {
            await _reservationRepository.UpdateAsync(reservation);
        }

        public async Task ChangeStatusAsync(int id, ReservationStatus status)
        {
            var reservation = await _reservationRepository
                .GetByIdAsync(id);


            if (reservation == null)
                throw new Exception("Резервацията не съществува.");


            reservation.Status = status;


            await _reservationRepository
                .UpdateAsync(reservation);
        }

        public async Task<List<SelectListItem>> GetAvailableTimesAsync(int employeeId, int beautyServiceId, DateOnly date)
        {
            var employee = await _employeeRepository
                .GetByIdAsync(employeeId);

            if (employee == null)
                return new List<SelectListItem>();


            var service = await _beautyServiceRepository
                .GetByIdAsync(beautyServiceId);


            if (service == null)
                return new List<SelectListItem>();


            List<SelectListItem> times = new();


            // Почивен ден

            var day = date
                .ToDateTime(TimeOnly.MinValue)
                .DayOfWeek;


            if (day == DayOfWeek.Saturday ||
               day == DayOfWeek.Sunday)
            {
                return times;
            }


            TimeOnly current = employee.WorkStart;


            while (current.AddMinutes(service.DurationMinutes)
                  <= employee.WorkEnd)
            {

                bool occupied =
                    await _reservationRepository.HasConflictAsync(
                        employeeId,
                        date,
                        current,
                        current.AddMinutes(service.DurationMinutes));


                if (!occupied)
                {
                    times.Add(new SelectListItem
                    {
                        Value = current.ToString("HH:mm"),
                        Text = current.ToString("HH:mm")
                    });
                }


                current = current.AddMinutes(30);
            }


            return times;
        }

        public async Task<List<Reservation>> GetEmployeeReservationsAsync(int employeeId)
        {
            return (await _reservationRepository.GetByEmployeeIdAsync(employeeId))
                .Where(r => r.Status != ReservationStatus.Completed && 
                r.Status != ReservationStatus.Cancelled).ToList();
        }
    }
}
