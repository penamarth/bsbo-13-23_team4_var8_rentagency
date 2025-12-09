// Program.cs
using RentalSystem.Models;
using RentalSystem.Services;
using RentalSystem.States;
using RentalSystem.Strategies;

// ========================================
// 🏠 ДЕМОНСТРАЦИЯ: СИСТЕМА АРЕНДЫ ЖИЛЬЯ
// ========================================
Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine("🏠 RENTAL SYSTEM — Демонстрация архитектуры с паттернами");
Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine();

// ----------------------------------------
// 1. Создание владельца (Owner)
// ----------------------------------------
var owner = new Owner
{
    Id = 1,
    Name = "Анна Петрова",
    Email = "anna.owner@example.com",
    Password = "secure123",
    Role = UserRole.Owner,
    Rating = 4.8
};

var apartment = owner.AddProperty("Москва, ул. Тверская, д. 15, кв. 42", 85_000, PropertyType.Apartment);
apartment.Area = 45.5;
apartment.Description = "Современная 1-комнатная квартира в центре";

var house = owner.AddProperty("Подмосковье, пос. Ильинское, ул. Лесная, 8", 120_000, PropertyType.House);
house.Area = 120.0;
house.Description = "Деревянный дом с участком";

Console.WriteLine($"✅ Владелец создан: {owner.Name} (роль: {owner.Role})");
Console.WriteLine($"   Добавлены объекты:");
Console.WriteLine($"   • {apartment.GetDetails()}");
Console.WriteLine($"   • {house.GetDetails()}");
Console.WriteLine();

// ----------------------------------------
// 2. Создание арендатора (Tenant)
// ----------------------------------------
var tenant = new Tenant
{
    Id = 2,
    Name = "Иван Сидоров",
    Email = "ivan.renter@example.com",
    Password = "mypassword",
    Role = UserRole.Tenant,
    Preferences = new List<string> { "центр", "метро", "ремонт" }
};

Console.WriteLine($"👤 Арендатор: {tenant.Name} (роль: {tenant.Role})");
Console.WriteLine($"   Предпочтения: {string.Join(", ", tenant.Preferences)}");
Console.WriteLine();

// Сценарий: Поиск жилья арендатором

Console.WriteLine();
Console.WriteLine("СЦЕНАРИЙ: Поиск жилья арендатором");
Console.WriteLine("----------------------------------------");

var rentSystem = new RentalSystem.Models.RentSystem(new List<Property> { apartment, house });

Console.WriteLine("➡ Ввод критериев поиска: \"Москва\"");
var found = rentSystem.Search(tenant, "Москва");

Console.WriteLine();
Console.WriteLine("Результаты поиска:");
if (found.Count > 0)
{
    foreach (var p in found)
        Console.WriteLine($" • {p.GetDetails()} (статус: {p.Status.GetName()})");
}
else
{
    Console.WriteLine("Нет подходящих объектов.");
}

Console.WriteLine("----------------------------------------");
Console.WriteLine();

// ----------------------------------------
// 3. Подача заявки на квартиру
// ----------------------------------------
Console.WriteLine("📝 ШАГ 1: Подача заявки на аренду квартиры...");
var application = tenant.SubmitApplication(apartment);

if (application == null)
{
    Console.WriteLine("❌ Не удалось подать заявку.");
    return;
}

Console.WriteLine($"✅ Заявка создана: ID={application.Id}");
Console.WriteLine($"   Статус заявки: {application.Status.GetName()} (финальный: {application.Status.IsFinal()})");
Console.WriteLine($"   Объект: {application.Property.Address}");
Console.WriteLine();

// ----------------------------------------
// 4. Оплата через Stripe
// ----------------------------------------
Console.WriteLine("💳 ШАГ 2: Оплата через Stripe...");
var stripeStrategy = new StripePaymentStrategy();
bool stripeSuccess = tenant.MakePayment(application, stripeStrategy);

Console.WriteLine($"   Результат: {(stripeSuccess ? "УСПЕХ" : "ОШИБКА")}");
Console.WriteLine($"   Статус заявки после оплаты: {application.Status.GetName()}");
Console.WriteLine();

