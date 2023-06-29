# Что умеет?

* Менеджер контактов. Позволяет зарегистрироваться и хранить свои контакты
* Функция импорта и экспорта в файл
* Возможность генерации тестовых контактов
* Веб и десктоп приложение
* Админ имеет возможность просмотра данных пользователей

# Из чего сделано?

* CQRS
* ASP NET Core 6
* EF Core 6 (SQLite)
* MediatR
* Automapper
* Web API
* Swagger
* Отдельный IdentityServer4
* MVC
* WPF

# Как запустить?

* Открыть [Contacts.sln](Contacts.sln)
* Выполнить мультизапуск проектов

Все базы самостоятельно создаются. Сервер идентификации при старте имеет пользователей:
admin: admin
alice : Pass123$
bob : Pass123$

Можно регистрироваться и создавать своих.
