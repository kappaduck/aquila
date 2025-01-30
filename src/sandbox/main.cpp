// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

#include <iostream>
#include <memory>
#include <SDL3/SDL.h>

template<typename T, auto func>
struct deleter {
    void operator()(T* ptr) {
        func(ptr);
    }
};

using window_ptr = std::unique_ptr<SDL_Window, deleter<SDL_Window, SDL_DestroyWindow>>;

int main(int argc, char** argv) {
    SDL_Init(SDL_INIT_VIDEO);

    window_ptr window{SDL_CreateWindow("Hello, World!", 800, 600, SDL_WINDOW_RESIZABLE)};
    SDL_Surface* surface = SDL_GetWindowSurface(window.get());

    SDL_FillSurfaceRect(surface, nullptr, SDL_MapSurfaceRGB(surface, 0xFF, 0xFF, 0xFF));

    SDL_UpdateWindowSurface(window.get());

    bool running = true;
    SDL_Event e;

    while (running) {
        while (SDL_PollEvent(&e)) {
            if (e.type == SDL_EventType::SDL_EVENT_QUIT) {
                running = false;
            }
        }
    }

    SDL_QuitSubSystem(SDL_INIT_VIDEO);
    SDL_Quit();

    return 0;
}
