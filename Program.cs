using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration; 
using Azure.Storage.Queues; 
using Azure.Storage.Queues.Models; 

namespace QueueApp
{
    class Program
    {
        static void Main(string[] args)
        {
             void InsertMessage(string queueName, string message)
            {
                // Получаем строку подключения из настроек приложения
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Создаем переменную, которая будет использоваться для создания и управления очередью 
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                // Если очереди нет, создаем её
                queueClient.CreateIfNotExists();

                if (queueClient.Exists())
                {
                    // Отправляем сообщение в очередь
                    queueClient.SendMessage(message);
                }

                Console.WriteLine($"Отправлено: {message}");
            }
            void PeekMessage(string queueName)
            {
                // Получаем строку подключения из настроек приложения
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Создаем переменную, которая будет использоваться для управления очередью 
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                if (queueClient.Exists())
                {
                    // Посмотреть следующее сообщение в очереди
                    PeekedMessage[] peekedMessage = queueClient.PeekMessages();

                    // Отображаем сообщение
                    Console.WriteLine($"Сообщение: '{peekedMessage[0].Body}'");
                }
            }
             void DequeueMessage(string queueName)
            {
                // Получаем строку подключения из настроек приложения
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Создаем переменную, которая будет использоваться для управления очередью 
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                if (queueClient.Exists())
                {
                    // Получаем следующее сообщение в очереди
                    QueueMessage[] retrievedMessage = queueClient.ReceiveMessages();

                    // Отображаем уведомление об удалении сообщения
                    Console.WriteLine($"Удалено сообщение: '{retrievedMessage[0].Body}'");

                    // Удаляем сообщение
                    queueClient.DeleteMessage(retrievedMessage[0].MessageId, retrievedMessage[0].PopReceipt);
                }

            }
             void GetQueueLength(string queueName)
            {
                // Получаем строку подключения из настроек приложения
                string connectionString = ConfigurationManager.AppSettings["StorageConnectionString"];

                // Создаем переменную, которая будет использоваться для управления очередью 
                QueueClient queueClient = new QueueClient(connectionString, queueName);

                if (queueClient.Exists())
                {
                    QueueProperties properties = queueClient.GetProperties();

                    // Извлекаем примерное количество кэшированных сообщений
                    int cachedMessagesCount = properties.ApproximateMessagesCount;

                    // Отображаем количество сообщений в очереди
                    Console.WriteLine($"Количество сообщений в очереди: {cachedMessagesCount}");
                }
            }
            string SWIT;
            do
            {
                Console.WriteLine("Выберите функцию: " +
      " 1 - Добавить сообщение " +
      " 2 - Просмотреть сообщение " +
      " 3 - Удалить сообщение " +
      " 4 - Просмотреть длину очереди " +
      " X - Закрыть");
                SWIT = Console.ReadLine();
                switch (SWIT)
                {
                    case "1":
                        Console.WriteLine("Введите сообщение:");
                        InsertMessage("practoch", Convert.ToString(Console.ReadLine()));
                        break;
                    case "2":
                        Console.WriteLine("Сообщение:");
                        PeekMessage("practoch");
                        break;
                    case "3":
                        DequeueMessage("practoch");
                        Console.WriteLine("Сообщение удалено");
                        break;
                    case "4":
                        GetQueueLength("practoch");
                        break;
                    case "5":
                        SWIT = "X";
                        break;
                }
            } while (SWIT != "X");
        }
    }
}
