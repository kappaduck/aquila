// Copyright (c) KappaDuck. All rights reserved.
// The source code is licensed under MIT License.

#include <iostream>
#include <memory>
#include <SDL3/SDL.h>

template<typename T, void(*destroy)(T*)>
struct deleter {
    void operator()(T* ptr) const {
        destroy(ptr);
    }
};

using window_ptr = std::unique_ptr<SDL_Window, deleter<SDL_Window, SDL_DestroyWindow>>;

int main() {
    SDL_Init(SDL_INIT_VIDEO);
    window_ptr window(SDL_CreateWindow("Aquila sandbox", 1080, 720, SDL_WINDOW_RESIZABLE | SDL_WINDOW_MINIMIZED));

    bool isOpen = true;
    SDL_Event event;

    while (isOpen) {
        while (SDL_PollEvent(&event)) {
            if (event.type == SDL_EventType::SDL_EVENT_QUIT) {
                isOpen = false;
            }
        }
    }

    SDL_QuitSubSystem(SDL_INIT_VIDEO);
    SDL_Quit();
}
