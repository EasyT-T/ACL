/*
   AngelCode Scripting Library
   Copyright (c) 2003-2017 Andreas Jonsson

   This software is provided 'as-is', without any express or implied
   warranty. In no event will the authors be held liable for any
   damages arising from the use of this software.

   Permission is granted to anyone to use this software for any
   purpose, including commercial applications, and to alter it and
   redistribute it freely, subject to the following restrictions:

   1. The origin of this software must not be misrepresented; you
      must not claim that you wrote the original software. If you use
      this software in a product, an acknowledgment in the product
      documentation would be appreciated but is not required.

   2. Altered source versions must be plainly marked as such, and
      must not be misrepresented as being the original software.

   3. This notice may not be removed or altered from any source
      distribution.

   The original version of this library can be located at:
   http://www.angelcode.com/angelscript/

   Andreas Jonsson
   andreas@angelcode.com
*/



//
// as_thread.h
//
// Classes for multi threading support
//

#include "angelscript_ida.h"

class asCThreadLocalData;

class asCThreadManager : public asIThreadManager
{
public:
	static asCThreadLocalData *GetLocalData();
	static int CleanupLocalData();

	static int  Prepare(asIThreadManager *externalThreadMgr);
	static void Unprepare();

protected:
	asCThreadManager();
	~asCThreadManager();

	// No need to use the atomic int here, as it will only be
	// updated within the thread manager's critical section
	int refCount;

	asDWORD tlsKey;
	asCThreadLocalData *tld;
};

template <class T> class asCArray
{
public:
	asCArray();
	asCArray(const asCArray<T> &);
	explicit asCArray(asUINT reserve);
	~asCArray();

	void   Allocate(asUINT numElements, bool keepData);
	void   AllocateNoConstruct(asUINT numElements, bool keepData);
	inline asUINT GetCapacity() const;

	void PushLast(const T &element);
	inline T    PopLast();

	bool   SetLength(asUINT numElements);
	bool   SetLengthNoConstruct(asUINT numElements);
	inline void   SetLengthNoAllocate(asUINT numElements);
	inline asUINT GetLength() const;

	void         Copy(const T*, asUINT count);
	asCArray<T> &operator =(const asCArray<T> &);
	void         SwapWith(asCArray<T> &other);

	inline const T &operator [](asUINT index) const;
	inline T       &operator [](asUINT index);
	inline T       *AddressOf();
	inline const T *AddressOf() const;

	bool Concatenate(const asCArray<T> &);
	void Concatenate(T*, unsigned int count);

	bool Exists(const T &element) const;
	int  IndexOf(const T &element) const;
	void RemoveIndex(asUINT index);          // Removes the entry without reordering the array
	void RemoveValue(const T &element);      // Removes the value without reordering the array
	void RemoveIndexUnordered(asUINT index); // Removes the entry without keeping the order

	bool operator==(const asCArray<T> &) const;
	bool operator!=(const asCArray<T> &) const;

public:
	// These are public to allow external code (e.g. JIT compiler) to do asOFFSET to 
	// access the members directly without having to modify the code to add friend
	T      *array;
	asUINT  length;                  // 32bits is enough for all uses of this array
	asUINT  maxLength;

protected:
	char    buf[2*4*AS_PTR_SIZE];    // Avoid dynamically allocated memory for tiny arrays
};

class asCString
{
public:
	asCString();
	~asCString();

#ifdef AS_CAN_USE_CPP11
	asCString(asCString &&);
	asCString &operator =(asCString &&);
#endif // c++11

	asCString(const asCString &);
	asCString(const char *);
	asCString(const char *, size_t length);
	explicit asCString(char);

	void   Allocate(size_t len, bool keepData);
	void   SetLength(size_t len);
	size_t GetLength() const;

	void Concatenate(const char *str, size_t length);
	asCString &operator +=(const asCString &);
	asCString &operator +=(const char *);
	asCString &operator +=(char);

	void Assign(const char *str, size_t length);
	asCString &operator =(const asCString &);
	asCString &operator =(const char *);
	asCString &operator =(char);

	asCString SubString(size_t start, size_t length = (size_t)(-1)) const;

	int FindLast(const char *str, int *count = 0) const;

	size_t Format(const char *fmt, ...);

	int Compare(const char *str) const;
	int Compare(const asCString &str) const;
	int Compare(const char *str, size_t length) const;

	char *AddressOf();
	const char *AddressOf() const;
	char &operator [](size_t index);
	const char &operator[](size_t index) const;
	size_t RecalculateLength();

protected:
	unsigned int length;
	union
	{
		char *dynamic;
		char local[12];
	};
};

//======================================================================

class asIScriptContext;

class asCThreadLocalData
{
public:
	asCArray<asIScriptContext *> activeContexts;
	asCString string;

protected:
	friend class asCThreadManager;

	asCThreadLocalData();
	~asCThreadLocalData();
};
