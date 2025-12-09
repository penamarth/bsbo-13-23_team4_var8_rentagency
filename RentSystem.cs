using System;
using System.Collections.Generic;
using RentalSystem.Models;
using RentalSystem.States;

namespace RentalSystem.SequenceDemo
{
    public class RentSystem
    {
        private readonly List<Property> _properties;

        public RentSystem(List<Property> properties)
        {
            _properties = properties;
        }

        public List<Property> Search(Tenant tenant, string criteria)
        {
            Console.WriteLine("1: Арендатор задаёт фильтры (район, цена, комнаты)");

            Console.WriteLine("2: RentSystem → Tenant: searchApartments(criteria)");

            // имитация вызова tenant.searchApartments
            Console.WriteLine("3: Tenant → Property: запрос к коллекции Property (фильтрация по критериям)");

            Console.WriteLine("4: SELECT * FROM properties " +
                              "WHERE address LIKE criteria AND price <= maxPrice AND status = 'AVAILABLE'");

            Console.WriteLine("5: результаты запроса");

            // Фильтрация — имитируем
            var result = new List<Property>();
            foreach (var property in _properties)
            {
                // Вывод шагов цикла, как на диаграмме
                Console.WriteLine("loop: для каждого найденного Property");

                Console.WriteLine("6: Property.getDetails()");
                Console.WriteLine("    → " + property.GetDetails());

                Console.WriteLine("7: IState.getName()");
                Console.WriteLine("    → статус: " + property.Status.GetName());

                result.Add(property);
            }

            Console.WriteLine("8: Tenant ← List<Property>");
            Console.WriteLine("9: RentSystem ← List<Property>");

            if (result.Count > 0)
            {
                Console.WriteLine("10: Найдены подходящие объекты → отображение списка вариантов");
            }
            else
            {
                Console.WriteLine("11: Сообщение: 'Нет подходящих объектов'");
            }

            return result;
        }
    }
}
