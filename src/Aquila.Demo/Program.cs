// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using KappaDuck.Aquila;
using KappaDuck.Aquila.System;

using SDLEngine engine = SDLEngine.Init(SubSystem.Video);

Console.WriteLine($"SDL version: {SDLEngine.GetVersion()}");
Console.WriteLine("Hello, Aquila!");
