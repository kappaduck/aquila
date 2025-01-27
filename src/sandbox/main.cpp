#include <iostream>
#include <SDL3/SDL.h>

bool userRequestQuit(const SDL_Event& event) {
    return event.type == SDL_EventType::SDL_EVENT_QUIT ||
          (event.type == SDL_EventType::SDL_EVENT_KEY_DOWN && event.key.key == SDLK_ESCAPE);
}

int main(int argc, char** argv) {
    SDL_Window* window = nullptr;

    SDL_Init(SDL_INIT_VIDEO);

    window = SDL_CreateWindow("Hello, Aquila in C++", 640, 480, SDL_WINDOW_RESIZABLE);

    bool running = true;
    SDL_Event event;

    while (running) {
        while (SDL_PollEvent(&event)) {
            if (userRequestQuit(event)) {
                running = false;
                break;
            }

            if (event.type == SDL_EventType::SDL_EVENT_MOUSE_BUTTON_DOWN) {
                std::cout << "Mouse button down at (" << event.button.x << ", " << event.button.y << ")" << std::endl;
            }
        }
    }

    SDL_DestroyWindow(window);

    SDL_QuitSubSystem(SDL_INIT_VIDEO);
    SDL_Quit();

    return 0;
}
