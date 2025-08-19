cmake_minimum_required(VERSION 3.31)
project(erf)

set(ERF_DOTNET_HOST_PATH "C:/Program Files/dotnet/packs/Microsoft.NETCore.App.Host.win-x86/9.0.8/runtimes/win-x86/native"
        CACHE PATH "Path to Microsoft.NETCore.App.Host")
set(CMAKE_MSVC_RUNTIME_LIBRARY "MultiThreadedDLL")
set(CMAKE_CXX_STANDARD 20)

include_directories("include" "angelscript")
include_directories(${ERF_DOTNET_HOST_PATH})

set(ANGELSCRIPT_HEADERS
        include/angelscript.h
        angelscript/as_array.h
        angelscript/as_builder.h
        angelscript/as_bytecode.h
        angelscript/as_callfunc.h
        angelscript/as_compiler.h
        angelscript/as_config.h
        angelscript/as_configgroup.h
        angelscript/as_context.h
        angelscript/as_criticalsection.h
        angelscript/as_datatype.h
        angelscript/as_debug.h
        angelscript/as_generic.h
        angelscript/as_map.h
        angelscript/as_memory.h
        angelscript/as_module.h
        angelscript/as_objecttype.h
        angelscript/as_outputbuffer.h
        angelscript/as_parser.h
        angelscript/as_property.h
        angelscript/as_restore.h
        angelscript/as_scriptcode.h
        angelscript/as_scriptengine.h
        angelscript/as_scriptfunction.h
        angelscript/as_scriptnode.h
        angelscript/as_scriptobject.h
        angelscript/as_string.h
        angelscript/as_string_util.h
        angelscript/as_texts.h
        angelscript/as_thread.h
        angelscript/as_tokendef.h
        angelscript/as_tokenizer.h
        angelscript/as_typeinfo.h
        angelscript/as_variablescope.h
)

add_library(erf SHARED
        library.cpp
        ${ANGELSCRIPT_HEADERS}
        angelscript/as_memory.cpp
        angelscript/as_string.cpp
        angelscript/as_string_util.cpp
        angelscript/as_thread.cpp
        library.h
)

target_compile_definitions(erf PRIVATE AS_NO_EXCEPTIONS)

target_link_directories(erf PRIVATE ${ERF_DOTNET_HOST_PATH})
target_link_libraries(erf PRIVATE "nethost.lib")