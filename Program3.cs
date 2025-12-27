// Program3.cs - Сценарий модерации (со стороны администратора)
using RentalSystem.Models;
using RentalSystem.States;

namespace RentalSystem
{
    public class ModerationScenario
    {
        public static void Run()
        {
            // ========================================
            // 🔐 ДЕМОНСТРАЦИЯ: СЦЕНАРИИ МОДЕРАЦИИ
            // ========================================
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("🔐 RENTAL SYSTEM — Сценарии модерации");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            // ----------------------------------------
            // 1. Создание пользователей системы
            // ----------------------------------------
            var admin = new Administrator
            {
                Id = 1,
                Name = "Администратор Иван",
                Email = "admin@rentalsystem.com",
                Password = "admin123",
                Role = UserRole.Admin
            };

            var owner1 = new Owner
            {
                Id = 2,
                Name = "Анна Петрова",
                Email = "anna.owner@example.com",
                Password = "secure123",
                Role = UserRole.Owner,
                Rating = 4.8
            };

            var owner2 = new Owner
            {
                Id = 3,
                Name = "Михаил Сидоров",
                Email = "mikhail.owner@example.com",
                Password = "password456",
                Role = UserRole.Owner,
                Rating = 3.5
            };

            var tenant1 = new Tenant
            {
                Id = 4,
                Name = "Ольга Козлова",
                Email = "olga.renter@example.com",
                Password = "tenant789",
                Role = UserRole.Tenant,
                Preferences = new List<string> { "центр", "метро" }
            };

            // Создаем объекты недвижимости для модерации
            var property1 = new Property
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Москва, ул. Тверская, д. 15, кв. 42",
                Price = 85_000,
                Area = 45.5,
                Description = "Современная 1-комнатная квартира в центре",
                Type = PropertyType.Apartment,
                Status = new PropertyStatus(PropertyStatusValue.UnderModeration),
                Owner = owner1
            };

            var property2 = new Property
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Санкт-Петербург, Невский пр., д. 100",
                Price = 65_000,
                Area = 38.0,
                Description = "Квартира с видом на Неву",
                Type = PropertyType.Apartment,
                Status = new PropertyStatus(PropertyStatusValue.UnderModeration),
                Owner = owner2
            };

