import { Module } from "@nestjs/common";
import { RabbitMQModule } from "@golevelup/nestjs-rabbitmq";
import { ConfigModule } from "@nestjs/config";

@Module({
  imports: [
    ConfigModule.forRoot(),
    RabbitMQModule.forRoot({
      uri: process.env.RABBITMQ_CONNECTION_STRING,
      connectionInitOptions: { wait: true, timeout: 10000 },
      connectionManagerOptions: {
        connectionOptions: {
          clientProperties: {
            connection_name: "Certificates",
          },
          timeout: 30000,
        },
      },
    }),
  ],
  exports: [RabbitMQModule],
})
export class RabbitModule {}