// ----------------------------------------
// 5. Повторная попытка — новая заявка, оплата через PayPal
// ----------------------------------------
Console.WriteLine("🔁 ШАГ 3: Новая заявка — оплата через PayPal...");

var newApplication = tenant.SubmitApplication(house);
if (newApplication != null)
{
    var paypalStrategy = new PayPalPaymentStrategy();
    bool paypalSuccess = tenant.MakePayment(newApplication, paypalStrategy);
    
    Console.WriteLine($"   Результат PayPal: {(paypalSuccess ? "УСПЕХ" : "ОШИБКА")}");
    Console.WriteLine($"   Статус заявки: {newApplication.Status.GetName()}");
    Console.WriteLine();
}

// ----------------------------------------
// 6. Третья заявка — через YooKassa (для российского рынка)
// ----------------------------------------
Console.WriteLine("🇷🇺 ШАГ 4: Заявка на квартиру — оплата через ЮKassa...");

var thirdApplication = tenant.SubmitApplication(apartment);
if (thirdApplication != null)
{
    var yooStrategy = new YooKassaPaymentStrategy();
    bool yooSuccess = tenant.MakePayment(thirdApplication, yooStrategy);
    
    Console.WriteLine($"   Результат ЮKassa: {(yooSuccess ? "УСПЕХ" : "ОШИБКА")}");
    Console.WriteLine($"   Статус заявки: {thirdApplication.Status.GetName()}");
    Console.WriteLine();
}

// ----------------------------------------
// 7. Проверка статусов через IState
// ----------------------------------------
Console.WriteLine("🔍 ШАГ 5: Проверка статусов через единый интерфейс IState");
Console.WriteLine($"   Статус квартиры: {apartment.Status.GetName()} (финальный: {apartment.Status.IsFinal()})");
Console.WriteLine($"   Статус заявки #1: {application.Status.GetName()} (финальный: {application.Status.IsFinal()})");
Console.WriteLine($"   Статус оплаты: SUCCESS → финальный = {new PaymentStatus(PaymentStatusValue.Success).IsFinal()}");
Console.WriteLine();

// ----------------------------------------
// 8. Добавление комментария к объекту
// ----------------------------------------
Console.WriteLine("💬 ШАГ 6: Добавление комментария к объекту недвижимости...");

var comment = new Comment
{
    Id = "c1",
    Text = "Отличная квартира! Быстро сдали.",
    Date = DateTime.Now,
    Author = tenant,
    Property = apartment
};

if (comment.AddComment())
{
    apartment.Comments.Add(comment);
    Console.WriteLine($"✅ Комментарий от {comment.Author.Name}: \"{comment.Text}\"");
}
Console.WriteLine();

// ----------------------------------------
// 9. Попытка подать заявку на уже сданную квартиру (ошибка)
// ----------------------------------------
// Принудительно ставим статус "сдано"
apartment.Status = new PropertyStatus(PropertyStatusValue.Rented);

Console.WriteLine("⚠️  ШАГ 7: Попытка подать заявку на уже сданную квартиру...");
var failedApp = tenant.SubmitApplication(apartment);
if (failedApp == null)
{
    Console.WriteLine("❌ Система корректно отклонила заявку: объект уже сдан в аренду.");
}
Console.WriteLine();

// ----------------------------------------
// 10. Сводка
// ----------------------------------------
Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine("✅ ДЕМОНСТРАЦИЯ ЗАВЕРШЕНА");
Console.WriteLine("=".PadRight(60, '='));
Console.WriteLine();
Console.WriteLine("📌 Архитектурные особенности, продемонстрированные в коде:");
Console.WriteLine("   • Ролевая модель: User → Tenant/Owner (нет класса Administrator)");
Console.WriteLine("   • Единый интерфейс статусов: IState");
Console.WriteLine("   • Паттерн Стратегия: 3 платёжные системы через общий интерфейс");
Console.WriteLine("   • Унифицированная модель недвижимости: Property + PropertyType");
Console.WriteLine("   • Инкапсуляция логики: заявка → оплата → обновление статусов");
Console.WriteLine();

// Пауза перед выходом
Console.WriteLine("Нажмите любую клавишу для выхода...");
Console.ReadKey();