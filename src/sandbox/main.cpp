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

    int version = SDL_GetVersion();

    std::cout << "SDL version: " << SDL_VERSIONNUM_MAJOR(version) << "." << SDL_VERSIONNUM_MINOR(version) << "." << SDL_VERSIONNUM_MICRO(version) << std::endl;
    std::cout << "Hello, Aquila in C++!" << std::endl;

    SDL_QuitSubSystem(SDL_INIT_VIDEO);
    SDL_Quit();
}
