import { MessagesHistoryDto } from "@onied/components/supportChatForUser/messageDtos";

const testCases: MessagesHistoryDto[] = [
  {
    supportNumber: null,
    currentSessionId: null,
    messages: [
      {
        messageId: "87dbe1eb-b8e5-4c0c-abac-7a8784ed5ff0",
        supportNumber: null,
        createdAt: new Date(Date.now()),
        messageText: "open-session 6c27c121-8cf2-4db2-8c74-8834cb735fcd",
        isSystem: true,
        readAt: new Date(Date.now() + 1000),
      },
      {
        messageId: "41e610ac-be32-4817-b432-c32e319d0b9b",
        supportNumber: null,
        createdAt: new Date(Date.now()),
        messageText: "Where is money lebowski",
        isSystem: false,
        readAt: new Date(Date.now() + 1000),
      },
      {
        messageId: "344c247f-44db-4597-9519-646a4f46b397",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 1000),
        messageText: "Lorem ipsum dolor sit amet",
        isSystem: false,
        readAt: null,
      },
      {
        messageId: "c3b47893-5eca-4cd6-9046-9f1a3985d616",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 1000),
        messageText: "close-session",
        isSystem: true,
        readAt: null,
      },
    ],
  },
  {
    supportNumber: null,
    currentSessionId: "4e3ac48f-65c4-421b-9e3c-4c3d3f50a9ce",
    messages: [
      {
        messageId: "87dbe1eb-b8e5-4c0c-abac-7a8784ed5ff0",
        supportNumber: null,
        createdAt: new Date(Date.now()),
        messageText: "open-session 6c27c121-8cf2-4db2-8c74-8834cb735fcd",
        isSystem: true,
        readAt: new Date(Date.now() + 1000),
      },
      {
        messageId: "41e610ac-be32-4817-b432-c32e319d0b9b",
        supportNumber: null,
        createdAt: new Date(Date.now()),
        messageText: "Алло ну что там с деньгами",
        isSystem: false,
        readAt: new Date(Date.now() + 1000),
      },
      {
        messageId: "344c247f-44db-4597-9519-646a4f46b397",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 1000),
        messageText: "С какими деньгами?",
        isSystem: false,
        readAt: new Date(Date.now() + 2000),
      },
      {
        messageId: "2d133ee3-a2ee-4024-b439-dffd41ed3b99",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 1000),
        messageText: "Ты куда звонишь?",
        isSystem: false,
        readAt: new Date(Date.now() + 2000),
      },
      {
        messageId: "7c3d7451-c138-4b01-909e-75a39323353c",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 1000),
        messageText: "close-session",
        isSystem: true,
        readAt: null,
      },
      {
        messageId: "7ee02f22-3516-4da2-99b6-7f7dd71aa359",
        supportNumber: null,
        createdAt: new Date(Date.now() + 1000),
        messageText: "Алло ну что там с деньгами? Ну которые я внес в капитал.",
        isSystem: false,
        readAt: null,
      },
    ],
  },
  {
    supportNumber: 420,
    currentSessionId: "4e3ac48f-65c4-421b-9e3c-4c3d3f50a9ce",
    messages: [
      {
        messageId: "87dbe1eb-b8e5-4c0c-abac-7a8784ed5ff0",
        supportNumber: null,
        createdAt: new Date(Date.now()),
        messageText: "open-session 6c27c121-8cf2-4db2-8c74-8834cb735fcd",
        isSystem: true,
        readAt: new Date(Date.now() + 1000),
      },
      {
        messageId: "41e610ac-be32-4817-b432-c32e319d0b9b",
        supportNumber: null,
        createdAt: new Date(Date.now()),
        messageText: "Алло ну что там с деньгами",
        isSystem: false,
        readAt: new Date(Date.now() + 1000),
      },
      {
        messageId: "344c247f-44db-4597-9519-646a4f46b397",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 1000),
        messageText: "С какими деньгами?",
        isSystem: false,
        readAt: new Date(Date.now() + 2000),
      },
      {
        messageId: "2d133ee3-a2ee-4024-b439-dffd41ed3b99",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 1000),
        messageText: "Ты куда звонишь?",
        isSystem: false,
        readAt: new Date(Date.now() + 2000),
      },
      {
        messageId: "7c3d7451-c138-4b01-909e-75a39323353c",
        supportNumber: null,
        createdAt: new Date(Date.now() + 1000),
        messageText: "close-session",
        isSystem: true,
        readAt: null,
      },
      {
        messageId: "7ee02f22-3516-4da2-99b6-7f7dd71aa359",
        supportNumber: null,
        createdAt: new Date(Date.now() + 1000),
        messageText: "Алло ну что там с деньгами? Ну которые я внес в капитал.",
        isSystem: false,
        readAt: new Date(Date.now() + 2000),
      },
      {
        messageId: "8f424b54-c98c-474b-84ba-807ddae84c4f",
        supportNumber: 420,
        createdAt: new Date(Date.now() + 2000),
        messageText: "Ты пьяный или кто, сынок?",
        isSystem: false,
        readAt: new Date(Date.now() + 3000),
      },
      {
        messageId: "f7158d9f-68db-4427-ad2c-7a50ae725e19",
        supportNumber: 69,
        createdAt: new Date(Date.now() + 2000),
        messageText: "Завяжи лямку!",
        isSystem: false,
        readAt: new Date(Date.now() + 3000),
      },
    ],
  },
];

function GetHistory(index: number): MessagesHistoryDto {
  return testCases[index];
}

export default GetHistory;
