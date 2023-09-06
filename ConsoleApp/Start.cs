using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Hosting;
using Services.Contracts;
using Services.Repositories.Abstractions;

namespace ConsoleApp
{
    public class Start : BackgroundService
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;

        public Start(IUserRepository userRepository, IProductRepository productRepository, IShopRepository shopRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _shopRepository = shopRepository ?? throw new ArgumentNullException(nameof(shopRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await PrintUsersTableAsync();
            await PrintProductsTableAsync();
            await PrintShopsTableAsync();
            await Menu();


        }
        #region private methods

        #region PrintTables methods
        private async Task PrintUsersTableAsync()
        {
            var users = await _userRepository.GetAllAsync();
            Console.WriteLine("Таблица пользователей:");
            Console.WriteLine("Id   Name    Age");
            foreach (var user in users)
                Console.WriteLine($"{user.Id}   {user.Name}    {user.Age}");
        }
        private async Task PrintProductsTableAsync()
        {
            var products = await _productRepository.GetAllAsync();
            Console.WriteLine("Таблица товаров:");
            Console.WriteLine("Id   Name    Price  ShopId");
            foreach (var prod in products)
                Console.WriteLine($"{prod.Id}   {prod.Name}    {prod.Price}   {prod.ShopId}");
        }
        private async Task PrintShopsTableAsync()
        {
            var shops = await _shopRepository.GetAllAsync();
            Console.WriteLine("Таблица магазинов:");
            Console.WriteLine("Id   Name    UserId");
            foreach (var shop in shops)
                Console.WriteLine($"{shop.Id}   {shop.Name}    {shop.UserId}");
        }
        #endregion

        private async Task Menu()
        {
            var flag = true;
            while (flag)
            {
                Console.WriteLine("Список таблиц:");
                Console.WriteLine("1-Пользователи");
                Console.WriteLine("2-Товары");
                Console.WriteLine("3-Магазины");
                Console.WriteLine("Для выхода из приложения нажмите-4");
                Console.WriteLine("Выберите таблицу для добавления объекта. Введите номер таблицы:");

                string? input = Console.ReadLine();
                bool result = int.TryParse(input, out var number);
                if (result == true)
                {
                    switch (number)
                    {
                        case 1:
                            await CreateAsync(TypeTable.USERS);
                            break;
                        case 2:
                            await CreateAsync(TypeTable.PRODUCTS);
                            break;
                        case 3:
                            await CreateAsync(TypeTable.SHOPS);
                            break;
                        case 4:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Введён неправильный символ");
                            break;
                    }
                }
                else
                    Console.WriteLine("Введён неправильный символ");
            }
        }
        private async Task CreateAsync(TypeTable typeTable)
        {
            var flag = true;
            Console.WriteLine("Для выхода в главное меню нажмите: 0");
            while (flag)
            {
                string message = GetMessageByCreate(typeTable);
                Console.WriteLine(message);
                string? name = Console.ReadLine();
                if (name == "0")
                    break;
                if (!string.IsNullOrEmpty(name))
                {
                    switch (typeTable)
                    {
                        case TypeTable.USERS:
                            flag = await CreateUserAsync(name);
                            break;
                        case TypeTable.SHOPS:
                            flag = await CreateShopAsync(name);
                            break;
                        case TypeTable.PRODUCTS:
                            flag = await CreateProductAsync(name);
                            break;
                        default:
                            Console.WriteLine("Введён неправильный символ");
                            break;
                    }
                }
                else
                    Console.WriteLine("Введён неправильный символ");
            }
        }
        private async Task<bool> CreateUserAsync(string name)
        {
            Console.WriteLine("Введите возраст пользователя:");
            string? input = Console.ReadLine();
            if (input == "0")
                return false;
            bool result = int.TryParse(input, out var age);
            if (result == true)
            {
                var userDto = new UserDto() { Name = name, Age = age };
                var user = await _userRepository.CreateAsync(_mapper.Map<User>(userDto));
                Console.WriteLine("Добален новый пользователь!");
                Console.WriteLine("Id   Name    Age");
                Console.WriteLine($"{user.Id}   {user.Name}    {user.Age}");
                return false;

            }
            else
                Console.WriteLine("Введён неправильный символ");

            return true;
        }
        private async Task<bool> CreateShopAsync(string name)
        {
            Console.WriteLine("Выберите пользователя-владельца:");
            var users = await _userRepository.GetAllAsync();
            Console.WriteLine("Id   Name");
            foreach (var user in users)
                Console.WriteLine($"{user.Id}   {user.Name}");
            Console.WriteLine("Введите Id пользователя:");
            string? input = Console.ReadLine();
            if (input == "0")
                return false;
            bool result = int.TryParse(input, out var userId);
            if (result == true)
            {
                var shopDto = new ShopDto() { Name = name, UserId = userId };
                var shop = await _shopRepository.CreateAsync(_mapper.Map<Shop>(shopDto));
                Console.WriteLine("Добален новый магазин!");
                Console.WriteLine("Id   Name    UserId");
                Console.WriteLine($"{shop.Id}   {shop.Name}    {shop.UserId}");
                return false;

            }
            else
                Console.WriteLine("Введён неправильный символ");

            return true;
        }
        private async Task<bool> CreateProductAsync(string name)
        {
            Console.WriteLine("Введите стоимость товара:");
            string? inputPrice = Console.ReadLine();
            if (inputPrice == "0")
                return false;

            bool priceResult = decimal.TryParse(inputPrice, out var price);
            if (priceResult == true)
                return await GetFlagByProductAsync(name, price);
            else
                Console.WriteLine("Введён неправильный символ");

            return true;
        }
        private string GetMessageByCreate(TypeTable typeTable)
        {
            switch (typeTable)
            {
                case TypeTable.USERS:
                    return "Введите имя пользователя:";
                case TypeTable.SHOPS:
                    return "Введите название магазина:";
                case TypeTable.PRODUCTS:
                    return "Введите название продукта:";
                default: throw new Exception("Неизвестный параметр");
            }
        }
        private async Task<bool> GetFlagByProductAsync(string name, decimal price)
        {
            Console.WriteLine("Выберите магазин:");
            var shops = await _shopRepository.GetAllAsync();
            Console.WriteLine("Id   Name    UserId");
            foreach (var shop in shops)
                Console.WriteLine($"{shop.Id}   {shop.Name}    {shop.UserId}");

            Console.WriteLine("Введите Id магазина:");
            string? input = Console.ReadLine();
            if (input == "0")
                return false;
            bool result = int.TryParse(input, out var shopId);
            if (result == true)
            {
                var productDto = new ProductDto() { Name = name, Price = price, ShopId = shopId };
                var prod = await _productRepository.CreateAsync(_mapper.Map<Product>(productDto));
                Console.WriteLine("Добален новый товар!");
                Console.WriteLine("Id   Name    Price  ShopId");
                Console.WriteLine($"{prod.Id}   {prod.Name}    {prod.Price}   {prod.ShopId}");
                return false;

            }
            else
                Console.WriteLine("Введён неправильный символ");

            return true;
        }
        private enum TypeTable
        {
            USERS,
            PRODUCTS,
            SHOPS
        }
        #endregion
    }
}