            var property3 = new Property
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Москва, ул. Арбат, д. 22",
                Price = 150_000,
                Area = 75.0,
                Description = "Премиальная квартира в историческом центре",
                Type = PropertyType.Apartment,
                Status = new PropertyStatus(PropertyStatusValue.Available),
                Owner = owner1
            };

            // Собираем список всех пользователей
            var allUsers = new List<User> { admin, owner1, owner2, tenant1 };

            // Собираем список всех объектов недвижимости
            var allProperties = new List<Property> { property1, property2, property3 };

            Console.WriteLine("👥 Пользователи системы созданы:");
            Console.WriteLine($"   • {admin.Name} (Администратор)");
            Console.WriteLine($"   • {owner1.Name} (Владелец, рейтинг: {owner1.Rating})");
            Console.WriteLine($"   • {owner2.Name} (Владелец, рейтинг: {owner2.Rating})");
            Console.WriteLine($"   • {tenant1.Name} (Арендатор)");
            Console.WriteLine();
            Console.WriteLine($"🏠 Объекты недвижимости: {allProperties.Count} (на модерации: {allProperties.Count(p => p.Status is PropertyStatus ps && ps.Value == PropertyStatusValue.UnderModeration)})");
            Console.WriteLine();

            // ----------------------------------------
            // 2. Авторизация администратора
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 1: Авторизация администратора");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ login(admin@rentalsystem.com, admin123)");
            var loginSuccess = admin.Login();
            Console.WriteLine();

            if (!loginSuccess)
            {
                Console.WriteLine("❌ Ошибка авторизации. Завершение работы.");
                return;
            }

            // ----------------------------------------
            // 3. Получение списка всех пользователей
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 2: Получение списка всех пользователей");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ getAllUsers()");
            var users = admin.GetAllUsers(allUsers);
            Console.WriteLine();
            Console.WriteLine("Список пользователей:");
            foreach (var user in users)
            {
                Console.WriteLine($"   • ID: {user.Id} | {user.Name} ({user.Email}) | Роль: {user.Role} | Статус: {user.Status.GetName()}");
            }
            Console.WriteLine();

            // ----------------------------------------
            // 4. Просмотр деталей пользователя
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 3: Просмотр деталей пользователя");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ getUserDetails(userId=3)");
            var userDetails = admin.GetUserDetails(allUsers, 3);
            Console.WriteLine();

            // ----------------------------------------
            // 5. Блокировка пользователя
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 4: Блокировка пользователя");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ changeState(UserBlocked)");
            var blockSuccess = admin.ChangeUserStatus(owner2, UserStatusValue.Blocked, "Нарушение правил публикации");
            Console.WriteLine();
            Console.WriteLine($"Текущий статус пользователя: {owner2.Name} - {owner2.Status.GetName()}");
            Console.WriteLine();

            // ----------------------------------------
            // 6. Разблокировка пользователя
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 5: Разблокировка пользователя");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ changeState(UserActive)");
            var unblockSuccess = admin.ChangeUserStatus(owner2, UserStatusValue.Active, "Предупреждение учтено, восстановление доступа");
            Console.WriteLine();
            Console.WriteLine($"Текущий статус пользователя: {owner2.Name} - {owner2.Status.GetName()}");
            Console.WriteLine();

            // ----------------------------------------
            // 7. Модерация объектов недвижимости
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 6: Модерация объектов недвижимости");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ getPropertiesUnderModeration()");
            var underModeration = admin.GetPropertiesUnderModeration(allProperties);
            Console.WriteLine();

            Console.WriteLine("Объекты на модерации:");
            foreach (var property in underModeration)
            {
                Console.WriteLine($"   • {property.Address} - {property.Price:C} - {property.Status.GetName()}");
            }
            Console.WriteLine();

            // ----------------------------------------
            // 8. Одобрение объекта недвижимости
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 7: Одобрение объекта недвижимости");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ approveProperty(property)");
            var approveSuccess = admin.ApproveProperty(property1);
            Console.WriteLine();
            Console.WriteLine($"Текущий статус объекта: {property1.Address} - {property1.Status.GetName()}");
            Console.WriteLine();

            // ----------------------------------------
            // 9. Отклонение объекта недвижимости
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 8: Отклонение объекта недвижимости");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ rejectProperty(property, reason)");
            var rejectSuccess = admin.RejectProperty(property2, "Некорректное описание объекта, требуется уточнение данных");
            Console.WriteLine();
            Console.WriteLine($"Текущий статус объекта: {property2.Address} - {property2.Status.GetName()}");
            Console.WriteLine();

            // ----------------------------------------
            // 10. Просмотр деталей объекта
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 9: Просмотр деталей объекта недвижимости");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ getPropertyDetails(property)");
            admin.GetPropertyDetails(property3);
            Console.WriteLine();

            // ----------------------------------------
            // 11. Удаление пользователя (финальное действие)
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("ШАГ 10: Удаление пользователя");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();

            Console.WriteLine("➡ changeState(UserDeleted)");
            var deleteSuccess = admin.ChangeUserStatus(tenant1, UserStatusValue.Deleted, "Удаление по запросу пользователя");
            Console.WriteLine();
            Console.WriteLine($"Текущий статус пользователя: {tenant1.Name} - {tenant1.Status.GetName()}");
            Console.WriteLine();

            // Попытка изменить статус удаленного пользователя
            Console.WriteLine("⚠️  Попытка изменить статус удаленного пользователя...");
            admin.ChangeUserStatus(tenant1, UserStatusValue.Active, "Попытка восстановления");
            Console.WriteLine();

            // ----------------------------------------
            // 12. Итоговая сводка
            // ----------------------------------------
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine("✅ СЦЕНАРИИ МОДЕРАЦИИ ЗАВЕРШЕНЫ");
            Console.WriteLine("=".PadRight(60, '='));
            Console.WriteLine();
            Console.WriteLine("📌 Реализованные сценарии:");
            Console.WriteLine("   • Авторизация администратора (login)");
            Console.WriteLine("   • Получение списка всех пользователей (getAllUsers)");
            Console.WriteLine("   • Просмотр деталей пользователя (getUserDetails)");
            Console.WriteLine("   • Изменение статуса пользователя (changeState):");
            Console.WriteLine("     - Блокировка пользователя (UserBlocked)");
            Console.WriteLine("     - Разблокировка пользователя (UserActive)");
            Console.WriteLine("     - Удаление пользователя (UserDeleted)");
            Console.WriteLine("   • Модерация объектов недвижимости:");
            Console.WriteLine("     - Получение списка на модерации");
            Console.WriteLine("     - Одобрение объекта (approveProperty)");
            Console.WriteLine("     - Отклонение объекта (rejectProperty)");
            Console.WriteLine("     - Просмотр деталей объекта");
            Console.WriteLine("   • Логирование действий администратора (UserActionLog)");
            Console.WriteLine("   • Уведомление пользователей о смене статуса");
            Console.WriteLine();

            // Статистика по объектам недвижимости
            Console.WriteLine("📊 Статистика по объектам недвижимости:");
            Console.WriteLine($"   Всего объектов: {allProperties.Count}");
            Console.WriteLine($"   Доступно: {allProperties.Count(p => p.Status is PropertyStatus ps && ps.Value == PropertyStatusValue.Available)}");
            Console.WriteLine($"   На модерации: {allProperties.Count(p => p.Status is PropertyStatus ps && ps.Value == PropertyStatusValue.UnderModeration)}");
            Console.WriteLine($"   Сдано: {allProperties.Count(p => p.Status is PropertyStatus ps && ps.Value == PropertyStatusValue.Rented)}");
            Console.WriteLine();

            // Статистика по пользователям
            Console.WriteLine("📊 Статистика по пользователям:");
            Console.WriteLine($"   Всего пользователей: {allUsers.Count}");
            Console.WriteLine($"   Активных: {allUsers.Count(u => u.Status is UserStatus us && us.Value == UserStatusValue.Active)}");
            Console.WriteLine($"   Заблокированных: {allUsers.Count(u => u.Status is UserStatus us && us.Value == UserStatusValue.Blocked)}");
            Console.WriteLine($"   Удаленных: {allUsers.Count(u => u.Status is UserStatus us && us.Value == UserStatusValue.Deleted)}");
            Console.WriteLine();

            // Пауза перед выходом
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}