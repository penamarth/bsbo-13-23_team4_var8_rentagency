using RentalSystem.States;

namespace RentalSystem.Models
{
    public class Administrator : User
    {
        public List<Property> ModeratedProperties { get; set; } = new();

        // Авторизация администратора
        public override bool Login()
        {
            if (!base.Login())
                return false;

            if (!IsAdmin())
            {
                Console.WriteLine("❌ Пользователь не является администратором");
                return false;
            }

            Console.WriteLine($"✅ Администратор {Name} успешно авторизован");
            return true;
        }

        // Получить всех пользователей
        public List<User> GetAllUsers(List<User> users)
        {
            Console.WriteLine("📋 Получение списка всех пользователей...");
            return users;
        }

        // Получить детали пользователя
        public User? GetUserDetails(List<User> users, int userId)
        {
            Console.WriteLine($"🔍 Поиск пользователя с ID={userId}...");
            var user = users.FirstOrDefault(u => u.Id == userId);
            
            if (user != null)
            {
                Console.WriteLine($"✅ Пользователь найден: {user.Name} ({user.Email})");
                Console.WriteLine($"   Статус: {user.Status.GetName()}");
                Console.WriteLine($"   Роль: {user.Role}");
            }
            else
            {
                Console.WriteLine("❌ Пользователь не найден");
            }
            
            return user;
        }

        // Изменить статус пользователя (блокировка/разблокировка/удаление)
        public bool ChangeUserStatus(User targetUser, UserStatusValue newStatus, string reason)
        {
            if (targetUser == null)
            {
                Console.WriteLine("❌ Целевой пользователь не указан");
                return false;
            }

            Console.WriteLine($"\n🔄 Попытка изменения статуса пользователя {targetUser.Name} на {newStatus}...");

            // Проверяем возможность перехода
            if (targetUser.Status is UserStatus currentStatus && currentStatus.CanTransitionTo(newStatus))
            {
                // Создаем новое состояние
                IState newState = newStatus switch
                {
                    UserStatusValue.Active => new UserActive(),
                    UserStatusValue.Blocked => new UserBlocked(),
                    UserStatusValue.Deleted => new UserDeleted(),
                    _ => throw new ArgumentException($"Неизвестный статус: {newStatus}")
                };

                targetUser.Status = newState;
                Console.WriteLine($"✅ Статус пользователя {targetUser.Name} успешно изменен на {targetUser.Status.GetName()}");

                // Логируем действие
                var log = new UserActionLog(
                    action: $"ChangeStatus to {newStatus}",
                    reason: reason,
                    adminId: this.Id,
                    targetUserId: targetUser.Id
                );
                Console.WriteLine(log.GetDetails());

                // Уведомляем пользователя
                targetUser.SendNotification($"Ваш статус был изменен на {targetUser.Status.GetName()} по причине: {reason}");

                return true;
            }
            else
            {
                Console.WriteLine($"❌ Невозможно изменить статус пользователя с {targetUser.Status.GetName()} на {newStatus}");
                return false;
            }
        }

        // Получить все объекты на модерации
        public List<Property> GetPropertiesUnderModeration(List<Property> allProperties)
        {
            Console.WriteLine("📋 Получение списка объектов на модерации...");
            var underModeration = allProperties
                .Where(p => p.Status is PropertyStatus propStatus && propStatus.Value == PropertyStatusValue.UnderModeration)
                .ToList();
            
            Console.WriteLine($"   Найдено объектов: {underModeration.Count}");
            return underModeration;
        }

        // Одобрить объект недвижимости
        public bool ApproveProperty(Property property)
        {
            if (property == null)
            {
                Console.WriteLine("❌ Объект недвижимости не указан");
                return false;
            }

            if (!(property.Status is PropertyStatus propStatus) || propStatus.Value != PropertyStatusValue.UnderModeration)
            {
                Console.WriteLine($"❌ Объект не находится на модерации (текущий статус: {property.Status.GetName()})");
                return false;
            }

            Console.WriteLine($"\n✅ Модерация объекта: {property.Address}");
            Console.WriteLine($"   Цена: {property.Price:C}");
            Console.WriteLine($"   Площадь: {property.Area} m²");
            Console.WriteLine($"   Описание: {property.Description}");

            property.Status = new PropertyStatus(PropertyStatusValue.Available);
            Console.WriteLine($"✅ Объект одобрен и переведен в статус: {property.Status.GetName()}");

            ModeratedProperties.Add(property);

            return true;
        }

        // Отклонить объект недвижимости
        public bool RejectProperty(Property property, string reason)
        {
            if (property == null)
            {
                Console.WriteLine("❌ Объект недвижимости не указан");
                return false;
            }

            if (!(property.Status is PropertyStatus propStatus) || propStatus.Value != PropertyStatusValue.UnderModeration)
            {
                Console.WriteLine($"❌ Объект не находится на модерации (текущий статус: {property.Status.GetName()})");
                return false;
            }

            Console.WriteLine($"\n❌ Отклонение объекта: {property.Address}");
            Console.WriteLine($"   Причина: {reason}");

            // Логируем действие
            var log = new UserActionLog(
                action: "Reject Property",
                reason: reason,
                adminId: this.Id,
                targetUserId: property.Owner?.Id ?? 0
            );
            Console.WriteLine(log.GetDetails());

            // Уведомляем владельца (если есть)
            if (property.Owner != null)
            {
                property.Owner.SendNotification($"Ваш объект {property.Address} был отклонен модератором. Причина: {reason}");
            }

            return true;
        }

        // Получить детали объекта недвижимости
        public Property? GetPropertyDetails(Property property)
        {
            if (property == null)
            {
                Console.WriteLine("❌ Объект недвижимости не указан");
                return null;
            }

            Console.WriteLine($"\n📋 Детали объекта недвижимости:");
            Console.WriteLine($"   Адрес: {property.Address}");
            Console.WriteLine($"   Тип: {property.Type}");
            Console.WriteLine($"   Цена: {property.Price:C}");
            Console.WriteLine($"   Площадь: {property.Area} m²");
            Console.WriteLine($"   Статус: {property.Status.GetName()}");
            Console.WriteLine($"   Описание: {property.Description}");
            if (property.Owner != null)
            {
                Console.WriteLine($"   Владелец: {property.Owner.Name} (ID: {property.Owner.Id})");
            }
            Console.WriteLine($"   Заявок: {property.Applications.Count}");
            Console.WriteLine($"   Комментариев: {property.Comments.Count}");

            return property;
        }
    }
}