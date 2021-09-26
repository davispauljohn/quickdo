﻿using Microsoft.Extensions.DependencyInjection;
using quickdo_terminal.Interfaces;
using System;

namespace quickdo_terminal
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var serviceProvider = new ServiceCollection()
                .AddTransient<IInputService, InputService>()
                .AddTransient<IDocumentRepository, JsonRepository>()
                .AddTransient<IDocumentService, DocumentService>()
                .BuildServiceProvider();

            var inputService = serviceProvider.GetService<IInputService>();

            inputService.ParseAndRunInput(args)
                .ForEach(o =>
                {
                    Console.ForegroundColor = o.Colour;
                    Console.WriteLine(o.Text);
                    Console.ResetColor();
                });
        }
    }
}