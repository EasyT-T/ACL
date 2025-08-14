cmake_minimum_required(VERSION 3.31)
project(erf)

set(ERF_DOTNET_HOST_PATH "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x86/9.0.8/runtimes/win-x86/native"
        CACHE PATH "Path to Microsoft.NETCore.App.Host")
set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreadedDLL")
set(CMAKE_CXX_STANDARD 20)

include_directories("includes")
include_directories(${ERF_DOTNET_HOST_PATH})

add_library(erf SHARED library.cpp)

target_link_directories(erf PRIVATE ${ERF_DOTNET_HOST_PATH})
target_link_libraries(erf PRIVATE "nethost.lib")