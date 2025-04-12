import { Module } from "@nestjs/common";
import { RabbitMQModule } from "@golevelup/nestjs-rabbitmq";
import { ConfigModule } from "@nestjs/config";

@Module({
  imports: [
    ConfigModule.forRoot(),
    RabbitMQModule.forRoot({
      uri: process.env.RABBITMQ_CONNECTION_STRING,
      connectionInitOptions: { wait: false },
      connectionManagerOptions: {
        connectionOptions: {
          clientProperties: {
            connection_name: "Certificates",
          },
        },
      },
    }),
  ],
  exports: [RabbitMQModule],
})
export class RabbitModule {}
