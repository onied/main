# Бекенд сертификатов

Зона ответственности:

- Рендер сертификата
- Имитация заказа сертификата
- Статус отправки сертификата

Минимальный набор эндпоинтов:

- `GET /certificate?userId=<GUID>&courseId=<GUID>` - Возвращает данные, необходимые для рендера сертификата; недоступен, если courseId не раздает сертификаты.
- `POST /certificate/order` - Ставит сертификат в очередь доставки; возвращает ID заказа; недоступен, если courseId не раздает сертификаты; недоступен, если заказ уже существует и не был завершен; недоступен, если адрес указан неверно или не находится на территории Российской Федерации; недоступен, если при обращении к сервису покупок было обнаружено, что пользователь не покупал заказ сертификатов.
- `GET /certificate/order/<ORDER_ID>/status` - Возвращает статус отправки сертификата.


---

## Description

[Nest](https://github.com/nestjs/nest) framework TypeScript starter repository.

## Installation

```bash
$ npm install
```

## Running the app

```bash
# development
$ npm run start

# watch mode
$ npm run start:dev

# production mode
$ npm run start:prod
```

## Test

```bash
# unit tests
$ npm run test

# e2e tests
$ npm run test:e2e

# test coverage
$ npm run test:cov
```

## Support

Nest is an MIT-licensed open source project. It can grow thanks to the sponsors and support by the amazing backers. If you'd like to join them, please [read more here](https://docs.nestjs.com/support).

## Stay in touch

- Author - [Kamil Myśliwiec](https://kamilmysliwiec.com)
- Website - [https://nestjs.com](https://nestjs.com/)
- Twitter - [@nestframework](https://twitter.com/nestframework)

## License

Nest is [MIT licensed](LICENSE).
