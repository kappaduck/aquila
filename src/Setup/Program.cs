// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

using Cake.Frosting;
using Setup;

CakeHost host = new();

return host.UseContext<SetupArguments>().Run(args);
