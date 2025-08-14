cmake_minimum_required(VERSION 3.31)
project(erf)

set(CMAKE_CXX_STANDARD 20)

include_directories("includes")
add_library(erf SHARED library.cpp)
