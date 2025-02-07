// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila;
using KappaDuck.Aquila.Events;
using KappaDuck.Aquila.System;
using KappaDuck.Aquila.Video.Windows;

using SDL engine = SDL.Init(SubSystem.Video);

Console.WriteLine($"SDL version: {SDL.GetVersion()}");
Console.WriteLine("Hello, Aquila!");
